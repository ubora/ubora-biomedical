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
                return RedirectToAction(nameof(CurrentUnansweredQuestion), new { questionnaireId });
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

            return RedirectToAction(nameof(CurrentUnansweredQuestion), new { questionnaireId = id });
        }

        public virtual IActionResult CurrentUnansweredQuestion(Guid questionnaireId)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null)
            {
                return NotFound();
            }

            if (questionnaire.IsFinished)
            {
                return RedirectToAction(nameof(Review), new { questionnaireId });
            }

            var nextQuestion = questionnaire.Questionnaire.FindNextUnansweredQuestion();

            return RedirectToAction(nameof(ReviewQuestion), new { questionnaireId, questionId = nextQuestion.Id });
        }

        public virtual IActionResult ReviewQuestion(Guid questionnaireId, Guid questionId, [FromServices]NextQuestionViewModel.Factory modelFactory)
        {
            var questionnaireAggregate = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            var model = modelFactory.Create(questionnaireAggregate, questionId);
            return View("CurrentUnansweredQuestion", model);
        }

        [HttpPost]
        public IActionResult AnswerYes(NextQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUnansweredQuestion(model.QuestionnaireId);
            }

            ExecuteUserProjectCommand(new AnswerApplicableRegulationsQuestionCommand
            {
                QuestionnaireId = model.QuestionnaireId,
                QuestionId = model.Id,
                Answer = true
            });

            if (!ModelState.IsValid)
            {
                return CurrentUnansweredQuestion(model.QuestionnaireId);
            }

            return RedirectToAction(nameof(CurrentUnansweredQuestion), new { questionnaireId = model.QuestionnaireId });
        }

        [HttpPost]
        public IActionResult AnswerNo(NextQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUnansweredQuestion(model.QuestionnaireId);
            }

            ExecuteUserProjectCommand(new AnswerApplicableRegulationsQuestionCommand
            {
                QuestionnaireId = model.QuestionnaireId,
                QuestionId = model.Id,
                Answer = false
            });

            if (!ModelState.IsValid)
            {
                return CurrentUnansweredQuestion(model.QuestionnaireId);
            }

            return RedirectToAction(nameof(CurrentUnansweredQuestion), new { questionnaireId = model.QuestionnaireId });
        }
    }
}