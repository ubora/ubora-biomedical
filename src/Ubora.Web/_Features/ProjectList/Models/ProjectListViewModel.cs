using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web._Components;
using Ubora.Domain.Projects._SortSpecifications;
using Ubora.Web._Features._Shared.Paging;
using Ubora.Domain.Infrastructure;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.ProjectList.Models
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
            private readonly IQueryProcessor _queryProcessor;
            private readonly ProjectCardViewModel.Factory _projectCardViewModelFactory;

            public Factory(
                IQueryProcessor queryProcessor,
                ProjectCardViewModel.Factory projectCardViewModelFactory)
            {
                _queryProcessor = queryProcessor;
                _projectCardViewModelFactory = projectCardViewModelFactory;
            }

            public ProjectListViewModel CreatePagedProjectListViewModel(SearchModel searchModel, string header,
                int page)
            {
                var sortSpecifications = new List<ISortSpecification<Project>>();
                switch (searchModel.SortBy)
                {
                    case SortBy.Newest:
                        sortSpecifications.Add(new SortByCreatedDateTimeSpecfication(SortOrder.Descending));
                        break;
                    case SortBy.Oldest:
                        sortSpecifications.Add(new SortByCreatedDateTimeSpecfication(SortOrder.Ascending));
                        break;
                }
                var sortByMultipleSpecification = new SortByMultipleSpecification<Project>(sortSpecifications);
                
                var combinedSpecification = CombineSpecificationMethods(searchModel);
                
                var projects = _queryProcessor.Find<Project>(combinedSpecification, sortByMultipleSpecification, 24, page);

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Pager = Pager.From(projects),
                    Projects = projects.Select(project => _projectCardViewModelFactory.Create(project)),
                    ShowProjectsNotFoundMessage = !projects.Any()
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

            private AndSpecification<Project> CombineSpecificationMethods(SearchModel searchModel)
            {
                var specifications = new List<Specification<Project>>();

                if (!string.IsNullOrEmpty(searchModel.Title))
                {
                    specifications.Add(new BySearchPhrase(searchModel.Title));
                }
                else
                {
                    specifications.Add(new MatchAll<Project>());
                }

                var areaSpecifications = IsAreaSpecifications(searchModel);
                specifications.Add(new OrSpecification<Project>(areaSpecifications.ToArray()));

                var isClinicalNeedTagsSpecifications = IsClinicalNeedTagsSpecifications(searchModel);
                specifications.Add(new OrSpecification<Project>(isClinicalNeedTagsSpecifications.ToArray()));

                var isPotentialTechnologyTagsSpecifications = IsPotentialTechnologyTagsSpecifications(searchModel);
                specifications.Add(new OrSpecification<Project>(isPotentialTechnologyTagsSpecifications.ToArray()));

                if (searchModel.ByStatus == ByStatusFilteringMethod.Draft)
                {
                    specifications.Add(new IsDraftSpec());
                }
                else if (searchModel.ByStatus == ByStatusFilteringMethod.NotDraft)
                {
                    specifications.Add(new NotSpecification<Project>(new IsDraftSpec()));
                }
                else if (searchModel.ByStatus == ByStatusFilteringMethod.Public)
                {
                    specifications.Add(new IsAgreedToTermsOfUboraSpec());
                }

                return new AndSpecification<Project>(specifications.ToArray());
            }

            private List<Specification<Project>> IsAreaSpecifications(SearchModel searchModel)
            {
                var projectSpecifications = new List<Specification<Project>>();
                foreach (var areaId in searchModel.ByArea)
                {
                    var area = Tags.Areas[areaId];
                    if (!string.IsNullOrEmpty(area))
                    {
                        projectSpecifications.Add(new IsAreaSpec(area));
                    }
                }

                return projectSpecifications;
            }

            private List<Specification<Project>> IsClinicalNeedTagsSpecifications(SearchModel searchModel)
            {
                var projectSpecifications = new List<Specification<Project>>();
                foreach (var clinicalNeedTagId in searchModel.ByClinicalNeedTags)
                {
                    var clinical = Tags.ClinicalNeeds[clinicalNeedTagId];
                    if (!string.IsNullOrEmpty(clinical))
                    {
                        projectSpecifications.Add(new IsClinicalNeedTagsSpec(clinical));
                    }
                }

                return projectSpecifications;
            }

            private List<Specification<Project>> IsPotentialTechnologyTagsSpecifications(SearchModel searchModel)
            {
                var projectSpecifications = new List<Specification<Project>>();
                foreach (var potentialTechnologyTagId in searchModel.ByPotentialTechnologyTags)
                {
                    var potentialTechnology = Tags.PotentialTechnologies[potentialTechnologyTagId];
                    if (!string.IsNullOrEmpty(potentialTechnology))
                    {
                        projectSpecifications.Add(
                            new IsPotentialTechnologyTagsSpec(potentialTechnology));
                    }
                }

                return projectSpecifications;
            }  
        }
    }
}