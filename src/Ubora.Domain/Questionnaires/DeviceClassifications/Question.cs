using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications.Texts;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class Question : QuestionBase<Answer>
    {
        public Question(string id, IEnumerable<Answer> answers)
        {
            Id = id;
            Answers = answers.ToArray();
        }
        
        [JsonConstructor]
        protected Question()
        {
        }

        public string Text => QuestionTexts.ResourceManager.GetString(this.Id);
    }
}