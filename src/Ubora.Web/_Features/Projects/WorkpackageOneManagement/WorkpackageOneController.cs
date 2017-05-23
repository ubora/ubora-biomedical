using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Web._Features.Projects.WorkpackageOneManagement
{
    public class WorkpackageOneController : ProjectController
    {
        public WorkpackageOne WorkpackageOne => QueryProcessor.FindById<WorkpackageOne>(ProjectId);

        public WorkpackageOneController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        private IActionResult SharedView(object model)
        {
            return View("WorkpackageOneSharedView", model);
        }

        private IActionResult SharedEdit<T>(EditWorkpackageOneViewModel model, string getActionName) where T : EditWorkpackageOneCommand, new()
        {
            if (!ModelState.IsValid)
            {
                return SharedView(model);
            }

            this.ExecuteUserProjectCommand(new T
            {
                NewValue = model.Value
            });

            if (!ModelState.IsValid)
            {
                return SharedView(model);
            }

            return RedirectToAction(getActionName);
        }

        public IActionResult DescriptionOfNeed()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Description of need",
                Value = WorkpackageOne.DescriptionOfNeed,
                PostUrl = Url.Action(nameof(DescriptionOfNeed))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfNeed(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditDescriptionOfNeedCommand>(model, nameof(DescriptionOfNeed));
        }

        public IActionResult DescriptionOfExistingSolutionsAndAnalysis()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Description of existing solutions and analysis",
                Value = WorkpackageOne.DescriptionOfExistingSolutionsAndAnalysis,
                PostUrl = Url.Action(nameof(DescriptionOfExistingSolutionsAndAnalysis))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfExistingSolutionsAndAnalysis(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditDescriptionOfExistingSolutionsAndAnalysisCommand>(model, nameof(DescriptionOfExistingSolutionsAndAnalysis));
        }

        public IActionResult ProductFunctionality()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product functionality",
                Value = WorkpackageOne.ProductFunctionality,
                PostUrl = Url.Action(nameof(ProductFunctionality))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductFunctionality(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductFunctionalityCommand>(model, nameof(ProductFunctionality));
        }

        public IActionResult ProductPerformance()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product performance",
                Value = WorkpackageOne.ProductPerformance,
                PostUrl = Url.Action(nameof(ProductPerformance))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductPerformance(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductPerformanceCommand>(model, nameof(ProductPerformance));
        }

        public IActionResult ProductUsability()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product usability",
                Value = WorkpackageOne.ProductUsability,
                PostUrl = Url.Action(nameof(ProductUsability))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductUsability(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductUsabilityCommand>(model, nameof(ProductUsability));
        }

        public IActionResult ProductSafety()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product safety",
                Value = WorkpackageOne.ProductSafety,
                PostUrl = Url.Action(nameof(ProductSafety))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductSafety(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductSafetyCommand>(model, nameof(ProductSafety));
        }

        public IActionResult PatientPopulationStudy()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Population study",
                Value = WorkpackageOne.PatientPopulationStudy,
                PostUrl = Url.Action(nameof(PatientPopulationStudy))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult PatientPopulationStudy(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditPatientPopulationStudyCommand>(model, nameof(PatientPopulationStudy));
        }

        public IActionResult UserRequirementStudy()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "User requirement study",
                Value = WorkpackageOne.UserRequirementStudy,
                PostUrl = Url.Action(nameof(UserRequirementStudy))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult UserRequirementStudy(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditUserRequirementStudyCommand>(model, nameof(UserRequirementStudy));
        }

        public IActionResult AdditionalInformation()
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Additional information",
                Value = WorkpackageOne.PatientPopulationStudy,
                PostUrl = Url.Action(nameof(PatientPopulationStudy))
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult AdditionalInformation(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditAdditionalInformationCommand>(model, nameof(AdditionalInformation));
        }
    }
}
