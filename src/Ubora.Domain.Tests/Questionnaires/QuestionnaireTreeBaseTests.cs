using System;
using FluentAssertions;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires
{
    public class QuestionnaireTreeBaseTests
    {
        [Fact]
        public void Constructor_Throws_When_Not_All_Questions_Are_Present_That_Are_Specified_As_Next_Question_In_Answers()
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

            // Act
            Action act = () => new TestQuestionnaireTree(questions);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().Contain("not found");
        }

        [Fact]
        public void Constructor_Throws_When_Answer_Refers_To_Its_Parent_Question_As_Next_Question()
        {
            var questions = new[]
            {
                new TestQuestion("parent", new[]
                {
                    new TestAnswer("y", "parent")
                })
            };

            // Act
            Action act = () => new TestQuestionnaireTree(questions);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().Contain("parent");
        }

        [Fact]
        public void Constructor_Throws_When_Question_Does_Not_Have_Any_Answers()
        {
            var questions = new[]
            {
                new TestQuestion("q", null)
            };

            // Act
            Action act = () => new TestQuestionnaireTree(questions);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("answers");
        }

        [Fact]
        public void Constructor_Throws_When_Questions_With_Duplicate_Id()
        {
            var questions = new[]
            {
                new TestQuestion("q", new[]
                {
                    new TestAnswer("y", null)
                }),
                new TestQuestion("q", new[]
                {
                    new TestAnswer("y", null)
                })
            };

            // Act
            Action act = () => new TestQuestionnaireTree(questions);

            act.ShouldThrow<InvalidOperationException>().And.Message.Should().Contain("duplicate");
        }
    }
}
