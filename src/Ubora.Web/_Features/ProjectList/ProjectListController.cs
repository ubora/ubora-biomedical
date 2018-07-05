using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;
using Ubora.Web._Features.ProjectList.Models;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListController : UboraController
    {
        [Route("projects", Order = 0)]
        [Route("projects/search", Order = 1, Name = "ProjectsSearch")]
        public IActionResult Search([FromServices]ProjectListViewModel.Factory modelFactory, SearchModel searchModel, int page = 1)
        {
            var projectListViewModel = modelFactory.CreateForSearch(searchModel, page);
    
            return View(nameof(Search), new SearchViewModel
            {
                Title = searchModel.Title,
                Tab = searchModel.Tab,
                ByPotentialTechnologyTags = searchModel.ByPotentialTechnologyTags,
                ByClinicalNeedTags = searchModel.ByClinicalNeedTags,
                ByArea = searchModel.ByArea,
                ByStatus = searchModel.ByStatus,
                SortBy = searchModel.SortBy,
                ProjectListViewModel = projectListViewModel
            });
        }
    }

    public class SearchModel
    {
        [StringLength(50)]
        public string Title { get; set; }
        public TabType Tab { get; set; }
        //[BindProperty(BinderType = typeof(CommaSeparatedValuesModelBinder))]
        public int[] ByPotentialTechnologyTags { get; set; } = new int[]{};
        //[BindProperty(BinderType = typeof(CommaSeparatedValuesModelBinder))]
        public int[] ByClinicalNeedTags { get; set; } = new int[]{};
        //[BindProperty(BinderType = typeof(CommaSeparatedValuesModelBinder))]
        public int[] ByArea { get; set; } = new int[]{};
        public ByStatusFilteringMethod ByStatus { get; set; }
        public SortBy SortBy { get; set; }
    }
}