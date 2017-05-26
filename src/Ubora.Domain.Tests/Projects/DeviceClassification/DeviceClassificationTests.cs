using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.DeviceClassification;
using Xunit;

namespace Ubora.Domain.Tests.Projects.DeviceClassification
{
    public class DeviceClassificationTests
    {
        [Fact]
        public void GetDefaultQuestion_Returns_Default_Main_Question()
        {
            var mainQuestion2 = new MainQuestion(Guid.NewGuid(), "mainQuestion2", null);
            var mainQuestion1 = new MainQuestion(Guid.NewGuid(), "mainQuestion1", mainQuestion2.Id);
            var expectedMainQuestion = new MainQuestion(Guid.NewGuid(), "expectedMainQuestion", mainQuestion1.Id);

            var mainQuestions = new[]
            {
                mainQuestion2,
                expectedMainQuestion,
                mainQuestion1
            };

            var deviceClassification = new TestDeviceClassification(mainQuestions.ToList(), null, null);

            // Act
            var actualResult = deviceClassification.GetDefaultMainQuestion();

            // Assert
            actualResult.Should().Be(expectedMainQuestion);
        }

        [Fact]
        public void GetNextMainQuestion_Thorws_If_Current_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetNextMainQuestion(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetNextMainQuestion_Returns_Next_Main_Question()
        {
            var mainQuestion3 = new MainQuestion(Guid.NewGuid(), "mainQuestion3", null);
            var mainQuestion2 = new MainQuestion(Guid.NewGuid(), "mainQuestion2", mainQuestion3.Id);
            var mainQuestion1 = new MainQuestion(Guid.NewGuid(), "mainQuestion1", mainQuestion2.Id);

            var mainQuestions = new[]
            {
                mainQuestion2,
                mainQuestion3,
                mainQuestion1
            };

            var deviceClassification = new TestDeviceClassification(mainQuestions.ToList(), null, null);

            // Act
            var actualResult = deviceClassification.GetNextMainQuestion(mainQuestion1.Id);

            // Assert
            actualResult.Should().Be(mainQuestion2);
        }

        [Fact]
        public void GetNextMainQuestion_Returns_Null_If_Is_Last_Main_Question()
        {
            var mainQuestion3 = new MainQuestion(Guid.NewGuid(), "mainQuestion3", null);
            var mainQuestion2 = new MainQuestion(Guid.NewGuid(), "mainQuestion2", mainQuestion3.Id);
            var mainQuestion1 = new MainQuestion(Guid.NewGuid(), "mainQuestion1", mainQuestion2.Id);

            var mainQuestions = new[]
            {
                mainQuestion2,
                mainQuestion3,
                mainQuestion1
            };

            var deviceClassification = new TestDeviceClassification(mainQuestions.ToList(), null, null);

            // Act
            var actualResult = deviceClassification.GetNextMainQuestion(mainQuestion3.Id);

            // Assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public void GetMainQuestion_Throws_If_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetMainQuestion(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetMainQuestion_Returns_Main_Question_According_To_Question_Id()
        {
            var mainQuestion3 = new MainQuestion(Guid.NewGuid(), "mainQuestion3", null);
            var mainQuestion2 = new MainQuestion(Guid.NewGuid(), "mainQuestion2", mainQuestion3.Id);
            var mainQuestion1 = new MainQuestion(Guid.NewGuid(), "mainQuestion1", mainQuestion2.Id);

            var mainQuestions = new[]
            {
                mainQuestion2,
                mainQuestion3,
                mainQuestion1
            };

            var deviceClassification = new TestDeviceClassification(mainQuestions.ToList(), null, null);

            // Act
            var actualResult = deviceClassification.GetMainQuestion(mainQuestion2.Id);

            // Assert
            actualResult.Should().Be(mainQuestion2);
        }

        [Fact]
        public void GetSubQuestions_Throws_If_ParentQuestion_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetSubQuestions(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetSubQuestions_Returns_Questions_Sub_Question_According_To_Parent_Question_Id()
        {
            var subQuestion1 = new SubQuestion(Guid.NewGuid(), "subQuestion1", Guid.Empty, Guid.Empty);
            var subQuestion2 = new SubQuestion(Guid.NewGuid(), "subQuestion2", Guid.Empty, subQuestion1.Id);

            var subQuestions = new[] {
                 subQuestion1,
                 subQuestion2
            };

            var deviceClassification = new TestDeviceClassification(null, subQuestions.ToList(), null);

            // Act
            var actualResult = deviceClassification.GetSubQuestions(subQuestion1.Id);

            // Assert
            actualResult.Should().BeEquivalentTo(new[] { subQuestion2 });
        }

        [Fact]
        public void GetSubQuestions_Returns_Null_If_No_Sub_Questions_Where_Found()
        {
            var subQuestion1 = new SubQuestion(Guid.NewGuid(), "subQuestion1", Guid.Empty, Guid.Empty);
            var subQuestion2 = new SubQuestion(Guid.NewGuid(), "subQuestion2", Guid.Empty, subQuestion1.Id);

            var subQuestions = new[] {
                 subQuestion1,
                 subQuestion2
            };

            var deviceClassification = new TestDeviceClassification(null, subQuestions.ToList(), null);

            // Act
            var actualResult = deviceClassification.GetSubQuestions(subQuestion2.Id);

            // Assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public void GetClassification_Throws_If_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetClassification(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetClassification_Should_Return_Classification_According_To_Question_Id()
        {
            var questionId = Guid.NewGuid();
            var questionIds = new[] { questionId };
            var expectedClassification = new Classification(Guid.NewGuid(), "classification1", 1, questionIds.ToList());

            var classifications = new[] { expectedClassification };

            var deviceClassification = new TestDeviceClassification(null, null, classifications.ToList());

            // Act
            var actualClassification = deviceClassification.GetClassification(questionId);

            // Assert
            actualClassification.Should().Be(expectedClassification);
        }

        [Fact]
        public void GetClassification_Should_Return_Classification_According_To_Classification_Text()
        {
            var questionId = Guid.NewGuid();
            var questionIds = new[] { questionId };
            var expectedClassification = new Classification(Guid.NewGuid(), "classification1", 1, questionIds.ToList());

            var classifications = new[] { expectedClassification };

            var deviceClassification = new TestDeviceClassification(null, null, classifications.ToList());

            // Act
            var actualClassification = deviceClassification.GetClassification("classification1");

            // Assert
            actualClassification.Should().Be(expectedClassification);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetClassification_Throws_If_Classification_Text_Is_Null_Or_Empty(string classificationText)
        {
            var deviceClassification = new TestDeviceClassification(null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetClassification(classificationText);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }
    }

    public class TestDeviceClassification : Domain.Projects.DeviceClassification.DeviceClassification
    {
        public TestDeviceClassification(List<MainQuestion> mainQuestions, List<SubQuestion> subQuestions, List<Classification> classifications) : base(mainQuestions, subQuestions, classifications)
        {

        }
    }
}
