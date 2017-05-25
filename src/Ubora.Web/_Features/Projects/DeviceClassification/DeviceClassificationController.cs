using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Web._Features.Projects.DeviceClassification
{
    public class DeviceClassificationController : ProjectController
    {
        private readonly IDeviceClassification _deviceClassification;

        public DeviceClassificationController(
            ICommandQueryProcessor processor,
            IDeviceClassification deviceClassification) : base(processor)
        {
            _deviceClassification = Find<Ubora.Domain.Projects.DeviceClassification.DeviceClassification>().Single();
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
                return BadRequest("No sub question found!");
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

        [HttpPost]
        public IActionResult Answer(AnswerViewModel answerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ModelState is invalid");
            }

            var questions = _deviceClassification.GetSubQuestions(answerViewModel.NextQuestionId);

            if (questions == null)
            {
                SetDeviceClassificationToProject(answerViewModel.NextQuestionId, Project.DeviceClassification);
                return RedirectToAction(nameof(NextMainQuestion), "DeviceClassification", new { mainQuestionId = answerViewModel.MainQuestionId });
            }

            return RedirectToAction(nameof(GetQuestions), "DeviceClassification", new { questionId = answerViewModel.NextQuestionId, mainQuestionId = answerViewModel.MainQuestionId });
        }

        public IActionResult CurrentClassification()
        {
            var currentClassificationViewModel = new CurrentClassificationViewModel
            {
                Classification = Project.DeviceClassification
            };

            return View(currentClassificationViewModel);
        }

        private void SetDeviceClassificationToProject(Guid questionId, string currentProjectDeviceClassificationText)
        {
            var currentClassification = _deviceClassification.GetClassification(questionId);

            var command = new SetDeviceClassificationForProjectCommand
            {
                ProjectId = ProjectId,
                DeviceClassification = currentClassification.Text,
                Actor = this.UserInfo
            };

            // Set if no classification exists on project
            if (string.IsNullOrEmpty(currentProjectDeviceClassificationText))
            {
                ExecuteUserProjectCommand(command);
                return;
            }

            var projectDeviceClassification = _deviceClassification.GetClassification(currentProjectDeviceClassificationText);

            // Set only if new classification is stronger than old one
            if (currentClassification > projectDeviceClassification)
            {
                ExecuteUserProjectCommand(command);
            }
        }

        public IActionResult NoClass()
        {
            return View();
        }
    }
}