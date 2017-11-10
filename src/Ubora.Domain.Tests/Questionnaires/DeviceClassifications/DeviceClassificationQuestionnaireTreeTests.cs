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

            var conditions = new[]
            {
                new ChosenAnswerDeviceClassCondition("q2", "a2")
            };
            var deviceClasses = new []
            {
                new DeviceClassOne(conditions)
            };

            // Act
            Action act = () => new DeviceClassificationQuestionnaireTree(questions, deviceClasses);

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

            var conditions = new[]
            {
                new ChosenAnswerDeviceClassCondition("q1", "not_exist")
            };
            var deviceClasses = new[]
            {
                new DeviceClassOne(conditions)
            };


            // Act
            Action act = () => new DeviceClassificationQuestionnaireTree(questions, deviceClasses);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("answer not found");
        }
    }
}
