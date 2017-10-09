using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class ReviewQuestionnaireViewModel
    {
        public IEnumerable<QuestionAnswerListItem> QuestionAnswerList { get; set; }

        public class QuestionAnswerListItem
        {
            public string QuestionText { get; set; }
            public string IsoStandardText { get; set; }
            public bool Answer { get; set; }
        }

        public class Factory
        {
            public ReviewQuestionnaireViewModel Create(Questionnaire questionnaire)
            {
                if (questionnaire == null) { throw new ArgumentNullException(nameof(questionnaire)); }

                var answeredQuestions = questionnaire.GetAllQuestions()
                    .Where(x => x.Answer.HasValue);

                return new ReviewQuestionnaireViewModel
                {
                    QuestionAnswerList = answeredQuestions.Select(x => new QuestionAnswerListItem
                    {
                        QuestionText = x.QuestionText,
                        // ReSharper disable once PossibleInvalidOperationException
                        Answer = x.Answer.Value,
                        IsoStandardText = x.IsoStandardHtmlText
                    })
                };
            }
        }
    }
}
