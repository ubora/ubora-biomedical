using System;
using FluentAssertions;
using Ubora.Domain.Questionnaires;
using Xunit;

namespace Ubora.Domain.Tests.DeviceClassifications
{
    public class QuestionnaireTreeTests
    {
        [Fact]
        public void ValidateQuestions_Throws_When_Not_All_Questions_Are_Present_That_Are_Specified_As_Next_Question_In_Answers()
        {
            var questions = new[]
            {
                new TestQuestion("q1", new[]
                {
                    new TestAnswer("y", "q1_1"),
                    new TestAnswer("n", "not_present")
                }),
                new TestQuestion("q1_1", new TestAnswer[0])
            };

            var questionnaireTree = new TestQuestionnaireTree();

            // Act
            Action act = () => questionnaireTree.ValidateQuestionsPublic(questions);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ValidateQuestions_Throws_When_Answer_Refers_To_Its_Parent_Question_As_Next_Question()
        {
            var questions = new[]
            {
                new TestQuestion("parent", new[]
                {
                    new TestAnswer("y", "parent")
                })
            };

            var questionnaireTree = new TestQuestionnaireTree();

            // Act
            Action act = () => questionnaireTree.ValidateQuestionsPublic(questions);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ValidateQuestions_Throws_When_Question_Does_Not_Have_Any_Answers()
        {
            var questions = new[]
            {
                new TestQuestion("q", null)
            };

            var questionnaireTree = new TestQuestionnaireTree();

            // Act
            Action act = () => questionnaireTree.ValidateQuestionsPublic(questions);

            act.ShouldThrow<InvalidOperationException>();
        }
    }

    public class TestQuestionnaireTree : QuestionnaireTreeBase<TestQuestion, TestAnswer>
    {
        public void ValidateQuestionsPublic(TestQuestion[] questions)
        {
            ValidateQuestions(questions);
        }
    }

    public class TestQuestion : QuestionBase<TestAnswer>
    {
        public TestQuestion(string id, TestAnswer[] answers)
        {
            Id = id;
            Answers = answers;
        }
    }

    public class TestAnswer : Questionnaires.AnswerBase
    {
        public TestAnswer(string id, string nextQuestionId)
        {
            Id = id;
            NextQuestionId = nextQuestionId;
        }
    }
}
