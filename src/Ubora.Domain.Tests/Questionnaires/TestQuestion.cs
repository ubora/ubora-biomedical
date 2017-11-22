using Ubora.Domain.Questionnaires;

namespace Ubora.Domain.Tests.Questionnaires
{
    public class TestQuestion : QuestionBase<TestAnswer>
    {
        public TestQuestion(string id, TestAnswer[] answers)
        {
            Id = id;
            Answers = answers;
        }
    }
}