using System;
using FluentAssertions;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTreeTests
    {
        [Fact]
        public void Constructor_Throws_When_Question_Of_Device_Class_Condition_Does_Not_Exist()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", "q1_1"),
                })
            };

            var deviceClassesWithConditions = new []
            {
                new DeviceClassOne().WithConditions(new ChosenAnswerDeviceClassCondition("q2", "a2"))
            };

            // Act
            Action act = () => new DeviceClassificationQuestionnaireTree(questions, deviceClassesWithConditions);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("question not found");
        }

        [Fact]
        public void Constructor_Throws_When_Answer_Of_Device_Class_Condition_Does_Not_Exist()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", null),
                })
            };

            var deviceClassesWithConditions = new[]
            {
                new DeviceClassOne().WithConditions(new ChosenAnswerDeviceClassCondition("q1", "not_exist"))
            };

            // Act
            Action act = () => new DeviceClassificationQuestionnaireTree(questions, deviceClassesWithConditions);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("answer not found");
        }

        [Fact]
        public void GetHighestRiskDeviceClass_Returns_Empty_DeviceClass_When_No_Condition_Is_Satisifed()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            var notSatisfiedCondition = new ChosenAnswerDeviceClassCondition("q1", "n");

            var deviceClassesWithConditions = new[]
            {
                new DeviceClassOne().WithConditions(notSatisfiedCondition)
            };

            var questionnaireTree = new DeviceClassificationQuestionnaireTree(questions, deviceClassesWithConditions);

            // Act
            var result = questionnaireTree.GetHighestRiskDeviceClass();

            // Assert
            result.Should().Be(DeviceClass.None);
        }

        [Fact]
        public void GetHighestRiskDeviceClass_Returns_Highest_Risk_Device_Class_Which_Has_Satisfied_Condition()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            var satisfiedCondition = new ChosenAnswerDeviceClassCondition("q1", "y");
            var notSatisfiedCondition = new ChosenAnswerDeviceClassCondition("q1", "n");

            var deviceClassesWithConditions = new[]
            {
                DeviceClass.One.WithConditions(satisfiedCondition),
                DeviceClass.TwoB.WithConditions(satisfiedCondition), // expected
                DeviceClass.Three.WithConditions(notSatisfiedCondition),
            };

            var questionnaireTree = new DeviceClassificationQuestionnaireTree(questions, deviceClassesWithConditions);
            questionnaireTree.FindNextUnansweredQuestion().ChooseAnswer("y", DateTime.UtcNow);

            // Act
            var result = questionnaireTree.GetHighestRiskDeviceClass();

            // Assert
            result.Should().Be(DeviceClass.TwoB);
        }
    }
}
