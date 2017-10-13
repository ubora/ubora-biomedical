using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.ApplicableRegulations.Commands;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Domain.ApplicableRegulations.Texts;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class ApplicableRegulationsController : ProjectController
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            ViewData["Title"] = "Applicable regulations questionnaire";
            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
        }

        public virtual IActionResult Index([FromServices]QuestionnaireIndexViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(this.ProjectId);

            return View("QuestionnaireIndex", model);
        }

        public IActionResult Review(Guid questionnaireId, [FromServices]ReviewQuestionnaireViewModel.Factory modelFactory)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null) { return NotFound(); }

            if (!questionnaire.IsFinished)
            {
                // Can't review when all questions have not been answered.
                return RedirectToAction(nameof(Next), new { questionnaireId });
            }

            var model = modelFactory.Create(questionnaire.Questionnaire);
            return View("ReviewQuestionnaire", model);
        }

        [HttpPost]
        public IActionResult Start([FromServices]QuestionnaireIndexViewModel.Factory modelFactory)
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

        public virtual IActionResult Next(Guid questionnaireId)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null) { return NotFound(); }

            if (questionnaire.IsFinished)
            {
                return RedirectToAction(nameof(Review), new { questionnaireId });
            }

            var nextQuestion = questionnaire.Questionnaire.FindNextUnansweredQuestion();
            var model = new NextQuestionViewModel
            {
                Id = nextQuestion.Id,
                Text = nextQuestion.QuestionText,
                QuestionnaireId = questionnaire.Id,
                Note = nextQuestion.NoteText
        };
            return View("NextQuestion", model);
        }

        [HttpPost]
        public IActionResult AnswerYes(NextQuestionViewModel model)
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
        public IActionResult AnswerNo(NextQuestionViewModel model)
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