using System;
using System.Collections.Generic;
using FluentAssertions;
using Ubora.Domain.Questionnaires.DeviceClassifications;
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
                new ChosenAnswerDeviceClassCondition("q2", "a2", DeviceClass.One)
            };

            // Act
            Action act = () => new DeviceClassificationQuestionnaireTree(questions, conditions);

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
                new ChosenAnswerDeviceClassCondition("q1", "not_exist", DeviceClass.One)
            };

            // Act
            Action act = () => new DeviceClassificationQuestionnaireTree(questions, conditions);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("answer not found");
        }

        [Fact]
        public void Constructor_Throws_When_Condition_Ids_Are_Duplicated()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            var conditions = new[]
            {
                new ChosenAnswerDeviceClassCondition("duplicate_id", new Dictionary<string, string> { { "q1", "n" } }, DeviceClass.One),
                new ChosenAnswerDeviceClassCondition("duplicate_id", new Dictionary<string, string> { { "q1", "y" } }, DeviceClass.TwoA)
            };

            // Act
            Action act = () => new DeviceClassificationQuestionnaireTree(questions, conditions);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().ContainEquivalentOf("duplicate");
        }
    }
}
