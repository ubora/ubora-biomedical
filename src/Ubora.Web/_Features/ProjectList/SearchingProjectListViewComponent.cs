﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.ProjectList
{
    public class SearchingProjectListViewComponent : ViewComponent
    {
        private readonly ProjectListViewModel.Factory _modelFactory;

        public SearchingProjectListViewComponent(ProjectListViewModel.Factory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(string title)
        {
            var model = _modelFactory.CreateByTitle(title);

            return View("~/_Features/ProjectList/ProjectListPartial.cshtml", model);
        }
    }
}