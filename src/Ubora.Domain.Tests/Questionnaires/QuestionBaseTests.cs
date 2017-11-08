using System;
using FluentAssertions;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires
{
    public class QuestionBaseTests
    {
        [Fact]
        public void ChooseAnswer_Marks_Chosen_Answer_And_Not_Chosen_Answers()
        {
            var answers = new[]
            {
                new TestAnswer("yes", null),
                new TestAnswer("no", null),
                new TestAnswer("maybe", null)
            };
            var question = new TestQuestion("q1", answers);
            var at = DateTime.UtcNow;

            // Act
            question.ChooseAnswer("no", at);

            // Assert
            answers[0].IsChosen.Should().BeFalse();
            answers[0].AnsweredAt.Should().Be(at);

            answers[1].IsChosen.Should().BeTrue();
            answers[1].AnsweredAt.Should().Be(at);

            answers[2].IsChosen.Should().BeFalse();
            answers[2].AnsweredAt.Should().Be(at);
        }

        [Fact]
        public void ChooseAnswer_Throws_When_Already_Answered()
        {
            var question = new TestQuestion("q1", new[]
            {
                new TestAnswer("yes", null),
                new TestAnswer("no", null),
                new TestAnswer("maybe", null)
            });
            var at = DateTime.UtcNow;

            question.ChooseAnswer("no", at);

            // Act
            Action act = () => question.ChooseAnswer("no", at);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ChooseAnswer_Throws_When_Answer_Not_Found()
        {
            var question = new TestQuestion("q1", new[]
            {
                new TestAnswer("yes", null),
                new TestAnswer("no", null),
                new TestAnswer("maybe", null)
            });
            var at = DateTime.UtcNow;

            // Act
            Action act = () => question.ChooseAnswer("whatever", at);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
