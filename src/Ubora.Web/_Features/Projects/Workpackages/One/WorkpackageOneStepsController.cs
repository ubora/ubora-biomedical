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

        private IActionResult SharedSubjectView(object model)
        {
            return View("Step", model);
        }

        private IActionResult SharedEditSubject<T>(EditWorkpackageOneStepViewModel model, string getActionName) where T : EditWorkpackageOneCommand, new()
        {
            if (!ModelState.IsValid)
            {
                return SharedSubjectView(model);
            }

            this.ExecuteUserProjectCommand(new T
            {
                NewValue = model.Value
            });

            if (!ModelState.IsValid)
            {
                return SharedSubjectView(model);
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfNeed(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditDescriptionOfNeedCommand>(model, nameof(DescriptionOfNeed));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfExistingSolutionsAndAnalysis(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditDescriptionOfExistingSolutionsAndAnalysisCommand>(model, nameof(DescriptionOfExistingSolutionsAndAnalysis));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult ProductFunctionality(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditProductFunctionalityCommand>(model, nameof(ProductFunctionality));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult ProductPerformance(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditProductPerformanceCommand>(model, nameof(ProductPerformance));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult ProductUsability(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditProductUsabilityCommand>(model, nameof(ProductUsability));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult ProductSafety(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditProductSafetyCommand>(model, nameof(ProductSafety));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult PatientPopulationStudy(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditPatientPopulationStudyCommand>(model, nameof(PatientPopulationStudy));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult UserRequirementStudy(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditUserRequirementStudyCommand>(model, nameof(UserRequirementStudy));
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
            return SharedSubjectView(model);
        }

        [HttpPost]
        public IActionResult AdditionalInformation(EditWorkpackageOneStepViewModel model)
        {
            return SharedEditSubject<EditAdditionalInformationCommand>(model, nameof(AdditionalInformation));
        }
    }
}
