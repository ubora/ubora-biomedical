using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class SideMenuViewModel
    {
        public IEnumerable<ISideMenuItem> TopLevelMenuItems { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IUrlHelper _urlHelper;

            public Factory(IQueryProcessor queryProcessor, IUrlHelper urlHelper)
            {
                _queryProcessor = queryProcessor;
                _urlHelper = urlHelper;
            }

            public SideMenuViewModel Create(Guid projectId, string selectedId)
            {
                var wpStatuses = _queryProcessor.ExecuteQuery(new GetStatusesOfProjectWorkpackagesQuery(projectId));

                var items = new ISideMenuItem[]
                {
                    /*wp0*/ new WpSideMenuHyperlinkMenuItem(NestingLevel.None, "DesignPlanning", "Design planning", _urlHelper.Action("ProjectOverview", "WorkpackageOne"))
                        .SetStatus(WorkpackageStatus.Accepted),
                    CreateWp1().SetStatus(wpStatuses.Wp1Status),
                    CreateWp2().SetStatus(wpStatuses.Wp2Status),
                    CreateWp3().SetStatus(wpStatuses.Wp3Status),
                    /*wp4*/ new WpSideMenuHyperlinkMenuItem(NestingLevel.None, "workpackageFour", "WP 4: Implementation", "#")
                        .SetStatus(wpStatuses.Wp4Status),
                    /*wp5*/ new WpSideMenuHyperlinkMenuItem(NestingLevel.None, "workpackageFive", "WP 5: Operation", "#")
                        .SetStatus(wpStatuses.Wp5Status),
                    /*wp6*/ new WpSideMenuHyperlinkMenuItem(NestingLevel.None, "workpackageSix", "WP 6: Project closure", "#")
                        .SetStatus(wpStatuses.Wp6Status)
                };

                if (!MarkSelected(items, selectedId))
                {
                    throw new InvalidOperationException($"Selected work package side menu item with ID not found: {selectedId}");
                }

                return new SideMenuViewModel
                {
                    TopLevelMenuItems = items
                };

                // Local functions below for cleanliness (i.e. don't have to pass 'projectId' forward): 
                IWorkpackageSideMenuItem CreateWp1()
                {
                    return new WpSideMenuCollapseMenuItem(NestingLevel.None, "workPackageOne", "WP 1: Medical need and product specification", new[]
                    {
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "ClinicalNeeds","Clinical needs", Wp1StepLink("ClinicalNeeds")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "ExistingSolutions","Existing solutions", Wp1StepLink("ExistingSolutions")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "IntendedUsers","Intended users", Wp1StepLink("IntendedUsers")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "ProductRequirements","Product requirements", Wp1StepLink("ProductRequirements")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "DeviceClassification","Device classification", _urlHelper.Action("Index", "DeviceClassifications")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "RegulationChecklist","Regulation checklist", _urlHelper.Action("Index", "ApplicableRegulations")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "WorkpackageOneReview","Formal review", _urlHelper.Action("Review", "WorkpackageOneReview"))
                    });
                }

                IWorkpackageSideMenuItem CreateWp2()
                {
                    return new WpSideMenuCollapseMenuItem(NestingLevel.None, "workPackageTwo", "WP 2: Conceptual design", new IWorkpackageSideMenuItem[]
                    {
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "PhysicalPrinciples", "Physical principles", href: Wp2StepLink("PhysicalPrinciples")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "Voting", "Voting", href: _urlHelper.Action("Voting", "Candidates")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "ConceptDescription", "Concept description", href: Wp2StepLink("ConceptDescription")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "StructuredInformationOnTheDevice", "Structured information on the device", href: _urlHelper.Action("StructuredInformationOnTheDevice", "WorkpackageTwo")),
                    });
                }

                IWorkpackageSideMenuItem CreateWp3()
                {
                    return new WpSideMenuCollapseMenuItem(NestingLevel.None, "workpackageThree", "WP 3: Design and prototyping", new IWorkpackageSideMenuItem[]
                    {
                        new WpSideMenuCollapseMenuItem(NestingLevel.One, "general-product-description", "General product description", new IWorkpackageSideMenuItem[]
                        {
                            new WpSideMenuCollapseMenuItem(NestingLevel.Two, "general-product-description-for-hardware", "Hardware", new IWorkpackageSideMenuItem[]
                            {
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Hardware_CommercialParts", "Commercial parts", href: Wp3StepLink("GeneralProductDescription_Hardware_CommercialParts")),
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Hardware_PurposelyDesignedParts", "Purposely designed parts", href: Wp3StepLink("GeneralProductDescription_Hardware_PurposelyDesignedParts")),
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Hardware_PrototypesAndFunctionalTrials", "Prototypes and functional trials", href: Wp3StepLink("GeneralProductDescription_Hardware_PrototypesAndFunctionalTrials")),
                            }),
                            new WpSideMenuCollapseMenuItem(NestingLevel.Two, "general-product-description-for-electronic-and-firmware", "Electronic & firmware", new IWorkpackageSideMenuItem[]
                            {
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_ElectronicAndFirmware_CommercialParts", "Commercial parts", href: Wp3StepLink("GeneralProductDescription_ElectronicAndFirmware_CommercialParts")),
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts", "Purposely designed parts", href: Wp3StepLink("GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts")),
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_ElectronicAndFirmware_PrototypesAndFunctionalTrials", "Prototypes and functional trials", href: Wp3StepLink("GeneralProductDescription_ElectronicAndFirmware_PrototypesAndFunctionalTrials")),
                            }),
                            new WpSideMenuCollapseMenuItem(NestingLevel.Two, "general-product-description-for-software", "Software", new IWorkpackageSideMenuItem[]
                            {
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Software_ExistingSolutions", "Existing solutions (open source)", href: Wp3StepLink("GeneralProductDescription_Software_ExistingSolutions")),
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Software_PurposelyDesignedParts", "Purposely designed parts", href: Wp3StepLink("GeneralProductDescription_Software_PurposelyDesignedParts")),
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Software_PrototypesAndFunctionalTrials", "Prototypes and functional trials", href: Wp3StepLink("GeneralProductDescription_Software_PrototypesAndFunctionalTrials")),
                            }),
                            new WpSideMenuCollapseMenuItem(NestingLevel.Two, "general-product-description-for-system-integration", "System integration", new IWorkpackageSideMenuItem[]
                            {
                                new WpSideMenuHyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_SystemIntegration_PrototypesAndFunctionalTrials", "Prototypes and functional trials", href: Wp3StepLink("GeneralProductDescription_SystemIntegration_PrototypesAndFunctionalTrials")),
                            })
                        }),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "DesignForIsoTestingCompliance", "Design for ISO testing compliance", href: Wp3StepLink("DesignForIsoTestingCompliance")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "InstructionsForFabricationOfPrototypes", "Instructions for fabrication of prototypes", href: Wp3StepLink("InstructionsForFabricationOfPrototypes"))
                    });
                }

                string Wp3StepLink(string stepId)
                {
                    return _urlHelper.Action("Read", "WorkpackageThree", new { projectId = projectId, stepId = stepId });
                }

                string Wp2StepLink(string stepId)
                {
                    return _urlHelper.Action("Read", "WorkpackageTwo", new { projectId = projectId, stepId = stepId });
                }

                string Wp1StepLink(string stepId)
                {
                    return _urlHelper.Action("Read", "WorkpackageOne", new { projectId = projectId, stepId = stepId });
                }
            }

            private bool MarkSelected(IEnumerable<ISideMenuItem> items, string selectedId)
            {
                foreach (var item in items)
                {
                    var asCollapseMenuItem = item as CollapseMenuItem;
                    if (asCollapseMenuItem != null)
                    {
                        var wasSubItemSelected = MarkSelected(asCollapseMenuItem.InnerMenuItems, selectedId);
                        if (wasSubItemSelected)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        var isSelectedHyperlinkMenuItem = string.Equals(item.Id, selectedId, StringComparison.OrdinalIgnoreCase);
                        if (isSelectedHyperlinkMenuItem)
                        {
                            ((HyperlinkMenuItem)item).IsSelected = true;
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}