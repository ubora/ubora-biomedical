using System;
using System.Collections.Generic;
using System.Linq;
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
            public IEnumerable<DeviceClassificationQuestionViewModel> Create(DeviceClassificationAggregate aggregate, string questionId)
            {
                var questions = aggregate.QuestionnaireTree.Questions.Where(q => q.IsAnswered).ToList();
                var nextUnansweredQuestion = aggregate.QuestionnaireTree.FindNextUnansweredQuestion();
                if (nextUnansweredQuestion != null)
                {
                    questions.Add(nextUnansweredQuestion);
                }

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
                    IsSelected = string.Equals(q.Id, questionId, StringComparison.InvariantCultureIgnoreCase),
                    IsFirstQuestion = questions.First().Id == q.Id,
                    IsLastQuestion = questions.Last().Id == q.Id
                });

                return models;
            }
        }
    }
}