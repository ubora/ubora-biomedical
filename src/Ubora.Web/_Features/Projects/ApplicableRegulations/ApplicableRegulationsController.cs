using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.ApplicableRegulations.Commands;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class ApplicableRegulationsController : ProjectController
    {
        public IActionResult Index([FromServices]IndexViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(this.ProjectId);
            return View(nameof(Index), model);
        }

        public IActionResult Review(Guid questionnaireId, [FromServices]ReviewViewModel.Factory modelFactory)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null) { return NotFound(); }

            if (!questionnaire.IsFinished)
            {
                return RedirectToAction(nameof(Next), new { questionnaireId });
            }

            var model = modelFactory.Create(questionnaire.Questionnaire);
            return View("Review", model);
        }

        [HttpPost]
        public IActionResult Start([FromServices]IndexViewModel.Factory modelFactory)
        {
            var id = Guid.NewGuid();
            ExecuteUserProjectCommand(new StartApplicableRegulationsQuestionnaireCommand
            {
                NewQuestionnaireId = id
            });

            if (!ModelState.IsValid)
            {
                return Index(modelFactory);
            }

            return RedirectToAction(nameof(Next), new { questionnaireId = id });
        }

        public IActionResult Next(Guid questionnaireId)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null) { return NotFound(); }

            if (questionnaire.IsFinished)
            {
                return RedirectToAction(nameof(Review), new { questionnaireId });
            }

            var nextQuestion = questionnaire.Questionnaire.FindNextUnansweredQuestion();
            var model = new QuestionViewModel
            {
                Id = nextQuestion.Id,
                Text = nextQuestion.QuestionText,
                QuestionnaireId = questionnaire.Id,
            };
            return View("Next", model);
        }

        [HttpPost]
        public IActionResult Yes(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Next(model.QuestionnaireId);
            }

            ExecuteUserProjectCommand(new AnswerApplicableRegulationsQuestionCommand
            {
                QuestionnaireId = model.QuestionnaireId,
                QuestionId = model.Id,
                Answer = true
            });

            if (!ModelState.IsValid)
            {
                return Next(model.QuestionnaireId);
            }

            return RedirectToAction(nameof(Next), new { questionnaireId = model.QuestionnaireId });
        }

        [HttpPost]
        public IActionResult No(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Next(model.QuestionnaireId);
            }

            ExecuteUserProjectCommand(new AnswerApplicableRegulationsQuestionCommand
            {
                QuestionnaireId = model.QuestionnaireId,
                QuestionId = model.Id,
                Answer = false
            });

            if (!ModelState.IsValid)
            {
                return Next(model.QuestionnaireId);
            }

            return RedirectToAction(nameof(Next), new { questionnaireId = model.QuestionnaireId });
        }
    }
}