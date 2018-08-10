using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Needs
{
    [Route("[controller]/[action]")]
    public class NeedsController : UboraController
    {
        public IActionResult First()
        {
            return View();
        }

        public IActionResult Second()
        {
            return View();
        }

        public IActionResult Third()
        {
            return View();
        }

        public IActionResult Fourth()
        {
            return View();
        }
    }
}
