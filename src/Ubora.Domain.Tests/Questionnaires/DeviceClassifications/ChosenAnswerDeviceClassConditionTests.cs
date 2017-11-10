using System;
using System.Collections.Generic;
using FluentAssertions;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications
{
    public class ChosenAnswerDeviceClassConditionTests
    {
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

            var condition = new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
            {
                { "q1", "y" },
                { "q2", "n" }
            });
            var deviceClass = new DeviceClassOne(new [] { condition });

            var questionnaireTree = new DeviceClassificationQuestionnaireTree(questions, new [] { deviceClass });

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

            var condition = new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
            {
                { "q1", "y" },
                { "q2", "n" }
            });
            var deviceClass = new DeviceClassOne(new[] { condition });

            var questionnaireTree = new DeviceClassificationQuestionnaireTree(questions, new[] { deviceClass });

            questionnaireTree.FindNextUnansweredQuestion().ChooseAnswer("n", DateTime.UtcNow);
            questionnaireTree.FindNextUnansweredQuestion().ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var result = condition.IsFulfilled(questionnaireTree);

            // Assert
            result.Should().BeFalse();
        }
    }
}
