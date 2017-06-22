﻿using FluentAssertions;
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
        public void GetDefaultPairedMainQuestion_Returns_Default_Paired_Main_Question()
        {
            var mainQuestion4 = new MainQuestion("mainQuestion4");
            var mainQuestion3 = new MainQuestion("mainQuestion3");
            var mainQuestion2 = new MainQuestion("mainQuestion2");
            var mainQuestion1 = new MainQuestion("mainQuestion1");

            var pairedMainQuestions = new PairedMainQuestions(null, mainQuestion3, mainQuestion4);
            var expectedPairedMainQuestions = new PairedMainQuestions(pairedMainQuestions, mainQuestion1, mainQuestion2);

            var pairedMainQuestionsList = new List<PairedMainQuestions>
            {
                pairedMainQuestions,
                expectedPairedMainQuestions
            };

            var deviceClassification = new TestDeviceClassification(pairedMainQuestionsList, null, null, null, null);

            // Act
            var actualResult = deviceClassification.GetDefaultPairedMainQuestion();

            // Assert
            actualResult.Should().Be(expectedPairedMainQuestions);
        }

        [Fact]
        public void GetNextPairedMainQuestions_Throws_If_Current_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetNextPairedMainQuestions(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetNextPairedMainQuestions_Returns_Next_Paired_Main_Questions()
        {
            var mainQuestion4 = new MainQuestion("mainQuestion4");
            var mainQuestion3 = new MainQuestion("mainQuestion3");
            var mainQuestion2 = new MainQuestion("mainQuestion2");
            var mainQuestion1 = new MainQuestion("mainQuestion1");

            var expectedPairedMainQuestions = new PairedMainQuestions(null, mainQuestion3, mainQuestion4);
            var pairedMainQuestions = new PairedMainQuestions(expectedPairedMainQuestions, mainQuestion1, mainQuestion2);

            var pairedMainQuestionsList = new List<PairedMainQuestions>
            {
                pairedMainQuestions,
                expectedPairedMainQuestions
            };

            var deviceClassification = new TestDeviceClassification(pairedMainQuestionsList, null, null, null, null);

            // Act
            var actualResult = deviceClassification.GetNextPairedMainQuestions(pairedMainQuestions.Id);

            // Assert
            actualResult.Should().Be(expectedPairedMainQuestions);
        }

        [Fact]
        public void GetNextPairedMainQuestions_Returns_Null_If_Is_Last_Paired_Main_Question()
        {
            var mainQuestion4 = new MainQuestion("mainQuestion4");
            var mainQuestion3 = new MainQuestion("mainQuestion3");
            var mainQuestion2 = new MainQuestion("mainQuestion2");
            var mainQuestion1 = new MainQuestion("mainQuestion1");

            var pairedMainQuestions2 = new PairedMainQuestions(null, mainQuestion3, mainQuestion4);
            var pairedMainQuestions1 = new PairedMainQuestions(pairedMainQuestions2, mainQuestion1, mainQuestion2);

            var pairedMainQuestionsList = new List<PairedMainQuestions>
            {
                pairedMainQuestions1,
                pairedMainQuestions2
            };

            var deviceClassification = new TestDeviceClassification(pairedMainQuestionsList, null, null, null, null);

            // Act
            var actualResult = deviceClassification.GetNextPairedMainQuestions(pairedMainQuestions2.Id);

            // Assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public void GetPairedMainQuestion_Throws_If_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetPairedMainQuestions(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetPairedMainQuestion_Returns_Paired_Main_Question_According_To_Question_Id()
        {
            var mainQuestion4 = new MainQuestion("mainQuestion4");
            var mainQuestion3 = new MainQuestion("mainQuestion3");
            var mainQuestion2 = new MainQuestion("mainQuestion2");
            var mainQuestion1 = new MainQuestion("mainQuestion1");

            var pairedMainQuestions = new PairedMainQuestions(null, mainQuestion3, mainQuestion4);
            var expectedPairedMainQuestions = new PairedMainQuestions(pairedMainQuestions, mainQuestion1, mainQuestion2);

            var pairedMainQuestionsList = new List<PairedMainQuestions>
            {
                pairedMainQuestions,
                expectedPairedMainQuestions
            };

            var deviceClassification = new TestDeviceClassification(pairedMainQuestionsList, null, null, null, null);

            // Act
            var actualResult = deviceClassification.GetPairedMainQuestions(expectedPairedMainQuestions.Id);

            // Assert
            actualResult.Should().Be(expectedPairedMainQuestions);
        }

        [Fact]
        public void GetSubQuestions_Throws_If_ParentQuestion_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetSubQuestions(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetSubQuestions_Returns_Questions_Sub_Question_According_To_Parent_Question_Id()
        {
            var subQuestion1 = new SubQuestion("subQuestion1", null, null);
            var subQuestion2 = new SubQuestion("subQuestion2", null, subQuestion1);

            var subQuestions = new List<SubQuestion> {
                 subQuestion1,
                 subQuestion2
            };

            var deviceClassification = new TestDeviceClassification(null, subQuestions, null, null, null);

            // Act
            var actualResult = deviceClassification.GetSubQuestions(subQuestion1.Id);

            // Assert
            actualResult.Should().BeEquivalentTo(new[] { subQuestion2 });
        }

        [Fact]
        public void GetSubQuestions_Returns_Null_If_No_Sub_Questions_Where_Found()
        {
            var subQuestion1 = new SubQuestion("subQuestion1", null, null);
            var subQuestion2 = new SubQuestion("subQuestion2", null, subQuestion1);

            var subQuestions = new List<SubQuestion> {
                 subQuestion1,
                 subQuestion2
            };

            var deviceClassification = new TestDeviceClassification(null, subQuestions, null, null, null);

            // Act
            var actualResult = deviceClassification.GetSubQuestions(subQuestion2.Id);

            // Assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public void GetClassification_Throws_If_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

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
            var expectedClassification = new Classification("classification1", 1, questionIds.ToList());

            var classifications = new List<Classification> { expectedClassification };

            var deviceClassification = new TestDeviceClassification(null, null, null, null, classifications);

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
            var expectedClassification = new Classification("classification1", 1, questionIds.ToList());

            var classifications = new List<Classification> { expectedClassification };

            var deviceClassification = new TestDeviceClassification(null, null, null, null, classifications);

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
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetClassification(classificationText);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetDefaultSpecialMainQuestion_Returns_Default_Special_Main_Question()
        {
            var specialQuestion2 = new SpecialMainQuestion("", null);
            var specialQuestion1 = new SpecialMainQuestion("", specialQuestion2);
            var specialQuestions = new List<SpecialMainQuestion> {
                specialQuestion1,
                specialQuestion2
            };

            var deviceClassification = new TestDeviceClassification(null, null, specialQuestions, null, null);

            // Act
            var defaultSpecialMainQuestion = deviceClassification.GetDefaultSpecialMainQuestion();

            // Assert
            defaultSpecialMainQuestion.Should().Be(specialQuestion1);
        }

        [Fact]
        public void GetNextSpecialMainQuestion_Throws_If_Current_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetNextSpecialMainQuestion(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetNextSpecialMainQuestion_Returns_Next_Special_Main_Question()
        {
            var expectedMainQuestion = new SpecialMainQuestion("expectedMainQuestion", null);
            var mainQuestion = new SpecialMainQuestion("mainQuestion", expectedMainQuestion);

            var specialMainQuestions = new List<SpecialMainQuestion>
            {
                mainQuestion,
                expectedMainQuestion
            };

            var deviceClassification = new TestDeviceClassification(null, null, specialMainQuestions, null, null);

            // Act
            var actualResult = deviceClassification.GetNextSpecialMainQuestion(mainQuestion.Id);

            // Assert
            actualResult.Should().Be(expectedMainQuestion);
        }

        [Fact]
        public void GetNextSpecialMainQuestion_Returns_Null_If_Is_Last_Special_Main_Question()
        {
            var mainQuestion2 = new SpecialMainQuestion("mainQuestion2", null);
            var mainQuestion1 = new SpecialMainQuestion("mainQuestion1", mainQuestion2);

            var specialMainQuestions = new List<SpecialMainQuestion>
            {
                mainQuestion2,
                mainQuestion1
            };

            var deviceClassification = new TestDeviceClassification(null, null, specialMainQuestions, null, null);

            // Act
            var actualResult = deviceClassification.GetNextSpecialMainQuestion(mainQuestion2.Id);

            // Assert
            actualResult.Should().BeNull();
        }

        [Fact]
        public void GetSpecialMainQuestion_Throws_If_Question_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetSpecialMainQuestion(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetSpecialMainQuestion_Returns_Special_Main_Question_According_To_Question_Id()
        {
            var mainQuestion2 = new SpecialMainQuestion("mainQuestion2", null);
            var expectedMainQuestion = new SpecialMainQuestion("mainQuestion1", mainQuestion2);

            var specialMainQuestions = new List<SpecialMainQuestion>
            {
                mainQuestion2,
                expectedMainQuestion
            };

            var deviceClassification = new TestDeviceClassification(null, null, specialMainQuestions, null, null);

            // Act
            var actualResult = deviceClassification.GetSpecialMainQuestion(expectedMainQuestion.Id);

            // Assert
            actualResult.Should().Be(expectedMainQuestion);
        }

        [Fact]
        public void GetSpecialSubQuestions_Throws_If_ParentQuestion_Id_Is_Empty()
        {
            var deviceClassification = new TestDeviceClassification(null, null, null, null, null);

            // Act
            Action testAction = () => deviceClassification.GetSpecialSubQuestions(Guid.Empty);

            // Assert
            Assert.Throws<ArgumentException>(testAction);
        }

        [Fact]
        public void GetSpecialSubQuestions_Returns_Questions_Sub_Question_According_To_Parent_Question_Id()
        {
            var mainQuestion1 = new SpecialMainQuestion("mainQuestion", null);
            var mainQuestion2 = new SpecialMainQuestion("mainQuestion", null);

            var subQuestion1 = new SpecialSubQuestion("subQuestion1", mainQuestion1);
            var subQuestion2 = new SpecialSubQuestion("subQuestion2", mainQuestion1);
            var subQuestion3 = new SpecialSubQuestion("subQuestion2", mainQuestion2);

            var specialSubQuestions = new List<SpecialSubQuestion>
            {
                subQuestion1,
                subQuestion2,
                subQuestion3
            };

            var deviceClassification = new TestDeviceClassification(null, null, null, specialSubQuestions, null);

            // Act
            var actualResult = deviceClassification.GetSpecialSubQuestions(mainQuestion1.Id);

            // Assert
            actualResult.Should().BeEquivalentTo(new[] { subQuestion1, subQuestion2 });
        }

        [Fact]
        public void GetSpecialSubQuestions_Returns_Null_If_No_Sub_Questions_Where_Found()
        {
            var mainQuestion1 = new SpecialMainQuestion("mainQuestion", null);
            var mainQuestion2 = new SpecialMainQuestion("mainQuestion", null);

            var subQuestion1 = new SpecialSubQuestion("subQuestion1", mainQuestion2);
            var subQuestion2 = new SpecialSubQuestion("subQuestion2", mainQuestion2);
            var subQuestion3 = new SpecialSubQuestion("subQuestion2", mainQuestion2);

            var specialSubQuestions = new List<SpecialSubQuestion>
            {
                subQuestion1,
                subQuestion2,
                subQuestion3
            };

            var deviceClassification = new TestDeviceClassification(null, null, null, specialSubQuestions, null);

            // Act
            var actualResult = deviceClassification.GetSpecialSubQuestions(mainQuestion1.Id);

            // Assert
            actualResult.Should().BeNull();
        }
    }

    public class TestDeviceClassification : Domain.Projects.DeviceClassification.DeviceClassification
    {
        public TestDeviceClassification(
            List<PairedMainQuestions> pairedMainQuestions,
            List<SubQuestion> subQuestions,
            List<SpecialMainQuestion> specialMainQuestions,
            List<SpecialSubQuestion> specialSubQuestions,
            List<Classification> classifications) :
            base(
                pairedMainQuestions,
                subQuestions,
                specialMainQuestions,
                specialSubQuestions,
                classifications)
        {

        }
    }
}
