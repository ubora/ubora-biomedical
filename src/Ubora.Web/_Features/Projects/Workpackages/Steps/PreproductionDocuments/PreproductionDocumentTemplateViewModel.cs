using System.Collections.Generic;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Questionnaires.DeviceClassifications.Queries;
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
        public WP1TemplatePartialViewModel Wp1TemplatePartialViewModel { get; set; }
        public WP3TemplatePartialViewModel Wp3TemplatePartialViewModel { get; set; }
        
        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IMarkdownConverter _markdownConverter;
     
            public Factory(IQueryProcessor queryProcessor, IMarkdownConverter markdownConverter)
            {
                _queryProcessor = queryProcessor;
                _markdownConverter = markdownConverter;
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

                var isCheckedWp1 = workpackageCheckListItems[0].IsChecked;
                if (isCheckedWp1)
                {
                    var workspackageOne = _queryProcessor.FindById<WorkpackageOne>(project.Id);
                    model.Wp1TemplatePartialViewModel = await GetWp1TemplatePartialViewModel(workspackageOne);
                }

                var isCheckedWp3 = workpackageCheckListItems[2].IsChecked;
                if (isCheckedWp3)
                {
                     var workspackageThree = _queryProcessor.FindById<WorkpackageThree>(project.Id);
                     model.Wp3TemplatePartialViewModel = await GetWp3TemplatePartialViewModel(workspackageThree);
                }

                var deviceClassificationAggregate = _queryProcessor.ExecuteQuery(new LatestFinishedProjectDeviceClassificationQuery(project.Id));
                if (deviceClassificationAggregate != null)
                {
                    model.DeviceClassification = deviceClassificationAggregate.QuestionnaireTree.GetHighestRiskDeviceClass().Name;
                }

                return model;
            }

            private async Task<WP1TemplatePartialViewModel> GetWp1TemplatePartialViewModel(WorkpackageOne workspackageOne)
            {
                var clinicalNeeds = workspackageOne.GetSingleStep("ClinicalNeeds");
                var existingSolutions = workspackageOne.GetSingleStep("ExistingSolutions");
                var intendedUsers = workspackageOne.GetSingleStep("IntendedUsers");
                var productRequirements = workspackageOne.GetSingleStep("ProductRequirements");

                return new WP1TemplatePartialViewModel
                {
                    ClinicalNeeds = await _markdownConverter.GetHtmlAsync(clinicalNeeds.Content),
                    ExistingSolutions = await _markdownConverter.GetHtmlAsync(existingSolutions.Content),
                    IntendedUsers = await _markdownConverter.GetHtmlAsync(intendedUsers.Content),
                    ProductRequirements = await _markdownConverter.GetHtmlAsync(productRequirements.Content)
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
        }
    }
}