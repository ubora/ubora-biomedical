using System;
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
        public string ClinicalNeedTags { get; set; }
        public string AreaTags { get; set; }
        public string TechnologyTags { get; set; }

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
                    ShowCardShadow = showCardShadow,
                    AreaTags = project.AreaOfUsageTags,
                    ClinicalNeedTags = project.ClinicalNeedTags,
                    TechnologyTags = project.PotentialTechnologyTags
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