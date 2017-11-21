using Ubora.Domain.Questionnaires;

namespace Ubora.Domain.Tests.Questionnaires
{
    public class TestAnswer : AnswerBase
    {
        public TestAnswer(string id, string nextQuestionId)
        {
            Id = id;
            NextQuestionId = nextQuestionId;
        }
    }
}