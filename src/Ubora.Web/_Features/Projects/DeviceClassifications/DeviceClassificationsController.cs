using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class DeviceClassViewModel
    {
        public int HitsForClassOne { get; set; }
        public int HitsForClassTwoA { get; set; }
        public int HitsForClassTwoB { get; set; }
        public int HitsForClassThree { get; set; }
    }

    public class DeviceClassificationQuestionViewModel
    {
        public string QuestionId { get; set; }
        public Guid QuestionnaireId { get; set; }
        public string Text { get; set; }
        public IEnumerable<DeviceClassificationAnswerViewModel> Answers { get; set; } = Enumerable.Empty<DeviceClassificationAnswerViewModel>();
        public string Class { get; set; }
    }

    public class DeviceClassificationAnswerViewModel
    {
        public string AnswerId { get; set; }
        public string Text { get; set; }
    }

    public class DeviceClassificationsController : ProjectController
    {
        public IActionResult Index()
        {
            return View("DeviceClassificationIndex");
        }

        [HttpPost]
        public IActionResult Start()
        {
            var id = Guid.NewGuid();
            ExecuteUserProjectCommand(new BeginClassifyingDeviceCommand
            {
                Id = id
            });

            if (!ModelState.IsValid)
            {
                return Index();
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

            //if (questionnaire.IsFinished)
            //{
            //    return RedirectToAction(nameof(Review), new { questionnaireId });
            //}

            var nextQuestion = questionnaire.QuestionnaireTree.FindNextUnansweredQuestion();

            return RedirectToAction(nameof(ViewQuestion), new { questionnaireId, questionId = nextQuestion.Id });
        }

        public virtual IActionResult ViewQuestion(Guid questionnaireId, string questionId)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            var question = questionnaireAggregate.QuestionnaireTree.FindQuestionOrThrow(questionId);

            var model = new DeviceClassificationQuestionViewModel
            {
                QuestionnaireId = questionnaireAggregate.Id,
                Text = question.Text,
                Answers = question.Answers.Select(answer => new DeviceClassificationAnswerViewModel
                {
                    AnswerId = answer.Id,
                    Text = answer.Text
                }),
                Class = GetClass(questionnaireId)
            };

            return View("ViewQuestion", model);
        }

        public virtual string GetClass(Guid questionnaireId)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            var model = new DeviceClassViewModel();

            foreach (var rule in questionnaireAggregate.QuestionnaireTree.Conditions.Where(x => x.IsFulfilled(questionnaireAggregate.QuestionnaireTree)))
            {
                if (rule.DeviceClass.Name == "I")
                {
                    model.HitsForClassOne++;
                }

                if (rule.DeviceClass.Name == "IIa")
                {
                    model.HitsForClassTwoA++;
                }

                if (rule.DeviceClass.Name == "IIb")
                {
                    model.HitsForClassTwoB++;
                }

                if (rule.DeviceClass.Name == "III")
                {
                    model.HitsForClassThree++;
                }
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        [HttpPost]
        public IActionResult Answer(Guid questionnaireId, string questionId, string answerId)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            //var question = questionnaireAggregate.QuestionnaireTree.FindQuestionOrThrow(questionId);
            ExecuteUserProjectCommand(new AnswerDeviceClassificationCommand
            {
                AnswerId = answerId,
                QuestionId = questionId,
                QuestionnaireId = questionnaireId
            });

            if (!ModelState.IsValid)
            {
                // TODO
                return Index();
            }

            return RedirectToAction(nameof(Current), new { questionnaireId });
        }
    }
}
