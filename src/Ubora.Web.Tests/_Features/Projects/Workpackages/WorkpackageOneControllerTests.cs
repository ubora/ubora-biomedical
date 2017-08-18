using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;
using Ubora.Domain.Projects;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageOneControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageOneController _workpackageOneController;

        public WorkpackageOneControllerTests()
        {
            _workpackageOneController = new WorkpackageOneController()
            {
                Url = Mock.Of<IUrlHelper>()
            };
            SetUpForTest(_workpackageOneController);
        }

        [Fact]
        public void Returns_View_With_ModelState_Errors_When_Form_Post_Is_Not_Valid()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new EditStepViewModel();
            AutoMapperMock.Setup(m => m.Map<EditStepViewModel>(step))
                .Returns(expectedModel);

            _workpackageOneController.ModelState.AddModelError("", "testError");

            var postModel = new EditStepViewModel { StepId = stepId };

            // Act
            var result = (ViewResult)_workpackageOneController.Edit(postModel);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public void Returns_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new EditStepViewModel();
            AutoMapperMock.Setup(m => m.Map<EditStepViewModel>(step))
                .Returns(expectedModel);

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<ICommand>())).Returns(new CommandResult("dummyError"));

            var postModel = new EditStepViewModel { StepId = stepId };

            // Act
            var result = (ViewResult)_workpackageOneController.Edit(postModel);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }

        [Fact]
        public void Redirects_When_Command_Is_Executed_Successfully()
        {
            EditWorkpackageOneStepCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<EditWorkpackageOneStepCommand>()))
                .Callback<EditWorkpackageOneStepCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            var stepId = Guid.NewGuid().ToString();
            var postModel = new EditStepViewModel
            {
                StepId = stepId,
                Content = "expectedValue"
            };

            // Act
            var result = (RedirectToActionResult)_workpackageOneController.Edit(postModel);

            // Assert
            executedCommand.StepId.Should().Be(stepId);
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.NewValue.Should().Be("expectedValue");

            result.ActionName.Should().Be(nameof(WorkpackageOneController.Read));
        }

        [Fact]
        public void Returns_View_Without_Editing_For_Workpackage_One_Step()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new ReadStepViewModel();
            AutoMapperMock.Setup(m => m.Map<ReadStepViewModel>(step))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_workpackageOneController.Read(stepId);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }

        [Fact]
        public void Returns_View_With_Editing_For_Workpackage_One_Step()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new ReadStepViewModel();
            AutoMapperMock.Setup(m => m.Map<ReadStepViewModel>(step))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_workpackageOneController.Read(stepId);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }

        [Fact]
        public void Returns_ProjectOverview_View_With_Success_Notice_When_ProjectOverview_Was_Saved_Successfully()
        {
            var projectTitle = "projectTitle";
            UpdateProjectCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<UpdateProjectCommand>()))
                .Callback<UpdateProjectCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            var areaOfUsageTags = "AreaOfUsageTags";
            var clinicalNeedTags = "ClinicalNeedTags";
            var gmdn = "Gmdn";
            var potentialTechnologyTags = "PotentialTechnologyTags";
            var projectOverViewModel = new ProjectOverviewViewModel
            {
                AreaOfUsageTags = areaOfUsageTags,
                ClinicalNeedTags = clinicalNeedTags,
                Gmdn = gmdn,
                PotentialTechnologyTags = potentialTechnologyTags,
            };

            var project = new Project().Set(x => x.Title, projectTitle);
            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            // Act
            var result = (ViewResult)_workpackageOneController.ProjectOverview(projectOverViewModel);

            // Assert
            executedCommand.PotentialTechnologyTags.Should().Be(potentialTechnologyTags);
            executedCommand.Gmdn.Should().Be(gmdn);
            executedCommand.AreaOfUsageTags.Should().Be(areaOfUsageTags);
            executedCommand.ClinicalNeedTags.Should().Be(clinicalNeedTags);
            executedCommand.Title.Should().Be(projectTitle);

            var successNotice = _workpackageOneController.Notices.Dequeue();
            successNotice.Text.Should().Be("Project overview changed successfully!");
            successNotice.Type.Should().Be(NoticeType.Success);
        }

        [Fact]
        public void Returns_ProjectOverview_View_With_Error_Notice_When_ProjectOverview_Was_Not_Saved_Successfully()
        {
            var projectTitle = "projectTitle";
            UpdateProjectCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<UpdateProjectCommand>()))
                .Callback<UpdateProjectCommand>(c => executedCommand = c)
                .Returns(new CommandResult(new[] { "error" }));

            var areaOfUsageTags = "AreaOfUsageTags";
            var clinicalNeedTags = "ClinicalNeedTags";
            var gmdn = "Gmdn";
            var potentialTechnologyTags = "PotentialTechnologyTags";
            var projectOverViewModel = new ProjectOverviewViewModel
            {
                AreaOfUsageTags = areaOfUsageTags,
                ClinicalNeedTags = clinicalNeedTags,
                Gmdn = gmdn,
                PotentialTechnologyTags = potentialTechnologyTags,
            };

            var project = new Project().Set(x => x.Title, projectTitle);
            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            // Act
            var result = (ViewResult)_workpackageOneController.ProjectOverview(projectOverViewModel);

            // Assert
            executedCommand.PotentialTechnologyTags.Should().Be(potentialTechnologyTags);
            executedCommand.Gmdn.Should().Be(gmdn);
            executedCommand.AreaOfUsageTags.Should().Be(areaOfUsageTags);
            executedCommand.ClinicalNeedTags.Should().Be(clinicalNeedTags);
            executedCommand.Title.Should().Be(projectTitle);

            var successNotice = _workpackageOneController.Notices.Dequeue();
            successNotice.Text.Should().Be("Failed to change project overview!");
            successNotice.Type.Should().Be(NoticeType.Error);
        }
    }
}
