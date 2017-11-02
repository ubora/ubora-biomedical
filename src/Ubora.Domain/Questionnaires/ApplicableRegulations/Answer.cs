using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class Answer : AnswerBase
    {
        [JsonConstructor]
        protected Answer()
        {
        }

        public Answer(string id, string nextQuestionId)
        {
            Id = id;
            NextQuestionId = nextQuestionId;
        }
    }
}