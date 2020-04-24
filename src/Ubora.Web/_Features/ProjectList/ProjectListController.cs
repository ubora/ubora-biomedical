using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;
using Ubora.Web._Features.ProjectList.Models;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListController : UboraController
    {
        [ResponseCache(NoStore = true)]
        [Route("projects", Order = 0)]
        [Route("projects/search", Order = 1, Name = "ProjectsSearch")]
        public IActionResult Search([FromServices]ProjectListViewModel.Factory modelFactory, SearchModel model, int page = 1)
        {
            var projectListViewModel = modelFactory.CreatePagedProjectListViewModel(model, "", page);

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (model.Tab == null)
            {
                model.Tab = (User.Identity.IsAuthenticated) ? TabType.MyProjects : TabType.AllProjects;
            }

            var searchViewModel = new SearchViewModel
            {
                Title = model.Title,
                Tab = model.Tab,
                ByPotentialTechnologyTags = model.ByPotentialTechnologyTags,
                ByClinicalNeedTags = model.ByClinicalNeedTags,
                ByArea = model.ByArea,
                ByStatus = model.ByStatus,
                SortBy = model.SortBy,
                ProjectListViewModel = projectListViewModel
            };

            if (isAjax)
            {
                return PartialView("~/_Features/ProjectList/ProjectsTabPartial.cshtml", searchViewModel);
            }
            else
            {
                return View(nameof(Search), searchViewModel);
            }
        }
    }

    public class SearchModel
    {
        [StringLength(50)]
        public string Title { get; set; }

        public TabType? Tab { get; set; }

        [BindProperty(BinderType = typeof(CommaSeparatedValuesModelBinder))]
        public int[] ByPotentialTechnologyTags { get; set; } = new int[] { };

        [BindProperty(BinderType = typeof(CommaSeparatedValuesModelBinder))]
        public int[] ByClinicalNeedTags { get; set; } = new int[] { };

        [BindProperty(BinderType = typeof(CommaSeparatedValuesModelBinder))]
        public int[] ByArea { get; set; } = new int[] { };

        public ByStatusFilteringMethod ByStatus { get; set; }

        public SortBy SortBy { get; set; }
    }
}