using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features.ProjectList.Models;

namespace Ubora.Web._Features.Home
{
    public class HomeController : UboraController
    {
        [Route("")]
        public IActionResult Index(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("LandingPage");
            }

            if (returnUrl != null)
            {
                return RedirectToLocal(returnUrl);
            }

            return View("LandingPage");
        }

        public IActionResult Error()
        {
            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            return View();
        }
    }

    public class IndexViewModel
    {
        public ProjectListViewModel ProjectListViewModel { get; set; }
    }
}
