using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Components;
using Ubora.Domain.Projects._SortSpecifications;
using Ubora.Web._Features._Shared.Paging;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListViewModel
    {
        protected ProjectListViewModel()
        {
        }

        public Pager Pager { get; set; }
        public string Header { get; protected set; }
        public IEnumerable<ProjectCardViewModel> Projects { get; protected set; }
        public bool ShowDefaultMessage { get; protected set; }
        public bool ShowProjectsNotFoundMessage { get; protected set; }

        public class Factory
        {
            private readonly IMapper _mapper;
            private readonly IQueryProcessor _queryProcessor;
            private readonly ImageStorageProvider _imageStorage;
            private readonly ProjectCardViewModel.Factory _projectCardViewModelFactory;

            public Factory(
                IQueryProcessor queryProcessor,
                IMapper mapper,
                ImageStorageProvider imageStorage,
                ProjectCardViewModel.Factory projectCardViewModelFactory)
            {
                _queryProcessor = queryProcessor;
                _mapper = mapper;
                _imageStorage = imageStorage;
                _projectCardViewModelFactory = projectCardViewModelFactory;
            }

            public ProjectListViewModel CreatePagedProjectListViewModel(string header, int page)
            {
                var projects = _queryProcessor.Find<Project>(new MatchAll<Project>(), new SortByTitleSpecification(), 24, page);

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Pager = Pager.From(projects),
                    Projects = projects.Select(project => _projectCardViewModelFactory.Create(project))
                };

                return model;
            }

            public ProjectListViewModel Create(string header, Guid userId)
            {
                var userProjects = _queryProcessor.Find<Project>(new HasMember(userId));

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Projects = userProjects.Select(project => _projectCardViewModelFactory.Create(project)),
                    ShowDefaultMessage = true
                };

                return model;
            }

            public ProjectListViewModel CreateForSearch(string title, int page)
            {
                if (string.IsNullOrEmpty(title))
                {
                    return CreatePagedProjectListViewModel(header: "All projects", page: page);
                }

                var projects = _queryProcessor.Find(new BySearchPhrase(title), 24, page);

                var model = new ProjectListViewModel();
                if (!projects.Any())
                {
                    model.ShowProjectsNotFoundMessage = true;
                }

                model.Pager = Pager.From(projects);
                model.Projects = projects.Select(project => _projectCardViewModelFactory.Create(project));

                return model;
            }
        }
    }
}