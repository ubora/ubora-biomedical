using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListController : UboraController
    {
        public IActionResult Search([FromServices]ProjectListViewModel.Factory modelFactory, SearchModel searchModel, int page = 1)
        {
            var projectListViewModel = modelFactory.CreateForSearch(searchModel, page);

            return View(new SearchViewModel {
                Title = searchModel.Title,
                Tab = searchModel.Tab,
                ByArea = searchModel.ByArea,
                ByStatus = searchModel.ByStatus,
                SortBy = searchModel.SortBy,
                ProjectListViewModel = projectListViewModel
            });
        }

        [HttpPost]
        public IActionResult Search([FromServices]ProjectListViewModel.Factory modelFactory, SearchModel searchModel)
        {
            var projectListViewModel = modelFactory.CreateForSearch(searchModel, 1);

            return View(new SearchViewModel {
                Title = searchModel.Title,
                Tab = searchModel.Tab,
                ByArea = searchModel.ByArea,
                ByStatus = searchModel.ByStatus,
                SortBy = searchModel.SortBy,
                ProjectListViewModel = projectListViewModel });
        }

        public class SearchModel
        {
            [StringLength(50)]
            public string Title { get; set; }
            public TabType Tab { get; set; }
            public string ByArea { get; set; }
            public ByStatusFilteringMethod ByStatus { get; set; }
            public SortBy SortBy { get; set; }
        }

        public enum TabType
        {
            MyProjects = 0,
            AllProjects = 1,
        }

        public enum ByStatusFilteringMethod
        {
            All = 0,
            NotDraft = 1,
            Draft = 2
        }

        public enum SortBy
        {
            Newest = 0,
            Oldest = 1
        }
    }
}