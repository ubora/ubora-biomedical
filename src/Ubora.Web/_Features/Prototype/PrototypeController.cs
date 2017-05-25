using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Prototype
{
	public class PrototypeController : Controller
	{
		public IActionResult Messages()
		{
			return View();
		}
	}
}
