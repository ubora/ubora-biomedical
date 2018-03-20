using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Home
{
	public class HomeController : UboraController
	{
	    [Route("")]
        public IActionResult Index(string returnUrl = null)
		{
		    if (!User.Identity.IsAuthenticated)
		    {
		        return LandingPage();
            }

            if (returnUrl != null)
		    {
		        return RedirectToLocal(returnUrl);
		    }

            return View();
        }

        public IActionResult LandingPage()
        {
            return View(nameof(LandingPage));
        }

        public IActionResult Error()
        {
            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            return View();
        }
    }
}
