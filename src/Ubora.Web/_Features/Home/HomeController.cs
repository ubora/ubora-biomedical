using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Home
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Search()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}

		public IActionResult WorkPackageZero()
		{
			return View();
		}

		public IActionResult WorkPackageOne()
		{
			return View();
		}

		public IActionResult ExampleProject()
		{
			return View();
		}
	}
}
