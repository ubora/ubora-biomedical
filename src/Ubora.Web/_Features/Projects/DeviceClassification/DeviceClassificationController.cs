using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Web._Features.Projects.DeviceClassification.Services;

namespace Ubora.Web._Features.Projects.DeviceClassification
{


    public class DeviceClassificationController : ProjectController
    {
        private readonly IDeviceClassification _deviceClassification;

        public DeviceClassificationController(
            ICommandQueryProcessor processor,
            IDeviceClassification deviceClassification) : base(processor)
        {
            _deviceClassification = deviceClassification;
        }

        public IActionResult GetMainQuestion(Guid? questionId)
        {
            if (questionId == null)
            {
                var initialMainQuestion = _deviceClassification.GetDefaultQuestion();

                var initialMainQuestionViewModel = new MainQuestionViewModel
                {
                    MainQuestionText = initialMainQuestion.Text,
                    MainQuestionId = initialMainQuestion.Id
                };

                return View(initialMainQuestionViewModel);
            }

            var mainQuestion = _deviceClassification.GetMainQuestion(questionId.Value);

            var mainQuestionViewModel = new MainQuestionViewModel
            {
                MainQuestionText = mainQuestion.Text,
                MainQuestionId = mainQuestion.Id
            };

            return View(mainQuestionViewModel);
        }

        public IActionResult NextMainQuestion(Guid mainQuestionId)
        {
            if (mainQuestionId == default(Guid))
            {
                return BadRequest();
            }

            var nextMainQuestion = _deviceClassification.GetNextMainQuestion(mainQuestionId);

            if (nextMainQuestion == null)
            {
                if (string.IsNullOrEmpty(Project.DeviceClassification))
                {
                    return RedirectToAction(nameof(NoClass), "DeviceClassification");
                }

                return RedirectToAction(nameof(CurrentClassification), "DeviceClassification");
            }

            return RedirectToAction(nameof(GetMainQuestion), "DeviceClassification", new { questionId = nextMainQuestion.Id });
        }

        public IActionResult GetQuestions(Guid questionId, Guid? mainQuestionId)
        {
            if (questionId == default(Guid))
            {
                return BadRequest();
            }
            var questions = _deviceClassification.GetSubQuestions(questionId);

            if (questions == null)
            {
                return RedirectToAction(nameof(Classification), "DeviceClassification", new { questionId = questionId, mainQuestionId = mainQuestionId });
            }

            if (mainQuestionId == null)
            {
                mainQuestionId = questions.First().MainQuestionId;
            }

            var questionsViewModel = new QuestionsViewModel
            {
                Questions = questions,
                MainQuestionId = mainQuestionId.Value
            };

            return View(questionsViewModel);
        }

        public IActionResult CurrentClassification()
        {
            var currentClassificationViewModel = new CurrentClassificationViewModel
            {
                Classification = Project.DeviceClassification
            };

            return View(currentClassificationViewModel);
        }

        public IActionResult Classification(Guid questionId, Guid mainQuestionId)
        {
            if (mainQuestionId == default(Guid))
            {
                return BadRequest();
            }

            if (questionId == default(Guid))
            {
                return BadRequest();
            }

            var currentClassification = _deviceClassification.GetClassification(questionId);

            var projectDeviceClassificationText = Project.DeviceClassification;

            var command = new SetDeviceClassificationForProjectCommand
            {
                ProjectId = ProjectId,
                DeviceClassification = currentClassification.Text,
                Actor = this.UserInfo
            };

            if (string.IsNullOrEmpty(projectDeviceClassificationText))
            {
                ExecuteUserProjectCommand(command);
                return RedirectToAction(nameof(NextMainQuestion), "DeviceClassification", new { mainQuestionId = mainQuestionId });
            }

            var projectDeviceClassification = _deviceClassification.GetClassification(projectDeviceClassificationText);

            if (currentClassification > projectDeviceClassification)
            {
                ExecuteUserProjectCommand(command);
            }

            return RedirectToAction(nameof(NextMainQuestion), "DeviceClassification", new { mainQuestionId = mainQuestionId });
        }

        public IActionResult NoClass()
        {
            return View();
        }
    }
}