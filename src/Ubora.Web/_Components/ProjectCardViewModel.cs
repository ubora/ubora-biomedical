using System;
using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web._Components
{
    public class ProjectCardViewModel
    {
        public Guid ProjectId { get; protected set; }
        public string ProjectTitle { get; protected set; }
        public bool IsDraftProject { get; protected set; }
        public string ProjectImageUrl { get; protected set; }
        public bool ShowCardShadow { get; protected set; }

        public class Factory
        {
            private readonly ImageStorageProvider _imageStorageProvider;

            public Factory(ImageStorageProvider imageStorageProvider)
            {
                _imageStorageProvider = imageStorageProvider;
            }

            public ProjectCardViewModel Create(Project project, bool showCardShadow = true)
            {
                var projectCardViewModel = new ProjectCardViewModel
                {
                    ProjectId = project.Id,
                    ProjectTitle = project.Title,
                    IsDraftProject = project.IsInDraft,
                    ShowCardShadow = showCardShadow
                };

                if (project.HasImage)
                {
                    projectCardViewModel.ProjectImageUrl = _imageStorageProvider.GetUrl(project.ProjectImageBlobLocation, ImageSize.Thumbnail400x300);
                }

                return projectCardViewModel;
            }
        }
    }
}