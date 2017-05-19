using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
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

        public IActionResult NextMainQuestion(Guid questionId)
        {
            var nextMainQuestion = _deviceClassification.GetNextMainQuestion(questionId);

            if (nextMainQuestion == null)
            {
                // TODO: Chack if you have a class saved. If you do then you know you device class by this stage.

                return RedirectToAction(nameof(NoClass), "DeviceClassification");
            }

            return RedirectToAction(nameof(GetMainQuestion), "DeviceClassification", new { questionId = nextMainQuestion.Id });
        }

        public IActionResult GetQuestions(Guid questionId, Guid? mainQuestionId)
        {
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

        public IActionResult Classification(Guid questionId, Guid mainQuestionId)
        {
            var classification = _deviceClassification.GetClassification(questionId);

            // TODO: Save classification

            var classificationViewModel = new ClassificationViewModel
            {
                ClassificationText = classification.Text,
                MainQuestionId = mainQuestionId,
                ClassificationId = classification.Id
            };

            return View(classificationViewModel);
        }

        public IActionResult NoClass()
        {
            return View();
        }
    }
}