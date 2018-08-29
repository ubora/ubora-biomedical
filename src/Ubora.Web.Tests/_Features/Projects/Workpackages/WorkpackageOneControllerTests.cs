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
using Ubora.Domain.Projects._Commands;
using Ubora.Web._Features._Shared.Notices;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task Returns_View_With_ModelState_Errors_When_Form_Post_Is_Not_Valid()
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
            var result = (ViewResult)await _workpackageOneController.Edit(postModel);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task Returns_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new EditStepViewModel();
            AutoMapperMock.Setup(m => m.Map<EditStepViewModel>(step))
                .Returns(expectedModel);

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<ICommand>()))
                .Returns(CommandResult.Failed("dummyError"));

            var postModel = new EditStepViewModel { StepId = stepId, ContentQuillDelta = "{test}" };

            // Act
            var result = (ViewResult)await _workpackageOneController.Edit(postModel);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }

        [Fact]
        public async Task Redirects_When_Command_Is_Executed_Successfully()
        {
            EditWorkpackageOneStepCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<EditWorkpackageOneStepCommand>()))
                .Callback<EditWorkpackageOneStepCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var stepId = Guid.NewGuid().ToString();
            var postModel = new EditStepViewModel
            {
                StepId = stepId,
                ContentQuillDelta = "{expectedValue}"
            };

            // Act
            var result = (RedirectToActionResult)await _workpackageOneController.Edit(postModel);

            // Assert
            executedCommand.StepId.Should().Be(stepId);
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.NewValue.Value.Should().Be("{expectedValue}");

            result.ActionName.Should().Be(nameof(WorkpackageOneController.Read));
        }

        [Fact]
        public async Task Returns_View_Without_Editing_For_Workpackage_One_Step()
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
            var result = (ViewResult)await _workpackageOneController.Read(stepId);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }

        [Fact]
        public async Task Returns_View_With_Editing_For_Workpackage_One_Step()
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
            var result = (ViewResult)await _workpackageOneController.Read(stepId);

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
                .Returns(CommandResult.Success);

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
            successNotice.Text.Should().Be(SuccessTexts.ProjectUpdated);
            successNotice.Type.Should().Be(NoticeType.Success);
        }

        [Fact]
        public void Redirects_To_ReturnUrl_With_Success_Notice_When_ProjectOverview_Was_Saved_Successfully_And_Has_ReturnUrl()
        {
            var projectTitle = "projectTitle";
            UpdateProjectCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<UpdateProjectCommand>()))
                .Callback<UpdateProjectCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

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

            var returnUrl = "returnUrl";

            // Act
            var result = (RedirectToActionResult)_workpackageOneController.ProjectOverview(projectOverViewModel, returnUrl);

            // Assert
            executedCommand.PotentialTechnologyTags.Should().Be(potentialTechnologyTags);
            executedCommand.Gmdn.Should().Be(gmdn);
            executedCommand.AreaOfUsageTags.Should().Be(areaOfUsageTags);
            executedCommand.ClinicalNeedTags.Should().Be(clinicalNeedTags);
            executedCommand.Title.Should().Be(projectTitle);

            var successNotice = _workpackageOneController.Notices.Dequeue();
            successNotice.Text.Should().Be(SuccessTexts.ProjectUpdated);
            successNotice.Type.Should().Be(NoticeType.Success);

            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");
        }

        [Fact]
        public void Returns_ProjectOverview_View_With_Error_Notice_When_ProjectOverview_Was_Not_Saved_Successfully()
        {
            var projectTitle = "projectTitle";
            UpdateProjectCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<UpdateProjectCommand>()))
                .Callback<UpdateProjectCommand>(c => executedCommand = c)
                .Returns(CommandResult.Failed("error"));

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

        [Fact]
        public void Returns_ProjectOverview_View_With_Error_Notice_And_ReturnUrl_When_ProjectOverview_Was_Not_Saved_Successfully_And_ReturnUrl_Is_Not_Null()
        {
            var projectTitle = "projectTitle";
            UpdateProjectCommand executedCommand = null;
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<UpdateProjectCommand>()))
                .Callback<UpdateProjectCommand>(c => executedCommand = c)
                .Returns(CommandResult.Failed("error"));

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

            var returnUrl = "returnUrl";

            // Act
            var result = (ViewResult)_workpackageOneController.ProjectOverview(projectOverViewModel, returnUrl);

            // Assert
            executedCommand.PotentialTechnologyTags.Should().Be(potentialTechnologyTags);
            executedCommand.Gmdn.Should().Be(gmdn);
            executedCommand.AreaOfUsageTags.Should().Be(areaOfUsageTags);
            executedCommand.ClinicalNeedTags.Should().Be(clinicalNeedTags);
            executedCommand.Title.Should().Be(projectTitle);

            var successNotice = _workpackageOneController.Notices.Dequeue();
            successNotice.Text.Should().Be("Failed to change project overview!");
            successNotice.Type.Should().Be(NoticeType.Error);

            result.ViewData.Values.Last().Should().Be(returnUrl);
        }

        [Fact]
        public void DiscardDesignPlanningChanges_Redirects_To_ProjectOverview_When_ReturnUrl_Is_Null()
        {
            // Act
            var result = (RedirectToActionResult)_workpackageOneController.DiscardDesignPlanningChanges();

            // Assert
            result.ActionName.Should().Be("ProjectOverview");
        }

        [Fact]
        public void DiscardDesignPlanningChanges_Redirects_To_ReturnUrl_When_ReturnUrl_Is__Not_Null()
        {
            var returnUrl = "/Home/Index";

            // Act
            var result = (RedirectToActionResult)_workpackageOneController.DiscardDesignPlanningChanges(returnUrl);

            // Assert
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");
        }
    }
}
