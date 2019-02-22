using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Queries;
using Ubora.Domain.Questionnaires.DeviceClassifications.Queries;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.Projects.ApplicableRegulations;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects;
using Ubora.Domain;
using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class PreproductionDocumentTemplateViewModel : ITagsAndKeywords
    {
        public string Title { get; set; }
        public string ProjectDescription { get; set; }
        public string DeviceClassification { get; set; }
        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<Member> Members { get; set; }
        public WP1TemplatePartialViewModel Wp1TemplatePartialViewModel { get; set; }
        public WP2TemplatePartialViewModel Wp2TemplatePartialViewModel { get; set; }
        public WP3TemplatePartialViewModel Wp3TemplatePartialViewModel { get; set; }
        public WP4TemplatePartialViewModel Wp4TemplatePartialViewModel { get; set; }

        public class Member
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public bool IsProjectLeader { get; set; }
            public bool IsCurrentUser { get; set; }
            public bool IsProjectMentor { get; set; }
            
            public string Roles
            {
                get
                {
                    var roles = new List<string>();
                    
                    if (IsProjectLeader)
                    {
                        roles.Add("leader");
                    }
                    
                    if (IsProjectMentor)
                    {
                        roles.Add("mentor");
                    }

                    return string.Join(", ", roles);
                }
            }
        }
        
        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IMarkdownConverter _markdownConverter;
            private readonly StructuredInformationResultViewModel.Factory _structuredInformationResultViewModel;
            private readonly IndexViewModel.Factory _indexViewModelFactory;
            private readonly QuestionnaireIndexViewModel.Factory _questionnaireIndexViewModelFactory;
            private readonly ReviewQuestionnaireViewModel.Factory _reviewQuestionnaireViewModelFactory;
            private readonly ImageStorageProvider _storageProvider;
            private readonly IConfiguration _configuration;
     
            public Factory(IQueryProcessor queryProcessor, IMarkdownConverter markdownConverter, 
                StructuredInformationResultViewModel.Factory structuredInformationResultViewModel, 
                IndexViewModel.Factory indexViewModelFactory, 
                QuestionnaireIndexViewModel.Factory questionnaireIndexViewModelFactory, ReviewQuestionnaireViewModel.Factory reviewQuestionnaireViewModelFactory, ImageStorageProvider storageProvider, IConfiguration Configuration)
            {
                _queryProcessor = queryProcessor;
                _markdownConverter = markdownConverter;
                _structuredInformationResultViewModel = structuredInformationResultViewModel;
                _indexViewModelFactory = indexViewModelFactory;
                _questionnaireIndexViewModelFactory = questionnaireIndexViewModelFactory;
                _reviewQuestionnaireViewModelFactory = reviewQuestionnaireViewModelFactory;
                _storageProvider = storageProvider;
                _configuration = Configuration;
            }
            
            protected Factory()
            {
            }

            public virtual async Task<PreproductionDocumentTemplateViewModel> Create(Project project)
            {
                var model = new PreproductionDocumentTemplateViewModel
                {
                    Title = project.Title,
                    ProjectDescription = project.Description,
                    AreaOfUsageTag = project.AreaOfUsageTag,
                    ClinicalNeedTag = project.ClinicalNeedTag,
                    PotentialTechnologyTag = project.PotentialTechnologyTag,
                    Keywords = project.Keywords,
                    Members = GetMembers(project)
                };

                if (project.HasImage)
                {
                    model.ImagePath = _storageProvider.GetUrl(project.ProjectImageBlobLocation, ImageSize.Thumbnail400x300); 
                }

                var wpStatuses = _queryProcessor.ExecuteQuery(new GetStatusesOfProjectWorkpackagesQuery(project.Id));
                var isWp1Exportable = wpStatuses.Wp1Status == WorkpackageStatus.Opened || wpStatuses.Wp1Status == WorkpackageStatus.Accepted;
                var isWp2Exportable = wpStatuses.Wp2Status == WorkpackageStatus.Opened || wpStatuses.Wp2Status == WorkpackageStatus.Accepted;
                var isWp3Exportable = wpStatuses.Wp3Status == WorkpackageStatus.Opened || wpStatuses.Wp3Status == WorkpackageStatus.Accepted;
                var isWp4Exportable = wpStatuses.Wp4Status == WorkpackageStatus.Opened || wpStatuses.Wp4Status == WorkpackageStatus.Accepted;

                if (isWp1Exportable)
                {
                    var workpackageOne = _queryProcessor.FindById<WorkpackageOne>(project.Id);

                    model.Wp1TemplatePartialViewModel = new WP1TemplatePartialViewModel
                    {
                        WorkpackageStepViewModels = await GetWorkpackageStepViewModels(workpackageOne),
                        ReviewQuestionnaireViewModels = GetReviewQuestionnaireViewModels(project)
                    };
                }

                if (isWp2Exportable)
                {
                    var workspackageTwo = _queryProcessor.FindById<WorkpackageTwo>(project.Id);
     
                    var deviceStructuredInformation = _queryProcessor
                        .Find(new DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Two) && new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = project.Id })
                        .FirstOrDefault();

                    model.Wp2TemplatePartialViewModel = new WP2TemplatePartialViewModel
                    {
                        WorkpackageStepViewModels = await GetWorkpackageStepViewModels(workspackageTwo),
                        StructuredInformationResultViewModel = _structuredInformationResultViewModel.Create(deviceStructuredInformation)
                    };
                }
                
                if (isWp3Exportable)
                {
                    var workspackageThree = _queryProcessor.FindById<WorkpackageThree>(project.Id);
                    model.Wp3TemplatePartialViewModel = new WP3TemplatePartialViewModel
                    {
                        WorkpackageStepViewModels = await GetWorkpackageStepViewModels(workspackageThree)
                    };
                }

                if (isWp4Exportable)
                {
                    var workspackageFour = _queryProcessor.FindById<WorkpackageFour>(project.Id);
                    var deviceStructuredInformation = _queryProcessor
                        .Find(new DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Four)&& new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = project.Id })
                        .FirstOrDefault();
                    var isoStandardsComplianceAggregate = _queryProcessor.FindById<IsoStandardsComplianceChecklist>(project.Id);
                    
                    model.Wp4TemplatePartialViewModel = new WP4TemplatePartialViewModel
                    {
                        WorkpackageStepViewModels = await GetWorkpackageStepViewModels(workspackageFour),
                        StructuredInformationResultViewModel = _structuredInformationResultViewModel.Create(deviceStructuredInformation),
                        IsoStandardIndexListViewModel = _indexViewModelFactory.Create(isoStandardsComplianceAggregate)
                    };
                }

                var deviceClassificationAggregate = _queryProcessor.ExecuteQuery(new LatestFinishedProjectDeviceClassificationQuery(project.Id));
                if (deviceClassificationAggregate != null)
                {
                    model.DeviceClassification = deviceClassificationAggregate.QuestionnaireTree.GetHighestRiskDeviceClass().Name;
                }

                return model;
            }

            private IEnumerable<ReviewQuestionnaireViewModel> GetReviewQuestionnaireViewModels(Project project)
            {
                var questionnaireIndexViewModel = _questionnaireIndexViewModelFactory.Create(project.Id);

                List<Guid> questionnaireIds = new List<Guid>();
                foreach (var questionnaire in questionnaireIndexViewModel.Previous)
                {
                    questionnaireIds.Add(questionnaire.QuestionnaireId);
                }

                var applicableRegulationsQuestionnaireAggregates = _queryProcessor.ExecuteQuery(
                    new FindApplicableRegulationsQuestionnaireAggregatesByIdsQuery {QuestionnaireIds = questionnaireIds.ToArray()});
                var reviewQuestionnaireViewModels =
                    applicableRegulationsQuestionnaireAggregates.Select(q =>
                        _reviewQuestionnaireViewModelFactory.Create(q.Questionnaire));
                return reviewQuestionnaireViewModels;
            }

            private List<Member> GetMembers(Project project)
            {
                var members = new List<Member>();
                var projectMemberGroups = project.Members.GroupBy(m => m.UserId);
                var projectMemberUserProfiles = _queryProcessor.ExecuteQuery(new GetProjectUserProfiles { ProjectId = project.Id});
                foreach (var userProfile in projectMemberUserProfiles)
                {
                    var projectMemberGroup = projectMemberGroups.FirstOrDefault(g => g.Key == userProfile.UserId);

                    var member = new Member
                    {
                        UserId = userProfile.UserId,
                        IsProjectLeader = projectMemberGroup.Any(x => x.IsLeader),
                        IsProjectMentor = projectMemberGroup.Any(x => x.IsMentor),
                        FullName = userProfile.FullName
                    };
                    members.Add(member);
                }

                return members;
            }

            private async Task<List<WorkpackageStepViewModel>> GetWorkpackageStepViewModels<T>(Workpackage<T> workpackage) where T : Workpackage<T>
            {
                var workpackageStepViewModels = new List<WorkpackageStepViewModel>();
                
                foreach (var step in workpackage.Steps)
                {
                    var workpackageStepView = new WorkpackageStepViewModel {Title = step.Title, Content = await _markdownConverter.GetHtmlAsync(step.Content ?? "")};
                    
                    workpackageStepViewModels.Add(workpackageStepView);
                }

                return workpackageStepViewModels;
            }
        }
    }
}