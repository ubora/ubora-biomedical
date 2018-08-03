using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.IsoStandardsCompliances;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Queries;
using Ubora.Domain.Questionnaires.DeviceClassifications.Queries;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.Projects.ApplicableRegulations;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;
using Project = Ubora.Domain.Projects.Project;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class PreproductionDocumentTemplateViewModel
    {
        public string Title { get; set; }
        public string ProjectDescription { get; set; }
        public string DeviceClassification { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string Gmdn { get; set; }
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

            public virtual async Task<PreproductionDocumentTemplateViewModel> Create(Project project, List<WorkpackageCheckBoxListItem> workpackageCheckListItems)
            {
                var model = new PreproductionDocumentTemplateViewModel();
                model.Title = project.Title;
                model.ProjectDescription = project.Description;
                model.AreaOfUsageTags = project.AreaOfUsageTags;
                model.ClinicalNeedTags = project.ClinicalNeedTags;
                model.PotentialTechnologyTags = project.PotentialTechnologyTags;
                model.Gmdn = project.Gmdn;
                model.Members = GetMembers(project);
                if (project.HasImage)
                {
                    model.ImagePath = _storageProvider.GetUrl(project.ProjectImageBlobLocation, ImageSize.Thumbnail400x300); 
                }

                var isCheckedWp1 = workpackageCheckListItems[0].IsChecked;
                if (isCheckedWp1)
                {
                    var workspackageOne = _queryProcessor.FindById<WorkpackageOne>(project.Id);
                    var wp1TemplatePartialViewModel = await GetWp1TemplatePartialViewModel(workspackageOne);
                    wp1TemplatePartialViewModel.ReviewQuestionnaireViewModels = GetReviewQuestionnaireViewModels(project);
                    
                    model.Wp1TemplatePartialViewModel = wp1TemplatePartialViewModel;
                }

                var isCheckedWp2 = workpackageCheckListItems[1].IsChecked;
                if (isCheckedWp2)
                {
                    var workspackageTwo = _queryProcessor.FindById<WorkpackageTwo>(project.Id);
     
                    var deviceStructuredInformation = _queryProcessor
                        .Find(new IsFromWhichWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Two) && new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = project.Id })
                        .FirstOrDefault();

                    var wp2TemplatePartialViewModel = await GetWp2TemplatePartialViewModel(workspackageTwo);
                    wp2TemplatePartialViewModel.StructuredInformationResultViewModel = _structuredInformationResultViewModel.Create(deviceStructuredInformation);
                    
                    model.Wp2TemplatePartialViewModel = wp2TemplatePartialViewModel;
                }
                
                var isCheckedWp3 = workpackageCheckListItems[2].IsChecked;
                if (isCheckedWp3)
                {
                    var workspackageThree = _queryProcessor.FindById<WorkpackageThree>(project.Id);
                    model.Wp3TemplatePartialViewModel = await GetWp3TemplatePartialViewModel(workspackageThree);  
                }

                var isCheckedWp4 = workpackageCheckListItems[3].IsChecked;
                if (isCheckedWp4)
                {
                    var workspackageFour = _queryProcessor.FindById<WorkpackageFour>(project.Id);
                    var deviceStructuredInformation = _queryProcessor
                        .Find(new IsFromWhichWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Four)&& new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = project.Id })
                        .FirstOrDefault();
                    var isoStandardsComplianceAggregate = _queryProcessor.FindById<IsoStandardsComplianceAggregate>(project.Id);

                    var wp4TemplatePartialViewModel = await GetWp4TemplatePartialViewModel(workspackageFour);
                    wp4TemplatePartialViewModel.StructuredInformationResultViewModel =
                        _structuredInformationResultViewModel.Create(deviceStructuredInformation);

                    wp4TemplatePartialViewModel.IsoStandardIndexListViewModel =
                        _indexViewModelFactory.Create(isoStandardsComplianceAggregate);
                    
                    model.Wp4TemplatePartialViewModel = wp4TemplatePartialViewModel;
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
                var userIds = projectMemberGroups.Select(m => m.Key);
                var projectMemberUserProfiles = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery {UserIds = userIds});
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

            private async Task<WP1TemplatePartialViewModel> GetWp1TemplatePartialViewModel(WorkpackageOne workpackageOne)
            {
                var clinicalNeeds = workpackageOne.GetSingleStep("ClinicalNeeds");
                var existingSolutions = workpackageOne.GetSingleStep("ExistingSolutions");
                var intendedUsers = workpackageOne.GetSingleStep("IntendedUsers");
                var productRequirements = workpackageOne.GetSingleStep("ProductRequirements");
              
                return new WP1TemplatePartialViewModel
                {
                    ClinicalNeeds = await _markdownConverter.GetHtmlAsync(clinicalNeeds.Content ?? ""),
                    ExistingSolutions = await _markdownConverter.GetHtmlAsync(existingSolutions.Content ?? ""),
                    IntendedUsers = await _markdownConverter.GetHtmlAsync(intendedUsers.Content ?? ""),
                    ProductRequirements = await _markdownConverter.GetHtmlAsync(productRequirements.Content ?? ""),
                };
            }

            private async Task<WP2TemplatePartialViewModel> GetWp2TemplatePartialViewModel(
                WorkpackageTwo workspackageTwo)
            {
                var physicalPrinciples = workspackageTwo.GetSingleStep("PhysicalPrinciples");
                var conceptDescription = workspackageTwo.GetSingleStep("ConceptDescription");

                return new WP2TemplatePartialViewModel
                {
                    PhysicalPrinciples = await _markdownConverter.GetHtmlAsync(physicalPrinciples.Content ?? ""),
                    ConceptDescription = await _markdownConverter.GetHtmlAsync(conceptDescription.Content ?? "")
                };
            }

            private async Task<WP3TemplatePartialViewModel> GetWp3TemplatePartialViewModel(WorkpackageThree workspackageThree)
            {
                var generalProductDescriptionHardwareCommercialParts =
                    workspackageThree.GetSingleStep("GeneralProductDescription_Hardware_CommercialParts");
                var generalProductDescriptionHardwarePurposelyDesignedParts =
                    workspackageThree.GetSingleStep("GeneralProductDescription_Hardware_PurposelyDesignedParts");
                var generalProductDescriptionHardwarePrototypesAndFunctionalTrials =
                    workspackageThree.GetSingleStep("GeneralProductDescription_Hardware_PrototypesAndFunctionalTrials");
                var generalProductDescriptionElectronicAndFirmwareCommercialParts =
                    workspackageThree.GetSingleStep("GeneralProductDescription_ElectronicAndFirmware_CommercialParts");
                var generalProductDescriptionElectronicAndFirmwarePurposelyDesignedParts =
                    workspackageThree.GetSingleStep("GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts");
                var generalProductDescriptionElectronicAndFirmwarePrototypesAndFunctionalTrials =
                    workspackageThree.GetSingleStep(
                        "GeneralProductDescription_ElectronicAndFirmware_PrototypesAndFunctionalTrials");
                var generalProductDescriptionSoftwareExistingSolutions =
                    workspackageThree.GetSingleStep("GeneralProductDescription_Software_ExistingSolutions");
                var generalProductDescriptionSoftwarePurposelyDesignedParts =
                    workspackageThree.GetSingleStep("GeneralProductDescription_Software_PurposelyDesignedParts");
                var generalProductDescriptionSoftwarePrototypesAndFunctionalTrials =
                    workspackageThree.GetSingleStep("GeneralProductDescription_Software_PrototypesAndFunctionalTrials");
                var generalProductDescriptionSystemIntegrationPrototypesAndFunctionalTrials =
                    workspackageThree.GetSingleStep("GeneralProductDescription_SystemIntegration_PrototypesAndFunctionalTrials");
                var designForIsoTestingCompliance = workspackageThree.GetSingleStep("DesignForIsoTestingCompliance");
                var instructionsForFabricationOfPrototypes =
                    workspackageThree.GetSingleStep("InstructionsForFabricationOfPrototypes");

                return new WP3TemplatePartialViewModel
                {
                    GeneralProductDescriptionHardwareCommercialParts =
                        await _markdownConverter.GetHtmlAsync(generalProductDescriptionHardwareCommercialParts.Content ?? ""),
                    GeneralProductDescriptionHardwarePurposelyDesignedParts =
                        await _markdownConverter.GetHtmlAsync(generalProductDescriptionHardwarePurposelyDesignedParts.Content ??
                                                              ""),
                    GeneralProductDescriptionHardwarePrototypesAndFunctionalTrials =
                        await _markdownConverter.GetHtmlAsync(
                            generalProductDescriptionHardwarePrototypesAndFunctionalTrials.Content ?? ""),
                    GeneralProductDescriptionElectronicAndFirmwareCommercialParts =
                        await _markdownConverter.GetHtmlAsync(
                            generalProductDescriptionElectronicAndFirmwareCommercialParts.Content ?? ""),
                    GeneralProductDescriptionElectronicAndFirmwarePurposelyDesignedParts =
                        await _markdownConverter.GetHtmlAsync(generalProductDescriptionElectronicAndFirmwarePurposelyDesignedParts
                                                                  .Content ?? ""),
                    GeneralProductDescriptionElectronicAndFirmwarePrototypesAndFunctionalTrials =
                        await _markdownConverter.GetHtmlAsync(
                            generalProductDescriptionElectronicAndFirmwarePrototypesAndFunctionalTrials.Content ?? ""),
                    GeneralProductDescriptionSoftwareExistingSolutions =
                        await _markdownConverter.GetHtmlAsync(generalProductDescriptionSoftwareExistingSolutions.Content ?? ""),
                    GeneralProductDescriptionSoftwarePurposelyDesignedParts =
                        await _markdownConverter.GetHtmlAsync(generalProductDescriptionSoftwarePurposelyDesignedParts.Content ??
                                                              ""),
                    GeneralProductDescriptionSoftwarePrototypesAndFunctionalTrials =
                        await _markdownConverter.GetHtmlAsync(
                            generalProductDescriptionSoftwarePrototypesAndFunctionalTrials.Content ?? ""),
                    GeneralProductDescriptionSystemIntegrationPrototypesAndFunctionalTrials =
                        await _markdownConverter.GetHtmlAsync(
                            generalProductDescriptionSystemIntegrationPrototypesAndFunctionalTrials.Content ?? ""),
                    DesignForIsoTestingCompliance =
                        await _markdownConverter.GetHtmlAsync(designForIsoTestingCompliance.Content ?? ""),
                    InstructionsForFabricationOfPrototypes =
                        await _markdownConverter.GetHtmlAsync(instructionsForFabricationOfPrototypes.Content ?? "")
                };
            }

            private async Task<WP4TemplatePartialViewModel> GetWp4TemplatePartialViewModel(
                WorkpackageFour workspackageFour)
            {
                var prototypesAndConsiderationsForSafetyAssessment = workspackageFour.GetSingleStep("PrototypesAndConsiderationsForSafetyAssessment");
                var qualityCriteria = workspackageFour.GetSingleStep("QualityCriteria");
                var resultsFromVitroOrVivo = workspackageFour.GetSingleStep("ResultsFromVitroOrVivo");
                
                return new WP4TemplatePartialViewModel
                {
                    PrototypesAndConsiderationsForSafetyAssessment = await _markdownConverter.GetHtmlAsync(prototypesAndConsiderationsForSafetyAssessment.Content ?? ""),
                    QualityCriteria = await _markdownConverter.GetHtmlAsync(qualityCriteria.Content ?? ""),
                    ResultsFromVitroOrVivo = await _markdownConverter.GetHtmlAsync(resultsFromVitroOrVivo.Content ?? "")
                };
            }
        }
    }
}