using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Questionnaires.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class ReviewQuestionnaireViewModel
    {
        public IEnumerable<QuestionAnswerListItem> QuestionAnswerList { get; set; }

        public class QuestionAnswerListItem
        {
            public string QuestionText { get; set; }
            public string IsoStandardText { get; set; }
            public string Answer { get; set; }
        }

        public class Factory
        {
            public ReviewQuestionnaireViewModel Create(ApplicableRegulationsQuestionnaireTree questionnaireTree)
            {
                if (questionnaireTree == null) { throw new ArgumentNullException(nameof(questionnaireTree)); }

                return new ReviewQuestionnaireViewModel
                {
                    QuestionAnswerList = questionnaireTree.AnsweredQuestions.Select(x => new QuestionAnswerListItem
                    {
                        QuestionText = x.QuestionText,
                        // ReSharper disable once PossibleInvalidOperationException
                        Answer = x.ChosenAnswerText,
                        IsoStandardText = x.IsoStandardHtmlText
                    })
                };
            }
        }
    }
}
