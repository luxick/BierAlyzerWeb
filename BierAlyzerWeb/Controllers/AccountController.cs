using System;
using System.Linq;
using BierAlyzerWeb.Helper;
using BierAlyzerWeb.Models.Account;
using Contracts.Model;
using Microsoft.AspNetCore.Mvc;

namespace BierAlyzerWeb.Controllers
{
    public class AccountController : Controller
    {
        #region SignUp (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles Get requests for the SignUp View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult SignUp()
        {
            if (HttpContext.IsSignedIn()) return RedirectToAction("Events", "Home");
            return View();
        }

        #endregion

        #region SignUp (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the SignUp View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
                if (AuthenticationHelper.TrySignUp(model, out var user))
                {
                    // tmp direct login
                    using (var context = ContextHelper.OpenContext())
                    {
                        var contextUser = context.User.FirstOrDefault(u => u.UserId == user.UserId);
                        if (contextUser != null)
                        {
                            contextUser.LastLogin = DateTime.Now;
                            context.SaveChanges();
                        }
                    }

                    HttpContext.Session.SetObject("User", user);
                    return RedirectToAction("Events", "Home");

                    ViewData["Success"] = true;
                    ModelState.Clear();
                }

            return View(model);
        }

        #endregion

        #region Login (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the SignUp View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Login()
        {
            if (HttpContext.IsSignedIn()) return RedirectToAction("Events", "Home");
            return View();
        }

        #endregion

        #region Login (POST)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles POST requests for the Login View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="model">    The model. </param>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
                // Check credentials
                if (AuthenticationHelper.LoginCorrect(model.Mail, model.Password, out var user) && user != null)
                {
                    if (user.Type != UserType.Admin && user.Enabled == false)
                    {
                        ViewData["ErrorStatus"] = true;
                        ViewData["ErrorMessage"] =
                            "Hallo Trinker. Du wurdest leider noch nicht freigeschaltet oder bist gesperrt.";
                    }
                    else
                    {
                        using (var context = ContextHelper.OpenContext())
                        {
                            var contextUser = context.User.FirstOrDefault(u => u.UserId == user.UserId);
                            if (contextUser != null)
                            {
                                contextUser.LastLogin = DateTime.Now;
                                context.SaveChanges();
                            }
                        }

                        HttpContext.Session.SetObject("User", user);
                        return RedirectToAction("Events", "Home");
                    }
                }
                else
                {
                    ViewData["ErrorStatus"] = true;
                    ViewData["ErrorMessage"] = "E-Mail oder Passwort nicht korrekt!";
                }

            return View(model);
        }

        #endregion

        #region Logout (GET)

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles GET requests for the Logout View </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        #endregion
    }
}