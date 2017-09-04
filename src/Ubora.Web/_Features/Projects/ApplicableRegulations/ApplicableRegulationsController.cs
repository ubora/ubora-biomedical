using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class ApplicableRegulationsController : ProjectController
    {
        public IActionResult Index()
        {
            return View(nameof(Index));
        }

        [HttpPost]
        public IActionResult Start()
        {
            var id = Guid.NewGuid();
            ExecuteUserProjectCommand(new StartQuestionnaireCommand
            {
                NewQuestionnaireId = id
            });

            if (!ModelState.IsValid)
            {
                return Index();
            }

            return RedirectToAction(nameof(Next), new { questionnaireId = id });
        }

        public IActionResult Next()
        {
            // check if questionnaire exists for project

            var aggregate = QueryProcessor.Find<ProjectQuestionnaireAggregate>()
                .Single(x => x.ProjectId == this.Project.Id && x.FinishedAt == null);

            var nextQuestion = aggregate.Questionnaire.GetNextUnanswered();

            var model = new QuestionViewModel
            {
                Id = nextQuestion.Id,
                Text = nextQuestion.Text,
                QuestionnaireId = aggregate.Id,
            };

            return View(nameof(Next), model);
        }

        [HttpPost]
        public IActionResult Yes(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Next();
            }

            ExecuteUserProjectCommand(new AnswerQuestionCommand
            {
                QuestionnaireId = model.QuestionnaireId,
                QuestionId = model.Id,
                Answer = true
            });

            if (!ModelState.IsValid)
            {
                return Next();
            }

            return RedirectToAction(nameof(Next), new { questionnaireId = model.QuestionnaireId });
        }

        [HttpPost]
        public IActionResult No(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Next();
            }

            ExecuteUserProjectCommand(new AnswerQuestionCommand
            {
                QuestionnaireId = model.QuestionnaireId,
                QuestionId = model.Id,
                Answer = false
            });

            if (!ModelState.IsValid)
            {
                return Next();
            }

            return RedirectToAction(nameof(Next), new { questionnaireId = model.QuestionnaireId });
        }
    }
}
