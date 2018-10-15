using BierAlyzerWeb.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BierAlyzerWeb.Controllers
{
    public class ArchiveController : Controller
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

        public IActionResult Archive()
        {
            return View();
        }
    }
}