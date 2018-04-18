using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireTree : QuestionnaireTreeBase<Question, Answer>
    {
        public ApplicableRegulationsQuestionnaireTree(IEnumerable<Question> questions)
        {
            Questions = questions.ToImmutableArray();
        }

        [JsonConstructor]
        protected ApplicableRegulationsQuestionnaireTree()
        {
        }
    }
}