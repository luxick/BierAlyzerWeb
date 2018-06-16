using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BierAlyzerWeb.Helper;
using BierAlyzerWeb.Models.Home;
using BierAlyzerWeb.Models.Management;
using Contracts.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BierAlyzerWeb.Controllers
{
    public class HomeController : Controller
    {
        #region OnActionExecuting
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Executes the action executing action. </summary>
        ///
        /// <remarks>   Andre Beging, 24.05.2018. </remarks>
        ///
        /// <param name="context">  The context. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.IsSignedIn())
            {
                context.Result = RedirectToAction("Login", "Account");
                return;
            }

            base.OnActionExecuting(context);
        }

        #endregion

        #region Events (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Events View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Events()
        {
            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new EventsModel { User = user };

            using (var context = ContextHelper.OpenContext())
            {
                var publicEvents = context.Event
                    .Where(e => e.Type == EventType.Public && !context.GetUserEvents(user.UserId).Select(u => u.EventId)
                                    .Contains(e.EventId));

                model.PublicEvents = publicEvents.ToList();

                if (context.User.Any(u => u.UserId == user.UserId))
                {
                    var userEvents = context.GetUserEvents(user.UserId).Where(e => e.Type != EventType.Hidden);
                    model.Events = userEvents.ToList();
                }
            }

            return View(model);
        }

        #endregion

        #region Events (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the Events View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult Events(EventsModel model)
        {
            if (ModelState.IsValid)
            {
                var user = HttpContext.GetUser();
                if (user != null)
                    using (var context = ContextHelper.OpenContext())
                    {
                        if (context.User.Any(u => u.UserId == user.UserId))
                        {
                            var contextEvent =
                                context.Event.FirstOrDefault(e => e.Code.ToLower() == model.EventCode.ToLower() &&
                                                                  e.Type != EventType.Hidden);
                            if (contextEvent != null)
                                if (!context.GetUserEvents(user.UserId).Any(e => e.EventId == contextEvent.EventId))
                                {
                                    var userEvent = new UserEvent
                                    {
                                        UserId = user.UserId,
                                        EventId = contextEvent.EventId
                                    };

                                    context.UserEvent.Add(userEvent);
                                    context.SaveChanges();
                                }
                        }
                    }
            }

            return RedirectToAction("Event");
        }

        #endregion

        #region Event (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Event View </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Event(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction("Events");

            var model = new EventModel();


            Event contextEvent;
            using (var context = ContextHelper.OpenContext())
            {
                contextEvent =
                    context.Event
                        .Include(e => e.DrinkEntries).ThenInclude(de => de.Drink)
                        .Include(e => e.DrinkEntries).ThenInclude(de => de.User)
                        .FirstOrDefault(e => e.EventId == id);

                model.Drinks = context.Drink.Where(d => d.Visible).OrderBy(d => d.Name).ThenByDescending(d => d.Amount)
                    .ToList();
                model.Event = contextEvent;
            }

            if (contextEvent == null) return RedirectToAction("Events");

            var eventUsers = new List<EventUserModel>();
            foreach (var drinkEntry in contextEvent.DrinkEntries)
            {
                // Drink Usage
                var drinkDisplayName = string.Format("{0} ({1} l)", drinkEntry.Drink.Name, drinkEntry.Drink.Amount);
                if (model.DrinkUsage.ContainsKey(drinkDisplayName))
                {
                    model.DrinkUsage[drinkDisplayName] += 1;
                }
                else
                {
                    model.DrinkUsage.Add(drinkDisplayName, 1);
                }

                // EventUser
                var eventUser = eventUsers.FirstOrDefault(eu => eu.User.UserId == drinkEntry.UserId);
                if (eventUser == null)
                {
                    eventUsers.Add(new EventUserModel
                    {
                        User = drinkEntry.User,
                        Amount = drinkEntry.Drink.Amount,
                        AlcoholAmount = drinkEntry.Drink.AlcoholAmount
                    });
                }
                else
                {
                    eventUser.Amount += drinkEntry.Drink.Amount;
                    eventUser.AlcoholAmount += drinkEntry.Drink.AlcoholAmount;
                }
            }

            model.DrinkUsage = model.DrinkUsage.OrderByDescending(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            model.EventUsers = eventUsers
                .OrderByDescending(eu => eu.Amount)
                .ThenBy(eu => eu.AlcoholAmount)
                .ThenBy(eu => eu.User.Username)
                .ToList();

            return View(model);
        }

        #endregion

        #region UserProfile (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the UserProfile View </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult UserProfile()
        {
            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new UserProfileModel
            {
                UserId = user.UserId,
                Mail = user.Mail,
                Origin = user.Origin,
                Username = user.Username
            };

            return View(model);
        }

        #endregion

        #region UserProfile (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the UserProfile View </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult UserProfile(UserProfileModel model)
        {
            var changePassword = false;
            if (!string.IsNullOrWhiteSpace(model.Password) || !string.IsNullOrWhiteSpace(model.PasswordConfirmation))
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                    ModelState.AddModelError("Password",
                        "Um das Passwort zu ändern, müssen Passwort und Passwortbestätigung eingetragen werden");

                if (string.IsNullOrWhiteSpace(model.PasswordConfirmation))
                    ModelState.AddModelError("PasswordConfirmation",
                        "Um das Passwort zu ändern, müssen Passwort und Passwortbestätigung eingetragen werden");

                if (!string.IsNullOrWhiteSpace(model.Password) && model.Password.Length < 6)
                    ModelState.AddModelError("Password", "Das Passwort muss mindestens 6 Zeichen lang sein.");

                if (!string.IsNullOrWhiteSpace(model.PasswordConfirmation) && model.PasswordConfirmation.Length < 6)
                    ModelState.AddModelError("PasswordConfirmation",
                        "Das Passwort muss mindestens 6 Zeichen lang sein.");

                if (ModelState.IsValid && model.Password != model.PasswordConfirmation)
                    ModelState.AddModelError("PasswordConfirmation",
                        "Die eingegebenen Passwörter stimmen nicht überein.");

                if (ModelState.IsValid)
                    changePassword = true;
            }

            if (ModelState.IsValid)
            {
                using (var context = ContextHelper.OpenContext())
                {
                    var contextUser = context.User.FirstOrDefault(x => x.UserId == model.UserId);
                    if (contextUser == null) return RedirectToAction("UserProfile");

                    contextUser.Username = model.Username;
                    contextUser.Origin = model.Origin;
                    contextUser.Modified = DateTime.Now;

                    if (changePassword)
                    {
                        var salt = AuthenticationHelper.GenerateSalt();
                        var hash = AuthenticationHelper.CalculatePasswordHash(salt, model.Password);
                        contextUser.Salt = salt;
                        contextUser.Hash = hash;
                    }

                    context.SaveChanges();
                }

                SharedProperties.OutdatedObjects.Add(model.UserId);

                ViewData["Success"] = true;
                ViewData["PasswortChanged"] = changePassword;
            }

            return View(model);
        }

        #endregion

        #region UserEvents (GET)
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   User events. </summary>
        ///
        /// <remarks>   Andre Beging, 25.05.2018. </remarks>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult UserEvents()
        {
            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new ManageEventsModel
            {
                EventStart = DateTime.Today,
                EventEnd = DateTime.Today.AddDays(1)
            };

            using (var context = ContextHelper.OpenContext())
            {
                var events = context.Event
                    .Where(x => x.OwnerId == user.UserId && x.Type != EventType.Public)
                    .Include(e => e.EventUsers)
                    .Include(e => e.Owner)
                    .ToList();

                model.Events = events.OrderBy(e => e.Status).ToList();
            }

            return View(model);
        }

        #endregion

        #region UserEvents (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   (An Action that handles HTTP POST requests) user events. </summary>
        ///
        /// <remarks>   Andre Beging, 25.05.2018. </remarks>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult UserEvents(ManageEventsModel model)
        {
            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                using (var context = ContextHelper.OpenContext())
                {
                    var newEvent = new Event
                    {
                        Name = model.EventName,
                        Description = model.EventDescription,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Code = EventHelper.GenerateCode(),
                        Start = model.EventStart,
                        End = model.EventEnd,
                        Type = EventType.Private,
                        OwnerId = user.UserId
                    };

                    context.Event.Add(newEvent);
                    var changes = context.SaveChanges();

                    if (changes == 1)
                    {
                        return RedirectToAction("UserEvents");
                    }
                }
            }

            return View(model);
        }

        #endregion

        #region UserEvent
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   User event. </summary>
        ///
        /// <remarks>   Andre Beging, 25.05.2018. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult UserEvent(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction("UserEvents");

            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new ManageEventModel();

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event
                    .Include(e => e.EventUsers)
                    .FirstOrDefault(e => e.EventId == id && e.OwnerId == user.UserId && e.Type != EventType.Public);

                if (contextEvent == null) return RedirectToAction("UserEvents");

                model.EventId = contextEvent.EventId;
                model.Start = contextEvent.Start;
                model.End = contextEvent.End;
                model.Name = contextEvent.Name;
                model.Description = contextEvent.Description;
                model.Code = contextEvent.Code;
                model.Status = contextEvent.Status;
                model.UserCount = contextEvent.EventUsers.Count;

                return View(model);
            }
        }

        #endregion

        #region JoinPublicEvent

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the JoinPublicEvent Action </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult JoinPublicEvent(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");

            if (id == Guid.Empty) return RedirectToAction("Events");

            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            using (var context = ContextHelper.OpenContext())
            {
                if (context.User.Any(u => u.UserId == user.UserId) &&
                    context.Event.Any(e => e.EventId == id && e.Type == EventType.Public))
                    if (!context.GetUserEvents(user.UserId).Any(e => e.EventId == id))
                    {
                        var userEvent = new UserEvent
                        {
                            UserId = user.UserId,
                            EventId = id
                        };

                        context.UserEvent.Add(userEvent);
                        context.SaveChanges();
                    }
            }

            return RedirectToAction("Events");
        }

        #endregion

        #region LeaveEvent

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the LeaveEvent Action </summary>
        /// <remarks>   Andre Beging, 29.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult LeaveEvent(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction("Events");

            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            using (var context = ContextHelper.OpenContext())
            {
                var contextUserEvent =
                    context.UserEvent.FirstOrDefault(ue => ue.EventId == id && ue.UserId == user.UserId);
                if (contextUserEvent != null)
                {
                    context.Remove(contextUserEvent);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Events");
        }

        #endregion

        #region BookDrink

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the BookDrink Action </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult BookDrink(string id)
        {
            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var parameters = id.Split("#");
            if (parameters.Length != 2) return RedirectToAction("Events");
            if (!Guid.TryParse(parameters[0], out var eventId)) return RedirectToAction("Events");
            if (!Guid.TryParse(parameters[1], out var drinkId)) return RedirectToAction("Events");

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event.FirstOrDefault(e => e.EventId == eventId);
                if (contextEvent == null) return RedirectToAction("Events");

                if (!context.Drink.Any(e => e.DrinkId == drinkId)) return RedirectToAction("Event", new { id = eventId });

                if (context.GetEventUsers(eventId).Any(u => u.UserId == user.UserId) &&
                    contextEvent.Status == EventStatus.Open)
                {
                    var drinkEntry = new DrinkEntry
                    {
                        DrinkId = drinkId,
                        UserId = user.UserId,
                        EventId = eventId
                    };

                    context.Add(drinkEntry);
                    context.SaveChanges();
                }

                return RedirectToAction("Event", new { id = eventId });
            }
        }

        #endregion

        public IActionResult RemoveEvent(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction("UserEvents");

            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event.FirstOrDefault(e => e.EventId == id && e.OwnerId == user.UserId && e.Type != EventType.Public);
                if (contextEvent != null)
                {
                    context.Remove(contextEvent);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("UserEvent", new { id });
        }

        #region SetEventStatus

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the SetEventStatus Action </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult SetEventStatus(string id)
        {
            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            var parameters = id.Split("#");
            if (parameters.Length != 2) return RedirectToAction("UserEvents");
            if (!Guid.TryParse(parameters[0], out var eventId)) return RedirectToAction("UserEvents");
            if (!Enum.TryParse(parameters[1], out EventStatus status)) return RedirectToAction("UserEvents");

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event.FirstOrDefault(e => e.EventId == eventId && e.OwnerId == user.UserId && e.Type != EventType.Public);
                if (contextEvent != null)
                {
                    if (status == EventStatus.Open)
                    {
                        // Is start in the future?
                        if (contextEvent.Start > DateTime.Now)
                            contextEvent.Start = DateTime.Now.AddMinutes(-1);

                        contextEvent.End = DateTime.Now.AddDays(1);
                    }

                    if (status == EventStatus.Closed)
                    {
                        contextEvent.End = DateTime.Now.AddMinutes(-1);

                        if (contextEvent.Start > contextEvent.End)
                            contextEvent.Start = DateTime.Today;
                    }

                    context.SaveChanges();
                }
            }

            return RedirectToAction("UserEvent", new { id = eventId });
        }

        #endregion
    }
}