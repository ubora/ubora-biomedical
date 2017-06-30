using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Web._Features.Projects.DeviceClassification;
using Ubora.Web._Features.Projects.DeviceClassification.Services;
using Ubora.Web._Features.Projects.DeviceClassification.ViewModels;
using Ubora.Web.Tests.Helper;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.DeviceClassification
{
    public class DeviceClassificationControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private DeviceClassificationController _deviceClassificationController;
        private Mock<IDeviceClassificationProvider> _deviceClassificationProviderMock;
        private Mock<IDeviceClassification> _deviceClassificationMock;

        public DeviceClassificationControllerTests()
        {
            _deviceClassificationMock = new Mock<IDeviceClassification>();

            _processorMock = new Mock<ICommandQueryProcessor>();
            _deviceClassificationProviderMock = new Mock<IDeviceClassificationProvider>();
            _deviceClassificationProviderMock.Setup(x => x.Provide()).Returns(_deviceClassificationMock.Object);

            _deviceClassificationController = new DeviceClassificationController(_processorMock.Object, _deviceClassificationProviderMock.Object);
            SetProjectAndUserContext(_deviceClassificationController);
        }

        [Fact]
        public void CurrentClassification_Returns_View_With_Project_Classification()
        {
            var expectedClassificationViewModel = new CurrentClassificationViewModel
            {
                Classification = "textClassification"
            };
            var project = new Project();

            project.SetPropertyValue(nameof(project.DeviceClassification), "textClassification");
            project.SetPropertyValue(nameof(project.Id), ProjectId);

            _processorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            // Act
            var result = (ViewResult)_deviceClassificationController.CurrentClassification();

            // Assert
            var actualModel = (CurrentClassificationViewModel)result.Model;
            actualModel.ShouldBeEquivalentTo(expectedClassificationViewModel);
        }

        [Fact]
        public void GetPairedMainQuestions_Returns_Default_Paired_Questions_If_Question_Id_Is_Null()
        {
            var mainQuestion1 = new MainQuestion("");
            var mainQuestion2 = new MainQuestion("");
            var pairedMainQuestions = new PairedMainQuestions(null, mainQuestion1, mainQuestion2);

            var expectedViewModel = new PairedMainQuestionsViewModel
            {
                PairedQuestionId = pairedMainQuestions.Id,
                MainQuestionOne = pairedMainQuestions.MainQuestionOne.Text,
                MainQuestionOneId = pairedMainQuestions.MainQuestionOne.Id,
                MainQuestionTwo = pairedMainQuestions.MainQuestionTwo.Text,
                MainQuestionTwoId = pairedMainQuestions.MainQuestionTwo.Id,
                Notes = new List<string>()
            };

            _deviceClassificationMock.Setup(x => x.GetDefaultPairedMainQuestion())
                .Returns(pairedMainQuestions);

            // Act
            var result = (ViewResult)_deviceClassificationController.GetPairedMainQuestions(null);

            // Assert
            result.Model.ShouldBeEquivalentTo(expectedViewModel);
        }

        [Fact]
        public void GetPairedMainQuestions_Returns_Paired_Questions_If_Question_Id_Is_Not_Null()
        {
            var mainQuestion1 = new MainQuestion("");
            var mainQuestion2 = new MainQuestion("");
            var pairedMainQuestions = new PairedMainQuestions(null, mainQuestion1, mainQuestion2);

            var expectedViewModel = new PairedMainQuestionsViewModel
            {
                PairedQuestionId = pairedMainQuestions.Id,
                MainQuestionOne = pairedMainQuestions.MainQuestionOne.Text,
                MainQuestionOneId = pairedMainQuestions.MainQuestionOne.Id,
                MainQuestionTwo = pairedMainQuestions.MainQuestionTwo.Text,
                MainQuestionTwoId = pairedMainQuestions.MainQuestionTwo.Id,
                Notes = new List<string>()
            };

            _deviceClassificationMock.Setup(x => x.GetPairedMainQuestions(pairedMainQuestions.Id))
                .Returns(pairedMainQuestions);

            // Act
            var result = (ViewResult)_deviceClassificationController.GetPairedMainQuestions(pairedMainQuestions.Id);

            // Assert
            result.Model.ShouldBeEquivalentTo(expectedViewModel);
        }

        [Fact]
        public void GetQuestions_Returns_BadRequest_If_ParentQuestion_Id_Is_Empty()
        {
            // Act
            var result = _deviceClassificationController.GetQuestions(Guid.Empty, null);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void GetQuestions_Returns_BadRequest_If_There_Are_No_Sub_Questions()
        {
            var parentQuestionId = Guid.NewGuid();
            _deviceClassificationMock.Setup(x => x.GetSubQuestions(parentQuestionId))
                .Returns((IReadOnlyCollection<SubQuestion>)null);

            // Act
            var result = _deviceClassificationController.GetQuestions(parentQuestionId, null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GetQuestions_Returns_Result_If_Sub_Questions_Exist()
        {
            var mainQuestion1 = new MainQuestion("mainQuestion1");
            var mainQuestion2 = new MainQuestion("mainQuestion2");
            var pairedMainQuestions = new PairedMainQuestions(null, mainQuestion1, mainQuestion2);
            var subQuestion1 = new SubQuestion("subQuestion1", pairedMainQuestions, mainQuestion1);
            var subQuestion2 = new SubQuestion("subQuestion2", pairedMainQuestions, mainQuestion1);

            var subQuestions = new List<SubQuestion>
            {
                subQuestion1,
                subQuestion2
            };

            var expectedViewModel = new QuestionsViewModel
            {
                Questions = subQuestions,
                PairedMainQuestionsId = pairedMainQuestions.Id,
                Notes = new List<string>()
            };

            _deviceClassificationMock.Setup(x => x.GetSubQuestions(mainQuestion1.Id))
                .Returns(subQuestions);

            // Act
            var result = (ViewResult)_deviceClassificationController.GetQuestions(mainQuestion1.Id, null);

            // Assert
            result.Model.ShouldBeEquivalentTo(expectedViewModel);
        }

        [Fact]
        public void MainQuestionAnswer_Redirects_To_GetQuestions_If_There_Are_Sub_Questions()
        {
            var mainQuestion1 = new MainQuestion("mainQuestion1");
            var mainQuestion2 = new MainQuestion("mainQuestion2");
            var pairedMainQuestions = new PairedMainQuestions(null, mainQuestion1, mainQuestion2);
            var subQuestion1 = new SubQuestion("subQuestion1", pairedMainQuestions, mainQuestion1);
            var subQuestion2 = new SubQuestion("subQuestion2", pairedMainQuestions, mainQuestion1);

            var subQuestions = new List<SubQuestion>
            {
                subQuestion1,
                subQuestion2
            };

            _deviceClassificationMock.Setup(x => x.GetSubQuestions(mainQuestion1.Id))
                .Returns(subQuestions);

            var mainQuestionAnswerViewModel = new MainQuestionAnswerViewModel { MainQuestionId = mainQuestion1.Id };

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.MainQuestionAnswer(mainQuestionAnswerViewModel);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.GetQuestions));
        }

        [Fact]
        public void MainQuestionAnswer_Redirects_To_NextMainQuestion_If_There_Are_No_Sub_Questions()
        {
            var mainQuestionId = Guid.NewGuid();

            _deviceClassificationMock.Setup(x => x.GetSubQuestions(mainQuestionId))
                .Returns((IReadOnlyCollection<SubQuestion>)null);

            var mainQuestionAnswerViewModel = new MainQuestionAnswerViewModel { MainQuestionId = mainQuestionId };

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.MainQuestionAnswer(mainQuestionAnswerViewModel);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.NextMainQuestion));
        }

        [Fact]
        public void Answer_Redirects_To_GetQuestions_If_There_Are_Sub_Questions()
        {
            var nextQuestionId = Guid.NewGuid();
            var subQuestions = new List<SubQuestion>();
            _deviceClassificationMock.Setup(x => x.GetSubQuestions(nextQuestionId))
                .Returns(subQuestions);

            var answerViewModel = new AnswerViewModel { NextQuestionId = nextQuestionId };

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.Answer(answerViewModel);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.GetQuestions));
        }

        [Fact]
        public void Answer_Redirects_To_NextMainQuestion_And_Sets_Project_Classification_If_There_Are_No_Sub_Questions()
        {
            var nextQuestionId = Guid.NewGuid();

            _deviceClassificationMock.Setup(x => x.GetSubQuestions(nextQuestionId))
                .Returns((IReadOnlyCollection<SubQuestion>)null);

            SetDeviceClassificationForProjectCommand executedCommand = null;
            _processorMock
                .Setup(x => x.Execute(It.IsAny<SetDeviceClassificationForProjectCommand>()))
                .Callback<SetDeviceClassificationForProjectCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            var classification = new Classification("classification", 1, null);
            _deviceClassificationMock.Setup(x => x.GetClassification(nextQuestionId))
                .Returns(classification);

            var answerViewModel = new AnswerViewModel { NextQuestionId = nextQuestionId };

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.Answer(answerViewModel);

            // Assert
            executedCommand.DeviceClassification.Should().Be(classification);
            result.ActionName.Should().Be(nameof(DeviceClassificationController.NextMainQuestion));
        }

        [Fact]
        public void SpecialQuestionAnswer_Redirects_To_GetSpecialSubQuestions_If_There_Are_Special_Sub_Questions()
        {
            var nextQuestionId = Guid.NewGuid();
            var subQuestions = new List<SpecialSubQuestion>();
            _deviceClassificationMock.Setup(x => x.GetSpecialSubQuestions(nextQuestionId))
                .Returns(subQuestions);

            var answerViewModel = new SpecialAnswerViewModel { NextQuestionId = nextQuestionId };

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.SpecialQuestionAnswer(answerViewModel);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.GetSpecialSubQuestions));
        }

        [Fact]
        public void SpecialQuestionAnswer_Redirects_To_NextSpecialMainQuestion_And_Sets_Project_Classification_If_There_Are_No_Special_Sub_Questions()
        {
            var nextQuestionId = Guid.NewGuid();

            _deviceClassificationMock.Setup(x => x.GetSpecialSubQuestions(nextQuestionId))
                .Returns((IReadOnlyCollection<SpecialSubQuestion>)null);

            SetDeviceClassificationForProjectCommand executedCommand = null;
            _processorMock
                .Setup(x => x.Execute(It.IsAny<SetDeviceClassificationForProjectCommand>()))
                .Callback<SetDeviceClassificationForProjectCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            var classification = new Classification("classification", 1, null);
            _deviceClassificationMock.Setup(x => x.GetClassification(nextQuestionId))
                .Returns(classification);

            var answerViewModel = new SpecialAnswerViewModel { NextQuestionId = nextQuestionId };

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.SpecialQuestionAnswer(answerViewModel);

            // Assert
            executedCommand.DeviceClassification.Should().Be(classification);
            result.ActionName.Should().Be(nameof(DeviceClassificationController.NextSpecialMainQuestion));
        }

        [Fact]
        public void NextMainQuestion_Returns_BadRequest_If_Main_Question_Is_Empty()
        {
            // Act
            var result = _deviceClassificationController.NextMainQuestion(Guid.Empty);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void NextMainQuestion_Redirects_To_GetPairedMainQuestions_If_Next_Paired_Main_Questions_Are_Not_Null()
        {
            var pairedMainQuestionsId = Guid.NewGuid();

            var pairedMainQuestions = new PairedMainQuestions(null, null, null);

            _deviceClassificationMock.Setup(x => x.GetNextPairedMainQuestions(pairedMainQuestionsId))
                .Returns(pairedMainQuestions);

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.NextMainQuestion(pairedMainQuestionsId);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.GetPairedMainQuestions));
        }

        [Fact]
        public void NextMainQuestion_Redirects_To_GetSpecialMainQuestion_If_Next_Paired_Main_Questions_Are_Null()
        {
            var pairedMainQuestionsId = Guid.NewGuid();

            _deviceClassificationMock.Setup(x => x.GetNextPairedMainQuestions(pairedMainQuestionsId))
                .Returns((PairedMainQuestions)null);

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.NextMainQuestion(pairedMainQuestionsId);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.GetSpecialMainQuestion));
        }

        [Fact]
        public void GetSpecialMainQuestion_Returns_Default_Special_Main_Question_If_Question_Id_Is_Null()
        {
            var specialMainQuestion = new SpecialMainQuestion("specialMainQuestion", null);

            var expectedViewModel = new SpecialMainQuestionViewModel
            {
                CurrentSpecialMainQuestionId = specialMainQuestion.Id,
                QuestionText = specialMainQuestion.Text
            };

            _deviceClassificationMock.Setup(x => x.GetDefaultSpecialMainQuestion())
                .Returns(specialMainQuestion);

            // Act
            var result = (ViewResult)_deviceClassificationController.GetSpecialMainQuestion(null);

            // Assert
            result.Model.ShouldBeEquivalentTo(expectedViewModel);
        }

        [Fact]
        public void GetSpecialMainQuestion_Returns_Special_Main_Question_If_Question_Id_Is_Not_Null()
        {
            var specialMainQuestion = new SpecialMainQuestion("specialMainQuestion", null);

            var expectedViewModel = new SpecialMainQuestionViewModel
            {
                CurrentSpecialMainQuestionId = specialMainQuestion.Id,
                QuestionText = specialMainQuestion.Text
            };

            _deviceClassificationMock.Setup(x => x.GetSpecialMainQuestion(specialMainQuestion.Id))
                .Returns(specialMainQuestion);

            // Act
            var result = (ViewResult)_deviceClassificationController.GetSpecialMainQuestion(specialMainQuestion.Id);

            // Assert
            result.Model.ShouldBeEquivalentTo(expectedViewModel);
        }

        [Fact]
        public void NextSpecialMainQuestion_Returns_BadRequest_If_Main_Question_Is_Empty()
        {
            // Act
            var result = _deviceClassificationController.NextSpecialMainQuestion(Guid.Empty);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void NextSpecialMainQuestion_Redirects_To_GetSpecialMainQuestions_If_Next_Special_Main_Questions_Is_Not_Null()
        {
            var currentSpecialMainQuestionId = Guid.NewGuid();

            var specialMainQuestion = new SpecialMainQuestion("specialMainQuestion", null);

            _deviceClassificationMock.Setup(x => x.GetNextSpecialMainQuestion(currentSpecialMainQuestionId))
                .Returns(specialMainQuestion);

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.NextSpecialMainQuestion(currentSpecialMainQuestionId);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.GetSpecialMainQuestion));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NextSpecialMainQuestion_Redirects_To_NoClass_If_Next_Special_Main_Questions_Is_Null_And_Project_Classification_Is_Null_Or_Empty(string classification)
        {
            var pairedMainQuestionsId = Guid.NewGuid();

            _deviceClassificationMock.Setup(x => x.GetNextPairedMainQuestions(pairedMainQuestionsId))
                .Returns((PairedMainQuestions)null);

            var project = new Project();

            project.SetPropertyValue(nameof(project.DeviceClassification), classification);
            project.SetPropertyValue(nameof(project.Id), ProjectId);

            _processorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.NextSpecialMainQuestion(pairedMainQuestionsId);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.NoClass));
        }

        [Fact]
        public void NextSpecialMainQuestion_Redirects_To_CurrentClassification_If_Next_Special_Main_Questions_Is_Null_And_Project_Classification_Is_Set()
        {
            var pairedMainQuestionsId = Guid.NewGuid();

            _deviceClassificationMock.Setup(x => x.GetNextPairedMainQuestions(pairedMainQuestionsId))
                .Returns((PairedMainQuestions)null);

            var project = new Project();

            project.SetPropertyValue(nameof(project.DeviceClassification), "deviceClassification");
            project.SetPropertyValue(nameof(project.Id), ProjectId);

            _processorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            // Act
            var result = (RedirectToActionResult)_deviceClassificationController.NextSpecialMainQuestion(pairedMainQuestionsId);

            // Assert
            result.ActionName.Should().Be(nameof(DeviceClassificationController.CurrentClassification));
        }

        [Fact]
        public void GetSpecialSubQuestions_Returns_BadRequest_If_Main_Question_Id_Is_Empty()
        {
            // Act
            var result = _deviceClassificationController.GetSpecialSubQuestions(Guid.Empty);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void GetSpecialSubQuestions_Returns_BadRequest_If_There_Are_No_Sub_Questions()
        {
            var parentQuestionId = Guid.NewGuid();
            _deviceClassificationMock.Setup(x => x.GetSpecialSubQuestions(parentQuestionId))
                .Returns((IReadOnlyCollection<SpecialSubQuestion>)null);

            // Act
            var result = _deviceClassificationController.GetSpecialSubQuestions(parentQuestionId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GetSpecialSubQuestions_Returns_Result_If_Special_Sub_Questions_Exist()
        {
            var subQuestion1 = new SpecialSubQuestion("subQuestion1", null);
            var subQuestion2 = new SpecialSubQuestion("subQuestion2", null);

            var subQuestions = new List<SpecialSubQuestion>
            {
                subQuestion1,
                subQuestion2
            };

            var mainQuestionId = Guid.NewGuid();

            _deviceClassificationMock.Setup(x => x.GetSpecialSubQuestions(mainQuestionId))
                .Returns(subQuestions);

            var expectedViewModel = new SpecialSubQuestionsViewModel
            {
                Questions = subQuestions,
                MainQuestionId = mainQuestionId,
                Notes = new List<string>()
            };

            // Act
            var result = (ViewResult)_deviceClassificationController.GetSpecialSubQuestions(mainQuestionId);

            // Assert
            result.Model.ShouldBeEquivalentTo(expectedViewModel);
        }
    }
}
