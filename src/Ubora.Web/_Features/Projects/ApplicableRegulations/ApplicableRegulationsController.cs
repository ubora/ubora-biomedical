﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features.Projects.Workpackages;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class ApplicableRegulationsController : ProjectController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData[nameof(PageTitle)] = "Applicable regulations questionnaire";
            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.RegulationChecklist;
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
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Start([FromServices]QuestionnaireIndexViewModel.Factory modelFactory)
        {
            var id = Guid.NewGuid();
            ExecuteUserProjectCommand(new StartApplicableRegulationsQuestionnaireCommand
            {
                NewQuestionnaireId = id
            }, Notice.Success(SuccessTexts.ApplicableRegulationsQuestionnaireStarted));

            if (!ModelState.IsValid)
            {
                return Index(modelFactory);
            }

            return RedirectToAction(nameof(CurrentUnansweredQuestion), new { questionnaireId = id });
        }

        [HttpPost]
        public IActionResult Stop(Guid questionnaireId, [FromServices]QuestionnaireIndexViewModel.Factory modelFactory)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null)
            {
                return NotFound();
            }

            ExecuteUserProjectCommand(new StopApplicableRegulationsQuestionnaireCommand
            {
                QuestionnaireId = questionnaireId,
            }, Notice.Success(SuccessTexts.ApplicableRegulationsQuestionnaireStopped));

            if (!ModelState.IsValid)
            {
                return Index(modelFactory);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Retake(Guid questionnaireId, [FromServices]QuestionnaireIndexViewModel.Factory modelFactory)
        {
            var id = Guid.NewGuid();
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null)
            {
                return NotFound();
            }

            ExecuteUserProjectCommand(new StopAndStartApplicableRegulationsQuestionnaireCommand
            {
                QuestionnaireId = questionnaireId,
                NewQuestionnaireId = id
            }, Notice.Success(SuccessTexts.ApplicableRegulationsQuestionnaireRetaken));

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

        public virtual IActionResult ReviewQuestion(Guid questionnaireId, string questionId, [FromServices]NextQuestionViewModel.Factory modelFactory)
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
                AnswerId = "y"
            }, Notice.None("There is enough visual feedback."));

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
                AnswerId = "n"
            }, Notice.None("There is enough visual feedback."));

            if (!ModelState.IsValid)
            {
                return CurrentUnansweredQuestion(model.QuestionnaireId);
            }

            return RedirectToAction(nameof(CurrentUnansweredQuestion), new { questionnaireId = model.QuestionnaireId });
        }
    }
}