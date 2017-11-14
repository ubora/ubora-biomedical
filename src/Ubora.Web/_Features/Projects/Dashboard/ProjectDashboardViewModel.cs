using System.Security.Principal;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Queries;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Services;
using Project = Ubora.Domain.Projects.Project;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class ProjectDashboardViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DeviceClassification { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string Gmdn { get; set; }
        public bool IsProjectMember { get; set; }
        public bool IsInDraft { get; set; }
        public string ImagePath { get; set; }
        public bool HasImage { get; set; }

        public class Factory
        {
            private readonly IMapper _autoMapper;
            private readonly ImageStorageProvider _storageProvider;
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IMapper autoMapper, ImageStorageProvider storageProvider, IQueryProcessor queryProcessor)
            {
                _autoMapper = autoMapper;
                _storageProvider = storageProvider;
                _queryProcessor = queryProcessor;
            }

            public ProjectDashboardViewModel Create(Project project, IPrincipal user)
            {
                var model = _autoMapper.Map<ProjectDashboardViewModel>(project);

                if (user.Identity.IsAuthenticated)
                {
                    var userId = new HasMember(user.GetId());
                    model.IsProjectMember = project.DoesSatisfy(userId);
                }

                var deviceClassificationAggregate = _queryProcessor.ExecuteQuery(new LatestFinishedProjectDeviceClassificationQuery(project.Id));
                if (deviceClassificationAggregate != null)
                {
                    model.DeviceClassification = deviceClassificationAggregate.QuestionnaireTree.GetHeaviestDeviceClass()?.Name;
                }

                if (project.HasImage)
                {
                    model.ImagePath = _storageProvider.GetUrl(project.ProjectImageBlobLocation, ImageSize.Thumbnail400x300);
                }

                return model;
            }
        }
    }
}