using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Web._Features.Projects.WorkpackageOneManagement
{
    public class WorkpackageOverviewViewModel
    {
        public IEnumerable<WorkpackageViewModel> Workpackages { get; set; }
    }

    public class WorkpackageViewModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }

    public class TaskViewModel
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }

    public class WorkpackageOneController : ProjectController
    {
        public WorkpackageOne WorkpackageOne => QueryProcessor.FindById<WorkpackageOne>(ProjectId);

        public WorkpackageOneController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Overview()
        {
            var model = new WorkpackageOverviewViewModel
            {
                Workpackages = new[]
                {
                    new WorkpackageViewModel
                    {
                        Title = "Workpackage 1",
                        Url = this.Url.Action(nameof(Index)),
                        Tasks = new []
                        {
                            new TaskViewModel { Title = "DescriptionOfNeed", Url = Url.Action(nameof(DescriptionOfNeed)) },
                            new TaskViewModel { Title = "DescriptionOfExistingSolutionsAndAnalysis", Url = Url.Action(nameof(DescriptionOfExistingSolutionsAndAnalysis)) },
                            new TaskViewModel { Title = "ProductFunctionality", Url = Url.Action(nameof(ProductFunctionality)) },
                            new TaskViewModel { Title = "ProductPerformance", Url = Url.Action(nameof(ProductPerformance)) },
                            new TaskViewModel { Title = "ProductUsability", Url = Url.Action(nameof(ProductUsability)) },
                            new TaskViewModel { Title = "ProductSafety", Url = Url.Action(nameof(ProductSafety)) },
                            new TaskViewModel { Title = "PatientPopulationStudy", Url = Url.Action(nameof(PatientPopulationStudy)) },
                            new TaskViewModel { Title = "UserRequirementStudy", Url = Url.Action(nameof(UserRequirementStudy)) },
                        }
                    }
                }
            };

            return View(model);
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Overview));
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

        public IActionResult DescriptionOfNeed(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Description of need",
                Value = WorkpackageOne.DescriptionOfNeed,
                PostUrl = Url.Action(nameof(DescriptionOfNeed)),
                EditUrl = Url.Action(nameof(DescriptionOfNeed), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfNeed(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditDescriptionOfNeedCommand>(model, nameof(DescriptionOfNeed));
        }

        public IActionResult DescriptionOfExistingSolutionsAndAnalysis(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Description of existing solutions and analysis",
                Value = WorkpackageOne.DescriptionOfExistingSolutionsAndAnalysis,
                PostUrl = Url.Action(nameof(DescriptionOfExistingSolutionsAndAnalysis)),
                EditUrl = Url.Action(nameof(DescriptionOfExistingSolutionsAndAnalysis), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult DescriptionOfExistingSolutionsAndAnalysis(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditDescriptionOfExistingSolutionsAndAnalysisCommand>(model, nameof(DescriptionOfExistingSolutionsAndAnalysis));
        }

        public IActionResult ProductFunctionality(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product functionality",
                Value = WorkpackageOne.ProductFunctionality,
                PostUrl = Url.Action(nameof(ProductFunctionality)),
                EditUrl = Url.Action(nameof(ProductFunctionality), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductFunctionality(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductFunctionalityCommand>(model, nameof(ProductFunctionality));
        }

        public IActionResult ProductPerformance(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product performance",
                Value = WorkpackageOne.ProductPerformance,
                PostUrl = Url.Action(nameof(ProductPerformance)),
                EditUrl = Url.Action(nameof(ProductPerformance), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductPerformance(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductPerformanceCommand>(model, nameof(ProductPerformance));
        }

        public IActionResult ProductUsability(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product usability",
                Value = WorkpackageOne.ProductUsability,
                PostUrl = Url.Action(nameof(ProductUsability)),
                EditUrl = Url.Action(nameof(ProductUsability), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductUsability(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductUsabilityCommand>(model, nameof(ProductUsability));
        }

        public IActionResult ProductSafety(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Product safety",
                Value = WorkpackageOne.ProductSafety,
                PostUrl = Url.Action(nameof(ProductSafety)),
                EditUrl = Url.Action(nameof(ProductSafety), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult ProductSafety(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditProductSafetyCommand>(model, nameof(ProductSafety));
        }

        public IActionResult PatientPopulationStudy(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Population study",
                Value = WorkpackageOne.PatientPopulationStudy,
                PostUrl = Url.Action(nameof(PatientPopulationStudy)),
                EditUrl = Url.Action(nameof(PatientPopulationStudy), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult PatientPopulationStudy(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditPatientPopulationStudyCommand>(model, nameof(PatientPopulationStudy));
        }

        public IActionResult UserRequirementStudy(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "User requirement study",
                Value = WorkpackageOne.UserRequirementStudy,
                PostUrl = Url.Action(nameof(UserRequirementStudy)),
                EditUrl = Url.Action(nameof(UserRequirementStudy), new { edit = true }),
                IsEdit = edit,
            };
            return SharedView(model);
        }

        [HttpPost]
        public IActionResult UserRequirementStudy(EditWorkpackageOneViewModel model)
        {
            return SharedEdit<EditUserRequirementStudyCommand>(model, nameof(UserRequirementStudy));
        }

        public IActionResult AdditionalInformation(bool edit = false)
        {
            var model = new EditWorkpackageOneViewModel
            {
                Title = "Additional information",
                Value = WorkpackageOne.PatientPopulationStudy,
                PostUrl = Url.Action(nameof(PatientPopulationStudy)),
                EditUrl = Url.Action(nameof(PatientPopulationStudy), new { edit = true }),
                IsEdit = edit,
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
