using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ubora.Domain.Resources;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Resources
{
    public class ResourceViewModel
    {
        public Guid ResourceId { get; set; }
    }
    
    public class ResourcesController : UboraController
    {
        public IActionResult Index()
        {
            return View(nameof(Index));
        }
        
        public IActionResult Add()
        {
            return View(nameof(Add));
        }
        
        [HttpPost]
        public IActionResult Add(CreateResourceCommand command)
        {
            ExecuteUserCommand(command, Notice.Success("TODO"));
            
            return View(nameof(Add));
        }
        
        [HttpPost]
        public IActionResult Delete(DeleteResourceCommand command)
        {
            ExecuteUserCommand(command, Notice.Success("TODO"));
            
            return View(nameof(Delete));
        }
        
        public IActionResult Read(Guid id)
        {
            var entity = QueryProcessor.FindById<Resource>(id);
            
            return View(nameof(Read));
        }
        
        public IActionResult Edit()
        {
            return View(nameof(Edit));
        }
        
        public IActionResult History()
        {
            return View(nameof(History));
        }
    }
}