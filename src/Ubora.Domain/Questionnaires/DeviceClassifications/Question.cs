using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications.Texts;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class Question : QuestionBase<Answer>
    {
        public Question(string id, IEnumerable<Answer> answers)
        {
            Id = id;
            Answers = answers.ToImmutableArray();
        }
        
        [JsonConstructor]
        protected Question()
        {
        }

        [JsonIgnore]
        public string Text => QuestionTexts.ResourceManager.GetString(this.Id);
    }
}