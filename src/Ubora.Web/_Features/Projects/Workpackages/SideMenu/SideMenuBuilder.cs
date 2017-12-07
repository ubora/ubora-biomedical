using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class SideMenuBuilder : ProjectViewComponent
    {
        public SideMenuBuilder(IQueryProcessor processor) : base(processor)
        {
        }

#pragma warning disable 1998
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore 1998
        {
            return View("~/_Features/Projects/Workpackages/SideMenu/Main.cshtml", Build(ProjectId));
        }

        public SideMenuViewModel Build(Guid projectId)
        {
            var wpStatuses = QueryProcessor.ExecuteQuery(new GetStatusesOfProjectWorkpackagesQuery(ProjectId));

            var items = new ISideMenuItem[]
            {
                new HyperlinkMenuItem(NestingLevel.None, "DesignPlanning", "Design planning", Url.Action("ProjectOverview", "WorkpackageOne")).SetStatus(WorkpackageStatus.Accepted),
                CreateWp1().SetStatus(wpStatuses.Wp1Status),
                CreateWp2().SetStatus(wpStatuses.Wp2Status),
                CreateWp3().SetStatus(wpStatuses.Wp3Status),
                new HyperlinkMenuItem(NestingLevel.None, "workpackageFour", "Implementation", "#", new WorkpackageFourIconProvider()).SetStatus(wpStatuses.Wp4Status),
                new HyperlinkMenuItem(NestingLevel.None, "workpackageFive", "Operation", "#", new WorkpackageFiveIconProvider()).SetStatus(wpStatuses.Wp5Status),
                new HyperlinkMenuItem(NestingLevel.None, "workpackageSix", "Project closure", "#", new WorkpackageSixIconProvider()).SetStatus(wpStatuses.Wp6Status)
            };

            MarkSelected(items);

            return new SideMenuViewModel
            {
                TopLevelMenuItems = items
            };
        }

        private ISideMenuItem CreateWp1()
        {
            return new CollapseMenuItem(NestingLevel.None, "workPackageOne", "Medical need and product specification", new[]
            {
                new HyperlinkMenuItem(NestingLevel.One, "ClinicalNeeds","Clinical needs", Wp1StepLink("ClinicalNeeds")),
                new HyperlinkMenuItem(NestingLevel.One, "ExistingSolutions","Existing solutions", Wp1StepLink("ExistingSolutions")),
                new HyperlinkMenuItem(NestingLevel.One, "IntendedUsers","Intended users", Wp1StepLink("IntendedUsers")),
                new HyperlinkMenuItem(NestingLevel.One, "ProductRequirements","Product requirements", Wp1StepLink("ProductRequirements")),
                new HyperlinkMenuItem(NestingLevel.One, "DeviceClassification","Device classification", Url.Action("Index", "DeviceClassifications")),
                new HyperlinkMenuItem(NestingLevel.One, "RegulationChecklist","Regulation checklist", Url.Action("Index", "ApplicableRegulations")),
                new HyperlinkMenuItem(NestingLevel.One, "WorkpackageOneReview","Formal review", Url.Action("Review", "WorkpackageOneReview"))
            }, new WorkpackageOneIconProvider());
        }

        private CollapseMenuItem CreateWp2()
        {
            return new CollapseMenuItem(NestingLevel.None, "workPackageTwo", "Conceptual design", new ISideMenuItem[]
            {
                new HyperlinkMenuItem(NestingLevel.Two, "PhysicalPrinciples", "Physical principles", href: Wp2StepLink("PhysicalPrinciples")),
                new HyperlinkMenuItem(NestingLevel.Two, "Voting", "Voting", href: Url.Action("Voting", "Candidates")),
                new HyperlinkMenuItem(NestingLevel.Two, "ConceptDescription", "Concept description", href: Wp2StepLink("ConceptDescription")),
                new HyperlinkMenuItem(NestingLevel.Two, "StructuredInformationOnTheDevice", "Structured information on the device", href: Wp2StepLink("StructuredInformationOnTheDevice")),
            }, new WorkpackageTwoIconProvider());
        }

        private ISideMenuItem CreateWp3()
        {
            return new CollapseMenuItem(NestingLevel.None, "workpackageThree", "Design and prototyping", new ISideMenuItem[]
            {
                new CollapseMenuItem(NestingLevel.One, "general-product-description", "General product description", new ISideMenuItem[]
                {
                    new CollapseMenuItem(NestingLevel.Two, "general-product-description-for-hardware", "Hardware", new ISideMenuItem[]
                    {
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Hardware_CommercialParts", "Commercial parts", href: Wp3StepLink("GeneralProductDescription_Hardware_CommercialParts")),
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Hardware_PurposelyDesignedParts", "Purposely designed parts", href: Wp3StepLink("GeneralProductDescription_Hardware_PurposelyDesignedParts")),
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Hardware_PrototypesAndFunctionalTrials", "Prototypes and functional trials", href: Wp3StepLink("GeneralProductDescription_Hardware_PrototypesAndFunctionalTrials")),
                    }),
                    new CollapseMenuItem(NestingLevel.Two, "general-product-description-for-electronic-and-firmware", "Electronic & firmware", new ISideMenuItem[]
                    {
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_ElectronicAndFirmware_CommercialParts", "Commercial parts", href: Wp3StepLink("GeneralProductDescription_ElectronicAndFirmware_CommercialParts")),
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts", "Purposely designed parts", href: Wp3StepLink("GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts")),
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_ElectronicAndFirmware_PrototypesAndFunctionalTrials", "Prototypes and functional trials", href: Wp3StepLink("GeneralProductDescription_ElectronicAndFirmware_PrototypesAndFunctionalTrials")),
                    }),
                    new CollapseMenuItem(NestingLevel.Two, "general-product-description-for-software", "Software", new ISideMenuItem[]
                    {
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Software_ExistingSolutions", "Existing solutions (open source)", href: Wp3StepLink("GeneralProductDescription_Software_ExistingSolutions")),
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Software_PurposelyDesignedParts", "Purposely designed parts", href: Wp3StepLink("GeneralProductDescription_Software_PurposelyDesignedParts")),
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_Software_PrototypesAndFunctionalTrials", "Prototypes and functional trials", href: Wp3StepLink("GeneralProductDescription_Software_PrototypesAndFunctionalTrials")),
                    }),
                    new CollapseMenuItem(NestingLevel.Two, "general-product-description-for-system-integration", "System integration", new ISideMenuItem[]
                    {
                        new HyperlinkMenuItem(NestingLevel.Three, "GeneralProductDescription_SystemIntegration_PrototypesAndFunctionalTrials", "Instructions for fabrication of prototypes", href: Wp3StepLink("GeneralProductDescription_SystemIntegration_PrototypesAndFunctionalTrials")),
                    })
                }),
                new HyperlinkMenuItem(NestingLevel.One, "DesignForIsoTestingCompliance", "Design for ISO testing compliance", href: Wp3StepLink("DesignForIsoTestingCompliance")),
                new HyperlinkMenuItem(NestingLevel.One, "InstructionsForFabricationOfPrototypes", "Instructions for fabrication of prototypes", href: Wp3StepLink("InstructionsForFabricationOfPrototypes"))
            }, new WorkpackageThreeIconProvider());
        }

        void MarkSelected(ISideMenuItem[] items)
        {
            var selected = ViewData["WorkpackageMenuOption"] as string;

            foreach (var item in items)
            {
                var asCollapse = item as CollapseMenuItem;
                if (asCollapse != null)
                {
                    MarkSelected(asCollapse.InnerMenuItems);
                }
                else
                {
                    if (item.Id == selected)
                    {
                        ((HyperlinkMenuItem)item).IsSelected = true;
                        return;
                    }
                }
            }
        }

        string Wp3StepLink(string stepId)
        {
            return Url.Action("Read", "WorkpackageThree", new { projectId = ProjectId, stepId = stepId });
        }

        string Wp2StepLink(string stepId)
        {
            return Url.Action("Read", "WorkpackageTwo", new { projectId = ProjectId, stepId = stepId });
        }

        string Wp1StepLink(string stepId)
        {
            return Url.Action("Read", "WorkpackageOne", new { projectId = ProjectId, stepId = stepId });
        }
    }
}