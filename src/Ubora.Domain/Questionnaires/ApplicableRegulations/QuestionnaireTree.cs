using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class QuestionnaireTree : QuestionnaireTreeBase<Question, Answer>
    {
        [JsonConstructor]
        protected QuestionnaireTree()
        {
        }

        public QuestionnaireTree(IEnumerable<Question> questions)
        {
            Questions = questions.ToArray();
        }
    }
}