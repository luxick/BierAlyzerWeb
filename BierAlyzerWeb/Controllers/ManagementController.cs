using System;
using System.Linq;
using BierAlyzerWeb.Helper;
using BierAlyzerWeb.Models.Management;
using Contracts.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BierAlyzerWeb.Controllers
{
    public class ManagementController : Controller
    {
        #region Event (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Event View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Event(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events");

            if (id == Guid.Empty) return RedirectToAction("Events");

            var model = new ManageEventModel();

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event.Include(e => e.EventUsers).FirstOrDefault(e => e.EventId == id);
                if (contextEvent == null) return RedirectToAction("Events");

                model.EventId = contextEvent.EventId;
                model.Start = contextEvent.Start;
                model.End = contextEvent.End;
                model.Name = contextEvent.Name;
                model.Description = contextEvent.Description;
                model.Code = contextEvent.Code;
                model.Type = contextEvent.Type;
                model.Status = contextEvent.Status;
                model.UserCount = contextEvent.EventUsers.Count;

                return View(model);
            }
        }

        #endregion

        #region Event (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the Event View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult Event(ManageEventModel model)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            if (model == null) return RedirectToAction("Events");
            if (model.EventId == Guid.Empty) return RedirectToAction("Events");
            if (!ModelState.IsValid) return RedirectToAction("Event", new {id = model.EventId});

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event.FirstOrDefault(e => e.EventId == model.EventId);

                if (contextEvent != null)
                {
                    if (!string.IsNullOrWhiteSpace(model.Name))
                        contextEvent.Name = model.Name;

                    contextEvent.Description = model.Description;
                    contextEvent.Start = model.Start;
                    contextEvent.End = model.End;

                    context.SaveChanges();
                }
            }

            return RedirectToAction("Event", new {id = model.EventId});
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
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            var model = new ManageEventsModel
            {
                EventStart = DateTime.Today,
                EventEnd = DateTime.Today.AddDays(5)
            };

            using (var context = ContextHelper.OpenContext())
            {
                model.Events = context.Event.Include(e => e.EventUsers).ToList();
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
        public IActionResult Events(ManageEventsModel model)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

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
                        Type = EventType.Private
                    };

                    context.Event.Add(newEvent);
                    context.SaveChanges();
                }

                return RedirectToAction("Events");
            }

            return View(model);
        }

        #endregion

        #region Users (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Users View </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Users()
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            var model = new ManageUsersModel();

            using (var context = ContextHelper.OpenContext())
            {
                model.Users = context.User
                    .Include(u => u.UserEvents)
                    .Include(u => u.DrinkEntries).ThenInclude(de => de.Drink)
                    .OrderByDescending(x => x.Type)
                    .ThenByDescending(x => x.Enabled)
                    .ToList();
            }

            return View(model);
        }

        #endregion

        #region User (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the Users View </summary>
        /// <remarks>   Andre Beging, 29.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public new IActionResult User(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            if (id == Guid.Empty) return RedirectToAction("Users");

            var model = new ManageUserModel();

            using (var context = ContextHelper.OpenContext())
            {
                var contextUser = context.User
                    .Include(e => e.DrinkEntries)
                    .Include(e => e.UserEvents)
                    .FirstOrDefault(e => e.UserId == id);

                if (contextUser == null) return RedirectToAction("Users");

                model.User = contextUser;
            }

            return View(model);
        }

        #endregion

        #region Drinks (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Drinks View </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Drinks()
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            var model = new ManageDrinksModel
            {
                DrinkName = "Bier",
                DrinkAmount = .5,
                DrinkPercentage = 5
            };

            using (var context = ContextHelper.OpenContext())
            {
                model.Drinks = context.Drink.Include(d => d.DrinkEntries).OrderBy(d => d.Name)
                    .ThenByDescending(d => d.Amount).ToList();
            }

            return View(model);
        }

        #endregion

        #region Drinks (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the Drinks View </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult Drinks(ManageDrinksModel model)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            if (ModelState.IsValid)
            {
                var drink = new Drink
                {
                    Amount = model.DrinkAmount,
                    Percentage = model.DrinkPercentage,
                    Name = model.DrinkName,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Visible = true
                };

                using (var context = ContextHelper.OpenContext())
                {
                    context.Add(drink);
                    context.SaveChanges();
                }

                return RedirectToAction("Drinks");
            }

            using (var context = ContextHelper.OpenContext())
            {
                model.Drinks = context.Drink.Include(d => d.DrinkEntries).OrderBy(d => d.Name).ThenBy(d => d.Amount)
                    .ToList();
            }

            return View(model);
        }

        #endregion

        #region Drink (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Drink View </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Drink(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            if (id == Guid.Empty) return RedirectToAction("Drinks");

            var model = new ManageDrinkModel();

            using (var context = ContextHelper.OpenContext())
            {
                var contextDrink = context.Drink.FirstOrDefault(e => e.DrinkId == id);
                if (contextDrink == null) return RedirectToAction("Drinks");

                model.DrinkId = contextDrink.DrinkId;
                model.Name = contextDrink.Name;
                model.Visible = contextDrink.Visible;
                model.Amount = contextDrink.Amount;
                model.Percentage = contextDrink.Percentage;

                return View(model);
            }
        }

        #endregion

        #region SetEventType

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the SetEventType Action </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult SetEventType(string id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            var parameters = id.Split("#");
            if (parameters.Length != 2) return RedirectToAction("Events");
            if (!Guid.TryParse(parameters[0], out var eventId)) return RedirectToAction("Events");
            if (!Enum.TryParse(parameters[1], out EventType type)) return RedirectToAction("Events");

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event.FirstOrDefault(e => e.EventId == eventId);
                if (contextEvent != null)
                {
                    contextEvent.Type = type;
                    context.SaveChanges();
                }
            }


            return RedirectToAction("Event", new {id = eventId});
        }

        #endregion

        #region SetEventStatus

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the SetEventStatus Action </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult SetEventStatus(string id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            var parameters = id.Split("#");
            if (parameters.Length != 2) return RedirectToAction("Events");
            if (!Guid.TryParse(parameters[0], out var eventId)) return RedirectToAction("Events");
            if (!Enum.TryParse(parameters[1], out EventStatus status)) return RedirectToAction("Events");

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event.FirstOrDefault(e => e.EventId == eventId);
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

            return RedirectToAction("Event", new {id = eventId});
        }

        #endregion

        #region ToggleDrinkVisibility

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the ToggleDrinkVisibility Action </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult ToggleDrinkVisibility(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            using (var context = ContextHelper.OpenContext())
            {
                var contextDrink = context.Drink.FirstOrDefault(d => d.DrinkId == id);
                if (contextDrink == null) return RedirectToAction("Drinks");

                contextDrink.Visible = !contextDrink.Visible;
                context.SaveChanges();
            }

            return RedirectToAction("Drinks");
        }

        #endregion

        #region ToggleUserEnabled

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the ToggleUserEnabled Action </summary>
        /// <remarks>   Andre Beging, 28.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult ToggleUserEnabled(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            using (var context = ContextHelper.OpenContext())
            {
                var contextUser = context.User.FirstOrDefault(u => u.UserId == id);
                if (contextUser == null) return RedirectToAction("Users");

                contextUser.Enabled = !contextUser.Enabled;
                context.SaveChanges();
            }

            SharedProperties.OutdatedObjects.Add(id);

            return RedirectToAction("User", new {id});
        }

        #endregion

        #region ToggleUserAdmin

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the ToggleUserAdmin Action </summary>
        /// <remarks>   Andre Beging, 29.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult ToggleUserAdmin(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            using (var context = ContextHelper.OpenContext())
            {
                var contextUser = context.User.FirstOrDefault(u => u.UserId == id);
                if (contextUser == null) return RedirectToAction("Users");

                contextUser.Type = contextUser.Type == UserType.User ? UserType.Admin : UserType.User;

                SharedProperties.OutdatedObjects.Add(contextUser.UserId);
                context.SaveChanges();
            }

            return RedirectToAction("User", new {id});
        }

        #endregion

        #region DeleteDrink

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the DeleteDrink Action </summary>
        ///
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult DeleteDrink(Guid id)
        {
            if (!HttpContext.IsSignedIn()) return RedirectToAction("Login", "Account");
            if (!HttpContext.CheckUserType(UserType.Admin)) return RedirectToAction("Events", "Home");

            using (var context = ContextHelper.OpenContext())
            {
                var contextDrink = context.Drink.Include(d => d.DrinkEntries).FirstOrDefault(d => d.DrinkId == id);
                if (contextDrink == null) return RedirectToAction("Drinks");

                if (contextDrink.DrinkEntries.Count == 0)
                {
                    context.Remove(contextDrink);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Drinks");
        }

        #endregion
    }
}