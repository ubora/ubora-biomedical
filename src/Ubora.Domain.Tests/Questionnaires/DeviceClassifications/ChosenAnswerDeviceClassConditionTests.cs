using System;
using System.Collections.Generic;
using FluentAssertions;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications
{
    public class ChosenAnswerDeviceClassConditionTests
    {
        [Fact]
        public void IsFulfilled_Throws_When_Conditon_Not_From_Questionnaire()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", null)
                })
            };

            var questionnaireTree = new DeviceClassificationQuestionnaireTree(questions, new ChosenAnswerDeviceClassCondition[0]);

            var condition = new ChosenAnswerDeviceClassCondition("q2", "a2", DeviceClass.One);

            // Act
            Action act = () => condition.IsFulfilled(questionnaireTree);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().Contain("not from");
        }

        [Fact]
        public void IsFulfilled_Returns_True_When_Exact_Chosen_Answers_Found_For_Given_Questions()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", "q2"),
                    new Answer("n", null)
                }),
                new Question("q2", new[]
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            var condition = new ChosenAnswerDeviceClassCondition("duplicate_id", new Dictionary<string, string>
            {
                { "q1", "y" },
                { "q2", "n" }
            }, DeviceClass.One);

            var questionnaireTree = new DeviceClassificationQuestionnaireTree(questions, new [] { condition });

            questionnaireTree.FindNextUnansweredQuestion().ChooseAnswer("y", DateTime.UtcNow);
            questionnaireTree.FindNextUnansweredQuestion().ChooseAnswer("n", DateTime.UtcNow);

            // Act
            var result = condition.IsFulfilled(questionnaireTree);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsFulfilled_Returns_False_When_Not_Exact_Chosen_Answers_Found()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", "q2"),
                    new Answer("n", "q2")
                }),
                new Question("q2", new[]
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            var condition = new ChosenAnswerDeviceClassCondition("duplicate_id", new Dictionary<string, string>
            {
                { "q1", "y" },
                { "q2", "n" }
            }, DeviceClass.One);

            var questionnaireTree = new DeviceClassificationQuestionnaireTree(questions, new[] { condition });

            questionnaireTree.FindNextUnansweredQuestion().ChooseAnswer("n", DateTime.UtcNow);
            questionnaireTree.FindNextUnansweredQuestion().ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var result = condition.IsFulfilled(questionnaireTree);

            // Assert
            result.Should().BeFalse();
        }
    }
}
