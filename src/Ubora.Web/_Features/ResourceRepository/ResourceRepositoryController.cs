using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ubora.Web._Features.ResourceRepository
{
    public class ResourceRepositoryController : UboraController
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewData["IsResourcesArea"] = true;
        }
        
        public IActionResult Index()
        {
            return View(nameof(Index));
        }
        
        public IActionResult Add()
        {
            return View(nameof(Add));
        }
        
        [HttpPost]
        public IActionResult Add(object model)
        {
            return View(nameof(Add));
        }
        
        public IActionResult Delete()
        {
            return View(nameof(Add));
        }
        
        [HttpPost]
        public IActionResult Delete(object model)
        {
            return View(nameof(Add));
        }
    }
}