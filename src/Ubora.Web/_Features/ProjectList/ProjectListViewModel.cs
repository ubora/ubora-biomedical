using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListViewModel
    {
        protected ProjectListViewModel()
        {
        }

        public string Header { get; protected set; }
        public IEnumerable<ProjectListItem> Projects { get; protected set; }
        public bool ShowDefaultMessage { get; protected set; }
        public bool ShowProjectsNotFoundMessage { get; protected set; }

        public class ProjectListItem
        {
            public Guid Id { get; protected set; }
            public string Title { get; protected set; }
            public bool IsInDraft { get; set; }
            public string ImagePath { get; set; }
        }

        public class Factory
        {
            private readonly IMapper _mapper;
            private readonly IQueryProcessor _queryProcessor;
            private readonly ImageStorageProvider _imageStorage;

            public Factory(
                IQueryProcessor queryProcessor,
                IMapper mapper,
                ImageStorageProvider imageStorage)
            {
                _queryProcessor = queryProcessor;
                _mapper = mapper;
                _imageStorage = imageStorage;
            }

            public ProjectListViewModel Create(string header)
            {
                var projects = _queryProcessor.Find<Project>(new MatchAll<Project>())
                    .OrderBy(p => p.Title);

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Projects = projects.Select(GetProjectListItem)
                };

                return model;
            }

            public ProjectListViewModel Create(string header, Guid userId)
            {
                var userProjects = _queryProcessor.Find<Project>(new HasMember(userId));

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Projects = userProjects.Select(GetProjectListItem),
                    ShowDefaultMessage = true
                };

                return model;
            }

            public ProjectListViewModel CreateForSearch(string title)
            {
                if (string.IsNullOrEmpty(title))
                {
                    return Create(header: "All projects");
                }

                var projects = _queryProcessor.Find(new BySearchPhrase(title));

                var model = new ProjectListViewModel();
                if (!projects.Any())
                {
                    model.ShowProjectsNotFoundMessage = true;
                }

                model.Projects = projects.Select(GetProjectListItem);

                return model;
            }

            private ProjectListItem GetProjectListItem(Project project)
            {
                var projectListItem = _mapper.Map<ProjectListItem>(project);

                if (project.HasImage)
                {
                    projectListItem.ImagePath = _imageStorage.GetUrl(project.ProjectImageBlobLocation, ImageSize.Thumbnail400x300);
                }

                return projectListItem;
            }
        }
    }
}