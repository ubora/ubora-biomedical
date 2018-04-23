using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Questionnaires.DeviceClassifications;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationQuestionViewModel : AnswerDeviceClassificationQuestionPostModel
    {
        public string QuestionText { get; set; }
        public IEnumerable<DeviceClassificationAnswerViewModel> Answers { get; set; } = Enumerable.Empty<DeviceClassificationAnswerViewModel>();
        public bool IsSelected { get; set; }
        public bool IsAnswered { get; set; }
        public bool IsLastQuestion { get; set; }
        public bool IsFirstQuestion { get; set; }

        public class Factory
        {
            public IEnumerable<DeviceClassificationQuestionViewModel> Create(DeviceClassificationAggregate aggregate, string selectedQuestionId)
            {
                var questions = aggregate.QuestionnaireTree.AnsweredQuestions.ToList();
                var nextUnansweredQuestion = aggregate.QuestionnaireTree.FindNextUnansweredQuestion();
                if (nextUnansweredQuestion != null)
                {
                    questions.Add(nextUnansweredQuestion);
                }

                var firstQuestionId = questions.First().Id;
                var lastQuestionId = questions.Last().Id;

                var models = questions.Select(q => new DeviceClassificationQuestionViewModel
                {
                    QuestionId = q.Id,
                    QuestionnaireId = aggregate.Id,
                    QuestionText = q.Text,
                    Answers = q.Answers.Select(answer => new DeviceClassificationAnswerViewModel
                    {
                        AnswerId = answer.Id,
                        AnswerText = answer.Text,
                        WasAnswerChosen = answer.IsChosen
                    }),
                    IsAnswered = q.IsAnswered,
                    IsSelected = (q.Id == selectedQuestionId),
                    IsFirstQuestion = (firstQuestionId == q.Id),
                    IsLastQuestion = (lastQuestionId == q.Id)
                });

                return models;
            }
        }
    }
}