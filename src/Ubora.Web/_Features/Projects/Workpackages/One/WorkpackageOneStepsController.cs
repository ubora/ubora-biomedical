using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Web._Features.Projects.Workpackages.One
{
    public class WorkpackageOneStepsController : ProjectController
    {
        public WorkpackageOne WorkpackageOne => QueryProcessor.FindById<WorkpackageOne>(ProjectId);

        public WorkpackageOneStepsController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        // Shared get/post actions are used because the views/commands are so similar.
        private IActionResult SharedStepView(object model)
        {
            return View("Step", model);
        }

        private IActionResult SharedEditStep<T>(EditWorkpackageOneStepViewModel model, string getActionName) where T : EditWorkpackageOneCommand, new()
        {
            if (!ModelState.IsValid)
            {
                return SharedStepView(model);
            }

            this.ExecuteUserProjectCommand(new T
            {
                NewValue = model.Value
            });

            if (!ModelState.IsValid)
            {
                return SharedStepView(model);
            }

            return RedirectToAction(getActionName);
        }

        public IActionResult DescriptionOfNeed(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Description of need",
                Value = WorkpackageOne.DescriptionOfNeed,
                ActionName = nameof(DescriptionOfNeed),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfNeed(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditDescriptionOfNeedCommand>(model, nameof(DescriptionOfNeed));
        }

        public IActionResult DescriptionOfExistingSolutionsAndAnalysis(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Description of existing solutions and analysis",
                Value = WorkpackageOne.DescriptionOfExistingSolutionsAndAnalysis,
                ActionName = nameof(DescriptionOfExistingSolutionsAndAnalysis),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfExistingSolutionsAndAnalysis(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditDescriptionOfExistingSolutionsAndAnalysisCommand>(model, nameof(DescriptionOfExistingSolutionsAndAnalysis));
        }

        public IActionResult ProductFunctionality(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Product functionality",
                Value = WorkpackageOne.ProductFunctionality,
                ActionName = nameof(ProductFunctionality),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult ProductFunctionality(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditProductFunctionalityCommand>(model, nameof(ProductFunctionality));
        }

        public IActionResult ProductPerformance(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Product performance",
                Value = WorkpackageOne.ProductPerformance,
                ActionName = nameof(ProductPerformance),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult ProductPerformance(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditProductPerformanceCommand>(model, nameof(ProductPerformance));
        }

        public IActionResult ProductUsability(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Product usability",
                Value = WorkpackageOne.ProductUsability,
                ActionName = nameof(ProductUsability),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult ProductUsability(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditProductUsabilityCommand>(model, nameof(ProductUsability));
        }

        public IActionResult ProductSafety(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Product safety",
                Value = WorkpackageOne.ProductSafety,
                ActionName = nameof(ProductSafety),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult ProductSafety(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditProductSafetyCommand>(model, nameof(ProductSafety));
        }

        public IActionResult PatientPopulationStudy(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Population study",
                Value = WorkpackageOne.PatientPopulationStudy,
                ActionName = nameof(PatientPopulationStudy),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult PatientPopulationStudy(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditPatientPopulationStudyCommand>(model, nameof(PatientPopulationStudy));
        }

        public IActionResult UserRequirementStudy(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "User requirement study",
                Value = WorkpackageOne.UserRequirementStudy,
                ActionName = nameof(UserRequirementStudy),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult UserRequirementStudy(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditUserRequirementStudyCommand>(model, nameof(UserRequirementStudy));
        }

        public IActionResult AdditionalInformation(bool edit = false)
        {
            var model = new EditWorkpackageOneStepViewModel
            {
                Title = "Additional information",
                Value = WorkpackageOne.AdditionalInformation,
                ActionName = nameof(AdditionalInformation),
                IsEdit = edit,
            };
            return SharedStepView(model);
        }

        [HttpPost]
        public IActionResult AdditionalInformation(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditStep<EditAdditionalInformationCommand>(model, nameof(AdditionalInformation));
        }
    }
}
