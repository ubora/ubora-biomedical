using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Resources.Commands;
using Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models;
using Ubora.Web._Areas.ResourcesArea._Shared;
using Ubora.Web._Features.Projects.Workpackages.SideMenu;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Domain.Resources.Queries;
using Ubora.Web._Areas.ResourcesArea.ResourcesMenus;
using Ubora.Web._Areas.ResourcesArea._Shared.Models;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories
{
    public class ResourceCategoriesController : ResourcesAreaController
    {
        [HttpGet("categories")]
        public IActionResult Index()
        {
            var sideMenuFactory = new ResourcesHierarchySideMenuFactory(Url);
            var root = QueryProcessor.ExecuteQuery(new FindResourceMenuRootQuery());

            var model = new SideMenuViewModel
            {
                TopLevelMenuItems = sideMenuFactory.CreateSideMenuItems(root)
            };

            return View(model);
        }

        [HttpGet("categories/create")]
        public IActionResult Create()
        {
            return View(new CreateResourceCategoryPostModel());
        }

        [HttpPost("categories/create")]
        public IActionResult Create(CreateResourceCategoryPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Create();
            }

            ExecuteUserCommand(new CreateResourceCategoryCommand
            {
                Title = model.Title,
                CategoryId = Guid.NewGuid(),
                Description = model.Description,
                MenuPriority = model.MenuPriority,
                ParentCategoryId = model.ParentCategoryId
            }, Notice.Success("TODO"));

            if (!ModelState.IsValid)
            {
                return Create();
            }

            return RedirectToAction(nameof(ResourcesMenusController.HighestPriorityResourcePage), nameof(ResourcesMenusController).RemoveSuffix());
        }

        [HttpGet("categories/test")]
        public IActionResult RadioButtons()
        {
            return ViewComponent(typeof(ResourceCategorySelectionViewComponent));
        }
    }
}