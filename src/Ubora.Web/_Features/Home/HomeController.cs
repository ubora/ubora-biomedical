using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Home
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
