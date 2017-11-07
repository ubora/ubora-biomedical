using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class Answer : AnswerBase
    {
        public Answer(string id, string nextQuestionId)
        {
            Id = id;
            NextQuestionId = nextQuestionId;
        }

        [JsonConstructor]
        protected Answer()
        {
        }
    }
}