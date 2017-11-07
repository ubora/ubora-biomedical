using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireTree : QuestionnaireTreeBase<Question, Answer>
    {
        public ApplicableRegulationsQuestionnaireTree(IEnumerable<Question> questions)
        {
            Questions = questions.ToArray();
        }

        [JsonConstructor]
        protected ApplicableRegulationsQuestionnaireTree()
        {
        }
    }
}