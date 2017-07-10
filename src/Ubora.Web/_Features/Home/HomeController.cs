using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Home
{
    public class HomeController : UboraController
    {
        public HomeController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [Route("")]
        public IActionResult Index(string returnUrl = null)
        {
            if (returnUrl != null)
            {
                return RedirectToLocal(returnUrl);
            }

            return View();
        }

        public IActionResult Error()
        {
            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            return View();
        }
    }
}
