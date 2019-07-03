using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models;
using Ubora.Web._Areas.ResourcesArea._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories
{
    [Route("resources/categories")]
    public class ResourceCategoriesController : ResourcesAreaController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["SelectedSideMenuOption"] = "resource-categories";
        }

        [HttpGet("create")]
        [Authorize(Policies.CanManageResources)]
        public IActionResult Create()
        {
            return View(new CreateResourceCategoryPostModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateResourceCategoryPostModel model)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResources))
            {
                return Forbid();
            }

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
            }, Notice.Success(SuccessTexts.ResourceCategoryCreated));

            if (!ModelState.IsValid)
            {
                return Create();
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet("")]
        [Authorize(Policies.CanManageResources)]
        public IActionResult List()
        {
            var model = new ListResourceCategoryViewModel
            {
                Categories = QueryProcessor.Find(new MatchAll<ResourceCategory>(), new ListItemResourceCategoryViewModel.Projection()).ToList(),
            };

            return View(model);
        }

        [HttpGet("selection")]
        public IActionResult FormSelectOptions(Guid? selectedCategory)
        {
            return ViewComponent(typeof(ResourceCategorySelectOptionsViewComponent), new { selectedCategory });
        }

        [HttpGet("edit")]
        [Authorize(Policies.CanManageResources)]
        public virtual IActionResult Edit(Guid resourceCategoryId, [FromServices]EditResourceCategoryViewModel.Factory modelFactory)
        {
            var category = QueryProcessor.FindById<ResourceCategory>(resourceCategoryId);
            if (category == null)
            {
                return NotFound();
            }

            return View(modelFactory.Create(category));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditResourceCategoryPostModel model, [FromServices]EditResourceCategoryViewModel.Factory modelFactory)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResources))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Edit(model.CategoryId, modelFactory);
            }

            ExecuteUserCommand(new EditResourceCategoryCommand
            {
                CategoryId = model.CategoryId,
                Description = model.Description,
                MenuPriority = model.MenuPriority,
                ParentCategoryId = model.ParentCategoryId,
                Title = model.Title
            }, Notice.Success(SuccessTexts.ResourceCategoryEdited));

            if (!ModelState.IsValid)
            {
                return Edit(model.CategoryId, modelFactory);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpPost("delete")]
        [Authorize(Policies.CanManageResources)]
        public async Task<IActionResult> Delete(DeleteResourceCategoryPostModel model, [FromServices]EditResourceCategoryViewModel.Factory modelFactory)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResources))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Edit(model.ResourceCategoryId, modelFactory);
            }

            ExecuteUserCommand(new DeleteResourceCategoryCommand
            {
                ResourceCategoryId = model.ResourceCategoryId
            }, Notice.Success(SuccessTexts.ResourceCategoryDeleted));

            if (!ModelState.IsValid)
            {
                return Edit(model.ResourceCategoryId, modelFactory);
            }

            return RedirectToAction(nameof(List));
        }
    }
}