using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Queries;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;

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
                var projects = _queryProcessor.Find<Project>();

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Projects = projects.Select(GetProjectListItem)
                };

                return model;
            }

            public ProjectListViewModel Create(string header, Guid userId)
            {
                var userProjects = _queryProcessor.Find<Project>()
                    .Where(x => x.Members.Any(m => m.UserId == userId));

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Projects = userProjects.Select(GetProjectListItem),
                    ShowDefaultMessage = true
                };

                return model;
            }

            public ProjectListViewModel CreateByTitle(string title)
            {
                if (String.IsNullOrEmpty(title))
                {
                    return new ProjectListViewModel
                    {
                        Projects = new ProjectListItem[] { }
                    };
                }

                var projects = _queryProcessor.ExecuteQuery(new SearchProjectsQuery(title));

                var model = new ProjectListViewModel
                {
                    Projects = projects.Select(GetProjectListItem)
                };

                return model;
            }

            private ProjectListItem GetProjectListItem(Project project)
            {
                var projectListItem = _mapper.Map<ProjectListItem>(project);

                if (project.HasImage)
                {
                    projectListItem.ImagePath = _imageStorage.GetUrl(project.ProjectImageBlobLocation, ImageSize.Thumbnail400x150);
                }

                return projectListItem;
            }
        }
    }
}