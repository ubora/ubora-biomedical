using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features.ProjectList;

namespace Ubora.Web._Features.Home
{
    public class HomeController : UboraController
    {
        [Route("")]
        public IActionResult Index([FromServices]ProjectListViewModel.Factory modelFactory, string returnUrl = null, int page = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("LandingPage");
            }

            if (returnUrl != null)
            {
                return RedirectToLocal(returnUrl);
            }

            //var model = modelFactory.CreatePagedProjectListViewModel(header: "Public projects", page: page);

            return View("Index", new IndexViewModel() { ProjectListViewModel = null });
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
