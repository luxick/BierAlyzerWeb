using System.Diagnostics;
using BierAlyzerWeb.Models.Public;
using Microsoft.AspNetCore.Mvc;

namespace BierAlyzerWeb.Controllers
{
    public class PublicController : Controller
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Handles the GET request for occuring errors </summary>
        ///
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Error()
        {
            return View(new ErrorModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the impressum. </summary>
        ///
        /// <remarks>   Andre Beging, 06.05.2018. </remarks>
        ///
        /// <returns>   An IActionResult. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Impressum()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}