using System.Collections.Generic;
using System.Collections.Immutable;
using Ubora.Domain.Questionnaires;

namespace Ubora.Domain.Tests.Questionnaires
{
    public class TestQuestion : QuestionBase<TestAnswer>
    {
        public TestQuestion(string id, IEnumerable<TestAnswer> answers)
        {
            Id = id;
            Answers = answers.ToImmutableArray();
        }
    }
}