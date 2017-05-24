using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.One
{
    public class WorkpackageOneOverviewViewModel
    {
        public string Title { get; set; }
        public IEnumerable<WorkpackageOneStepViewModel> Steps { get; set; }

        public class Factory
        {
            public WorkpackageOneOverviewViewModel Create()
            {
                return new WorkpackageOneOverviewViewModel
                {
                    Title = "Workpackage 1: Design and prototyping",
                    Steps = new[]
                    {
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Description Of Need",
                            ActionName = nameof(WorkpackageOneStepsController.DescriptionOfNeed)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Description Of Existing Solutions And Analysis",
                            ActionName = nameof(WorkpackageOneStepsController.DescriptionOfExistingSolutionsAndAnalysis)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Product Functionality",
                            ActionName = nameof(WorkpackageOneStepsController.ProductFunctionality)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Product Performance",
                            ActionName = nameof(WorkpackageOneStepsController.ProductPerformance)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Product Usability",
                            ActionName = nameof(WorkpackageOneStepsController.ProductUsability)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Product Safety",
                            ActionName = nameof(WorkpackageOneStepsController.ProductSafety)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Patient Population Study",
                            ActionName = nameof(WorkpackageOneStepsController.PatientPopulationStudy)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "User Requirement Study",
                            ActionName = nameof(WorkpackageOneStepsController.UserRequirementStudy)
                        },
                        new WorkpackageOneStepViewModel
                        {
                            Title = "Additional Information",
                            ActionName = nameof(WorkpackageOneStepsController.AdditionalInformation)
                        },
                    }
                };
            }
        }
    }
}