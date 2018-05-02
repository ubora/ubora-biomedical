using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Home
{
    public class HomeController : UboraController
    {
        [Route("")]
        public IActionResult Index(string returnUrl = null, int page = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("LandingPage");
            }

            if (returnUrl != null)
            {
                return RedirectToLocal(returnUrl);
            }

            return View("Index", new IndexViewModel() { Page = page });
        }

        public IActionResult Error()
        {
            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            return View();
        }
    }

    public class IndexViewModel
    {
        public int Page { get; set; }
    }
}
