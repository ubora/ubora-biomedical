using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.ApplicableRegulations.Commands;
using Ubora.Web._Features.Projects._Shared;

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

        public IActionResult PreviousQuestion(Guid questionnaireId, Guid questionId)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null) { return NotFound(); }
            var answeredQuestions = questionnaire.Questionnaire.GetAllQuestions().Where(x => x.Answer.HasValue);


            var previousQuestion = PreviewsAnsweredQuestion(answeredQuestions, questionId);
            if (previousQuestion == null)
            {
                //peaks indeksi lehele minema
                return NotFound();
            }

            var model = new LastQuestionViewModel
            {
                Id = previousQuestion.Id,
                Text = previousQuestion.QuestionText,
                QuestionnaireId = questionnaireId,
                Answer = previousQuestion.Answer.Value
            };
            
            //var lastQuestion2 = questionnaire.Questionnaire.GetAllQuestions().(x => x.Id == Guid.NewGuid());

            return View("NavigateQuestion", model);
        }
        public IActionResult ForwardQuestion(Guid questionnaireId, Guid questionId)
        {
            var questionnaire = QueryProcessor.FindById<ApplicableRegulationsQuestionnaireAggregate>(questionnaireId);
            if (questionnaire == null) { return NotFound(); }
            var answeredQuestions = questionnaire.Questionnaire.GetAllQuestions().Where(x => x.Answer.HasValue);

            var nextQuestion = NextAnsweredQuestion(answeredQuestions, questionId);
            if (nextQuestion == null)
            {
                return RedirectToAction(nameof(Next), new { questionnaireId });
            }
            var model = new LastQuestionViewModel()
            {
                Id = nextQuestion.Id,
                Text = nextQuestion.QuestionText,
                QuestionnaireId = questionnaire.Id,
                Answer = nextQuestion.Answer.Value

            };
            return View("NavigateQuestion", model);
        }



        private static Question PreviewsAnsweredQuestion(IEnumerable<Question> answeredQuestions, Guid lastQuestionId)
        {
            Question prev = null;
            foreach (var q in answeredQuestions)
            {
                if (q.Id == lastQuestionId)
                {
                    break;
                }
                prev = q;
            }

            if (prev == null)
            {

            }
            return prev;
        }
        private static Question NextAnsweredQuestion(IEnumerable<Question> answeredQuestions, Guid lastQuestionId)
        {
            Question prev = null;
            foreach (var q in answeredQuestions.Reverse())
            {
                if (q.Id == lastQuestionId)
                {
                    break;
                }
                prev = q;
            }

            if (prev == null)
            {
                return null;
            }
            return prev;
        }
    }
}