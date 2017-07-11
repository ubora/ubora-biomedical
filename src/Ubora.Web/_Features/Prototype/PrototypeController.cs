using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Prototype
{
    public class PrototypeController : UboraController
    {
        public IActionResult Messages()
        {
            return View();
        }
    }
}
