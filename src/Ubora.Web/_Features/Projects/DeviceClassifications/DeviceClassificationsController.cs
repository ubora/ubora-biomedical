﻿using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;
using Ubora.Web._Features.Projects.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationsController : ProjectController
    {
        public virtual IActionResult Index([FromServices]DeviceClassificationIndexViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(this.ProjectId);

            return View("DeviceClassificationIndex", model);
        }

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
                ModelState.Remove(nameof(model.QuestionId)); // When the question is already answered, we want a new form created with the new question ID input.
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
    }
}
