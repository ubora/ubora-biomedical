using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class SideMenuViewModel
    {
        public IEnumerable<ISideMenuItem> TopLevelMenuItems { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IUrlHelper _urlHelper;
            private readonly IAuthorizationService _authorizationService;

            public Factory(IQueryProcessor queryProcessor, IUrlHelper urlHelper, IAuthorizationService authorizationService)
            {
                _queryProcessor = queryProcessor;
                _urlHelper = urlHelper;
                _authorizationService = authorizationService;
            }

            public SideMenuViewModel Create(Guid projectId, string selectedId, ClaimsPrincipal user)
            {
                var wpStatuses = _queryProcessor.ExecuteQuery(new GetStatusesOfProjectWorkpackagesQuery(projectId));

                var items = new ISideMenuItem[]
                {
                    /*wp0*/ new WpSideMenuHyperlinkMenuItem(NestingLevel.None, "DesignPlanning", "Design planning", _urlHelper.Action("ProjectOverview", "WorkpackageOne"))
                        .SetStatus(WorkpackageStatus.Accepted),
                    CreateWp1(wpStatuses.Wp1Status),
                    CreateWp2(wpStatuses.Wp2Status),
                    CreateWp3(wpStatuses.Wp3Status),
                    CreateWp4(wpStatuses.Wp4Status),
                    CreateWp5(wpStatuses.Wp5Status),
                    CreateWp6(wpStatuses.Wp6Status)
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
                IWorkpackageSideMenuItem CreateWp1(WorkpackageStatus workpackageStatus)
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
                    }).SetStatus(workpackageStatus);
                }

                IWorkpackageSideMenuItem CreateWp2(WorkpackageStatus workpackageStatus)
                {
                    return new WpSideMenuCollapseMenuItem(NestingLevel.None, "workPackageTwo", "WP 2: Conceptual design", new IWorkpackageSideMenuItem[]
                    {
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "PhysicalPrinciples", "Physical principles", href: Wp2StepLink("PhysicalPrinciples")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "Voting", "Voting", href: _urlHelper.Action("Voting", "Candidates")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "ConceptDescription", "Concept description", href: Wp2StepLink("ConceptDescription")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.Two, "StructuredInformationOnTheDevice", "Structured information on the device", href: _urlHelper.Action("StructuredInformationOnTheDevice", "WorkpackageTwo")),
                    }).SetStatus(workpackageStatus);
                }

                IWorkpackageSideMenuItem CreateWp3(WorkpackageStatus workpackageStatus)
                {
                    if (workpackageStatus == WorkpackageStatus.Unlockable)
                    {
                        if (!_authorizationService.IsAuthorized(user, Policies.CanUnlockWorkpackages))
                        {
                            workpackageStatus = WorkpackageStatus.Closed;
                        }

                        return new WpSideMenuHyperlinkMenuItem(NestingLevel.One, WorkpackageMenuOption.WorkpackageThreeLocked,
                            "WP 3: Design and prototyping", href: _urlHelper.Action(nameof(WorkpackageThreeController.Unlocking), "WorkpackageThree")).SetStatus(workpackageStatus);
                    }

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
                    }).SetStatus(workpackageStatus);
                }
                
                IWorkpackageSideMenuItem CreateWp4(WorkpackageStatus workpackageStatus)
                {
                    if (workpackageStatus == WorkpackageStatus.Unlockable)
                    {
                        if (!_authorizationService.IsAuthorized(user, Policies.CanUnlockWorkpackages))
                        {
                            workpackageStatus = WorkpackageStatus.Closed;
                        }

                        return new WpSideMenuHyperlinkMenuItem(NestingLevel.One, WorkpackageMenuOption.WorkpackageFourLocked,
                            "WP 4: Implementation", href: _urlHelper.Action(nameof(WorkpackageFourController.Unlocking), "WorkpackageFour")).SetStatus(workpackageStatus);
                    }

                    return new WpSideMenuCollapseMenuItem(NestingLevel.None, "workpackageFour", "WP 4: Implementation", new[]
                    {
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "PrototypesAndConsiderationsForSafetyAssessment","Prototypes and considerations for safety assessment", href: Wp4StepLink("PrototypesAndConsiderationsForSafetyAssessment")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "QualityCriteria","Quality criteria", href: Wp4StepLink("QualityCriteria")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "IsoCompliance","ISO compliance", href: _urlHelper.Action(nameof(IsoCompliancesController.Index), nameof(IsoCompliancesController).RemoveSuffix())),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "ResultsFromVitroOrVivo","Results from vitro/vivo", href: Wp4StepLink("ResultsFromVitroOrVivo")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "WP4StructuredInformationOnTheDevice","Structured information on the device", href: _urlHelper.Action("StructuredInformationOnTheDevice", "WorkpackageFour")),
                        //new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "PreproductionDocuments","Preproduction documents", href: "#"),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "protostep","protostep", href: _urlHelper.Action("protostep", "workpackagefour"))
                    }).SetStatus(workpackageStatus);
                }

                IWorkpackageSideMenuItem CreateWp5(WorkpackageStatus workpackageStatus)
                {
                    var wpName = "WP 5: Operation";
                    if (workpackageStatus == WorkpackageStatus.Unlockable)
                    {
                        if (!_authorizationService.IsAuthorized(user, Policies.CanUnlockWorkpackages))
                        {
                            workpackageStatus = WorkpackageStatus.Closed;
                        }

                        return new WpSideMenuHyperlinkMenuItem(NestingLevel.One, WorkpackageMenuOption.WorkpackageFiveLocked,
                            wpName, href: _urlHelper.Action(nameof(WorkpackageFiveController.Unlocking), WorkpackageFiveController.Name)).SetStatus(workpackageStatus);
                    }

                    return new WpSideMenuCollapseMenuItem(NestingLevel.None, "workpackageFive", wpName, new[]
                    {
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, "ProductionDocumentation", "Production documentation", href: Wp5StepLink("ProductionDocumentation")),
                        new WpSideMenuHyperlinkMenuItem(NestingLevel.One, WorkpackageMenuOption.BusinessModelCanvas, "Business model canvas", href: _urlHelper.Action(nameof(WorkpackageFiveController.BusinessModelCanvas), WorkpackageFiveController.Name)),
                    }).SetStatus(workpackageStatus);
                }

                IWorkpackageSideMenuItem CreateWp6(WorkpackageStatus workpackageStatus)
                {
                    var wpName = "WP 6: Project closure";
                    if (workpackageStatus == WorkpackageStatus.Unlockable)
                    {
                        if (!_authorizationService.IsAuthorized(user, Policies.CanUnlockWorkpackages))
                        {
                            workpackageStatus = WorkpackageStatus.Closed;
                        }

                        return new WpSideMenuHyperlinkMenuItem(NestingLevel.One, WorkpackageMenuOption.WorkpackageSixLocked,
                            wpName, href: _urlHelper.Action(nameof(WorkpackageSixController.Unlocking), WorkpackageSixController.Name)).SetStatus(workpackageStatus);
                    }

                    return new WpSideMenuHyperlinkMenuItem(NestingLevel.One, WorkpackageMenuOption.ProjectClosure,
                            wpName, href: _urlHelper.Action(nameof(WorkpackageSixController.Index), WorkpackageSixController.Name)).SetStatus(workpackageStatus);
                }

                string Wp5StepLink(string stepId)
                {
                    return _urlHelper.Action("Read", "WorkpackageFive", new { projectId = projectId, stepId = stepId });
                }

                string Wp4StepLink(string stepId)
                {
                    return _urlHelper.Action("Read", "WorkpackageFour", new { projectId = projectId, stepId = stepId });
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