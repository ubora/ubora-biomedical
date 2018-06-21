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
using static Ubora.Web._Features.ProjectList.ProjectListController;
using Ubora.Domain.Infrastructure;

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

            public ProjectListViewModel CreatePagedProjectListViewModel(SearchModel searchModel, string header, int page)
            {
                var sortSpecifications = new List<ISortSpecification<Project>>();
                switch (searchModel.SortBy)
                {
                    case SortBy.Newest:
                        sortSpecifications.Add(new SortByCreatedDateTimeSpecfication(SortOrder.Ascending));
                        break;
                    case SortBy.Oldest:
                        sortSpecifications.Add(new SortByCreatedDateTimeSpecfication(SortOrder.Descending));
                        break;
                }

                var specification = CombineSpecificationMethods(false, searchModel);
                var projects = _queryProcessor.Find<Project>(specification, new SortByMultipleProjectSpecification(sortSpecifications), 4, page);

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

            public ProjectListViewModel CreateForSearch(SearchModel searchModel, int page)
            {
                if (string.IsNullOrEmpty(searchModel.Title))
                {
                    return CreatePagedProjectListViewModel(searchModel: searchModel, header: "All projects", page: page);
                }

                var sortSpecifications = new List<ISortSpecification<Project>>();
                switch (searchModel.SortBy)
                {
                    case SortBy.Newest:
                        sortSpecifications.Add(new SortByCreatedDateTimeSpecfication(SortOrder.Ascending));
                        break;
                    case SortBy.Oldest:
                        sortSpecifications.Add(new SortByCreatedDateTimeSpecfication(SortOrder.Descending));
                        break;
                }

                var specification = CombineSpecificationMethods(true, searchModel);
                var projects = _queryProcessor.Find(specification, new SortByMultipleProjectSpecification(sortSpecifications), 4, page);

                var model = new ProjectListViewModel();
                if (!projects.Any())
                {
                    model.ShowProjectsNotFoundMessage = true;
                }

                model.Pager = Pager.From(projects);
                model.Projects = projects.Select(project => _projectCardViewModelFactory.Create(project));

                return model;
            }

            private AndSpecification<Project> CombineSpecificationMethods(bool isSearching, SearchModel searchModel)
            {
                var specifications = new List<Specification<Project>>();

                if (isSearching)
                {
                    specifications.Add(new BySearchPhrase(searchModel.Title));
                }
                else
                {
                    specifications.Add(new MatchAll<Project>());
                }

                if (!string.IsNullOrEmpty(searchModel.ByArea))
                {
                    specifications.Add(new IsAreaSpec(searchModel.ByArea));
                }

                if (searchModel.ByStatus == ByStatusFilteringMethod.Draft)
                {
                    specifications.Add(new IsDraftSpec());
                }
                else if (searchModel.ByStatus == ByStatusFilteringMethod.NotDraft)
                {
                    specifications.Add(new NotSpecification<Project>(new IsDraftSpec()));
                }

                return new AndSpecification<Project>(specifications.ToArray());
            }
        }
    }
}