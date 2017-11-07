using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationsController : ProjectController
    {
        [HttpPost]
        public IActionResult Start()
        {
            var id = Guid.NewGuid();
            ExecuteUserProjectCommand(new StartClassifyingDeviceCommand
            {
                Id = id
            });

            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("TODO");
                //return Index();
            }

            return RedirectToAction(nameof(Current), new { questionnaireId = id });
        }

        public IActionResult Current(Guid questionnaireId)
        {
            var questionnaire = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaire == null)
            {
                return NotFound();
            }

            if (questionnaire.IsFinished)
            {
                return RedirectToAction(nameof(Review), new { questionnaireId });
            }

            var nextQuestion = questionnaire.QuestionnaireTree.FindNextUnansweredQuestion();

            return RedirectToAction(nameof(ViewQuestion), new { questionnaireId, questionId = nextQuestion.Id });
        }

        public virtual IActionResult ViewQuestion(Guid questionnaireId, string questionId, [FromServices]DeviceClassificationQuestionViewModel.Factory modelFactory)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            var models = modelFactory.Create(questionnaireAggregate, questionId);

            return View("ViewQuestion", models);
        }

        [HttpPost]
        public IActionResult Answer(AnswerDeviceClassificationQuestionPostModel model, [FromServices]DeviceClassificationQuestionViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
            {
                return ViewQuestion(model.QuestionnaireId, model.QuestionId, modelFactory);
            }

            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(model.QuestionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            ExecuteUserProjectCommand(new AnswerDeviceClassificationCommand
            {
                AnswerId = model.AnswerId,
                QuestionId = model.QuestionId,
                QuestionnaireId = model.QuestionnaireId
            });

            if (!ModelState.IsValid)
            {
                return ViewQuestion(model.QuestionnaireId, model.QuestionId, modelFactory);
            }

            return RedirectToAction(nameof(Current), new { model.QuestionnaireId });
        }

        public IActionResult Review(Guid questionnaireId, [FromServices]DeviceClassificationReviewViewModel.Factory modelFactory)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            if (!questionnaireAggregate.IsFinished)
            {
                return RedirectToAction(nameof(Current), new { questionnaireId });
            }

            var model = modelFactory.Create(questionnaireAggregate);
            return View("Review", model);
        }

        public virtual string GetClass(Guid questionnaireId)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            var model = new DeviceClassViewModel();

            var deviceClassHits = questionnaireAggregate.QuestionnaireTree.GetDeviceClassHits();
            foreach (var deviceClass in deviceClassHits)
            {
                if (deviceClass.Name == "I")
                {
                    model.HitsForClassOne++;
                }

                if (deviceClass.Name == "IIa")
                {
                    model.HitsForClassTwoA++;
                }

                if (deviceClass.Name == "IIb")
                {
                    model.HitsForClassTwoB++;
                }

                if (deviceClass.Name == "III")
                {
                    model.HitsForClassThree++;
                }
            }

            var chosenClass = deviceClassHits.Max();
            if (chosenClass != null)
            {
                model.ChosenClass = chosenClass.Name;
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}
