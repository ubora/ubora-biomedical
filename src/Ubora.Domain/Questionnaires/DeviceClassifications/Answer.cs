using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications.Texts;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class Answer : AnswerBase
    {
        public Answer(string answerId, string nextQuestionId)
        {
            Id = answerId;
            NextQuestionId = nextQuestionId;
        }

        [JsonConstructor]
        protected Answer()
        {
        }

        [JsonIgnore]
        public string Text => AnswerTexts.ResourceManager.GetString(this.Id);
    }
}