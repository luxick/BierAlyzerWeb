using System;
using System.Collections.Generic;
using System.Linq;
using BierAlyzerWeb.Helper;
using BierAlyzerWeb.Models.Management;
using Contracts.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BierAlyzerWeb.Controllers
{
    public class ManagementController : Controller
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

            if (!context.HttpContext.CheckUserType(UserType.Admin))
            {
                context.Result = RedirectToAction("Events", "Home");
            }

            base.OnActionExecuting(context);
        }

        #endregion

        #region Event (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Event View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Event(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction("Events");

            var model = new ManageEventModel();

            using (var context = ContextHelper.OpenContext())
            {
                var contextEvent = context.Event
                    .Include(e => e.EventUsers)
                    .FirstOrDefault(e => e.EventId == id);

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
            if (model == null) return RedirectToAction("Events");
            if (model.EventId == Guid.Empty) return RedirectToAction("Events");
            if (!ModelState.IsValid) return RedirectToAction("Event", new { id = model.EventId });

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
                    contextEvent.Type = model.Type;

                    context.SaveChanges();
                }
            }

            return RedirectToAction("Event", new { id = model.EventId });
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
            var model = new ManageEventsModel
            {
                EventStart = DateTime.Today,
                EventEnd = DateTime.Today.AddDays(1)
            };

            using (var context = ContextHelper.OpenContext())
            {
                model.Events = context.Event.Include(e => e.EventUsers).Include(e => e.Owner).ToList();
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
            var user = HttpContext.GetUser();

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
        /// <summary>   Handles GET requests for the User View </summary>
        /// <remarks>   Andre Beging, 29.04.2018. </remarks>
        /// <param name="id">   The identifier. </param>
        /// <param name="successMessages"></param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public new IActionResult User(Guid id, List<string> successMessages = null)
        {
            if (id == Guid.Empty) return RedirectToAction("Users");

            var model = new ManageUserModel();

            using (var context = ContextHelper.OpenContext())
            {
                var contextUser = context.User
                    .Include(e => e.DrinkEntries)
                    .Include(e => e.UserEvents)
                    .FirstOrDefault(e => e.UserId == id);

                if (contextUser == null) return RedirectToAction("Users");

                model.UserId = contextUser.UserId;
                model.Mail = contextUser.Mail;
                model.Type = contextUser.Type;
                model.Name = contextUser.Username;
                model.Origin = contextUser.Origin;
            }

            successMessages = successMessages ?? new List<string>();
            ViewData["SuccessMessages"] = successMessages;

            return View(model);
        }

        #endregion

        #region User (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the User View </summary>
        ///
        /// <remarks>   Andre Beging, 06.05.2018. </remarks>
        ///
        /// <param name="model">    The model. </param>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public new IActionResult User(ManageUserModel model)
        {
            var successMessages = new List<string>();

            var changePassword = ModelState.GetValidationState("Password") == ModelValidationState.Valid
                && ModelState.GetValidationState("PasswordConfirmation") == ModelValidationState.Valid;

            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirmation");

            if (ModelState.IsValid || changePassword)
            {
                using (var context = ContextHelper.OpenContext())
                {
                    var contextUser = context.User.FirstOrDefault(u => u.UserId == model.UserId);
                    if (contextUser == null) return RedirectToAction("Users");

                    if (ModelState.IsValid)
                    {
                        contextUser.Username = model.Name;
                        contextUser.Origin = model.Origin;
                        contextUser.Type = model.Type;

                        successMessages.Add("Der User wurde gespeichert.");
                    }

                    if (changePassword)
                    {
                        var salt = AuthenticationHelper.GenerateSalt();
                        var hash = AuthenticationHelper.CalculatePasswordHash(salt, model.Password);

                        contextUser.Salt = salt;
                        contextUser.Hash = hash;

                        successMessages.Add("Das Passwort wurde geändert.");
                    }

                    contextUser.Modified = DateTime.Now;
                    context.SaveChanges();
                }

                SharedProperties.OutdatedObjects.Add(model.UserId);
            }
            else
            {
                return View(model);
            }


            return RedirectToAction("User", new { id = model.UserId, successMessages });
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
            var user = HttpContext.GetUser();
            if (user == null) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var drink = new Drink
                {
                    Amount = model.DrinkAmount,
                    Percentage = model.DrinkPercentage,
                    Name = model.DrinkName,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Visible = true,
                    OwnerId = user.UserId
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
        /// <param name="successMessage"></param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Drink(Guid id, string successMessage = null)
        {
            if (id == Guid.Empty) return RedirectToAction("Drinks");

            var model = new ManageDrinkModel();

            using (var context = ContextHelper.OpenContext())
            {
                var contextDrink = context.Drink
                    .Include(d => d.DrinkEntries)
                    .FirstOrDefault(e => e.DrinkId == id);

                if (contextDrink == null) return RedirectToAction("Drinks");

                model.DrinkId = contextDrink.DrinkId;
                model.Name = contextDrink.Name;
                model.Visible = contextDrink.Visible;
                model.Amount = contextDrink.Amount;
                model.Percentage = contextDrink.Percentage;
                model.UsageCount = contextDrink.DrinkEntries.Count;
            }

            if (successMessage != null)
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            return View(model);
        }

        #endregion

        #region Drink (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the Drink View </summary>
        ///
        /// <remarks>   Andre Beging, 11.05.2018. </remarks>
        ///
        /// <param name="model">    The model. </param>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult Drink(ManageDrinkModel model)
        {
            string successMessage = null;
            if (ModelState.IsValid)
            {
                using (var context = ContextHelper.OpenContext())
                {
                    var contextDrink = context.Drink.FirstOrDefault(d => d.DrinkId == model.DrinkId);
                    if (contextDrink != null)
                    {
                        contextDrink.Name = model.Name;
                        context.SaveChanges();

                        successMessage = "Getränk gespeichert!";
                    }
                }
            }

            return RedirectToAction("Drink", new { id = model.DrinkId, successMessage });
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


            return RedirectToAction("Events");
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

            return RedirectToAction("Event", new { id = eventId });
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
            using (var context = ContextHelper.OpenContext())
            {
                var contextUser = context.User.FirstOrDefault(u => u.UserId == id);
                if (contextUser == null) return RedirectToAction("Users");

                contextUser.Enabled = !contextUser.Enabled;
                context.SaveChanges();
            }

            SharedProperties.OutdatedObjects.Add(id);

            return RedirectToAction("Users");
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