using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features.Projects.Workpackages.Reviews;
using Xunit;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageOneReviewControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<WorkpackageOneReviewController> _workpackageOneReviewControllerMock;
        private readonly WorkpackageOneReviewController _workpackageOneReviewController;

        public WorkpackageOneReviewControllerTests()
        {
            _workpackageOneReviewControllerMock = new Mock<WorkpackageOneReviewController> { CallBase = true };
            _workpackageOneReviewController = _workpackageOneReviewControllerMock.Object;
            _workpackageOneReviewController.Url = Mock.Of<IUrlHelper>();

            SetUpForTest(_workpackageOneReviewController);

            var dummyWorkpackage = new WorkpackageOne();
            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(dummyWorkpackage);
        }

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var rolesAndPoliciesAuthorizations = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
                {
                    new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                    {
                        MethodName = nameof(WorkpackageOneReviewController.SubmitForReview),
                        Policies = new []{ nameof(Policies.CanSubmitWorkpackageForReview) }
                    },
                    new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                    {
                        MethodName = nameof(WorkpackageOneReviewController.Decision),
                        Policies = new []{ nameof(Policies.CanReviewProjectWorkpackages) }
                    },
                    new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                    {
                        MethodName = nameof(WorkpackageOneReviewController.Accept),
                        Policies = new []{ nameof(Policies.CanReviewProjectWorkpackages) }
                    },
                    new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                    {
                        MethodName = nameof(WorkpackageOneReviewController.Reject),
                        Policies = new []{ nameof(Policies.CanReviewProjectWorkpackages) }
                    },
                    new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                    {
                        MethodName = nameof(WorkpackageOneReviewController.ReopenWorkpackageAfterAcceptance),
                        Policies = new []{ nameof(Policies.CanReviewProjectWorkpackages) }
                    }
                };

            AssertHasAuthorizeAttributes(typeof(WorkpackageOneReviewController), rolesAndPoliciesAuthorizations);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task Submit_Button_Is_Hidden_Completely_When_Workpackage_Is_Under_Review_Or_Has_Been_Accepted(
            bool isReviewInProcess,
            bool hasBeenAccepted)
        {
            var workpackage = Mock.Of<WorkpackageOne>(
                x => x.HasReviewInProcess == isReviewInProcess
                && x.HasBeenAccepted == hasBeenAccepted
                && x.Reviews == new List<WorkpackageReview>());

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackage);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsHiddenCompletely
                .Should().BeTrue();
        }

        [Fact]
        public async Task Submit_Button_Is_Hidden_With_Message_When_User_Is_Not_Authorized()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), Policies.CanSubmitWorkpackageForReview))
                .ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsHiddenWithMessage
                .Should().BeTrue();
        }

        [Fact]
        public async Task Submit_Button_Can_Be_Visible()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), Policies.CanSubmitWorkpackageForReview))
                .ReturnsAsync(AuthorizationResult.Success);

            var members = new List<ProjectMember> { new ProjectMentor(Guid.NewGuid()) };
            var project = new Project();
            project.Set(p => p.Members, members);

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId)).Returns(project);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsVisible
                .Should().BeTrue();
        }

        [Fact]
        public async Task Submit_Button_Can_Be_Visible_Request_Mentoring()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), Policies.CanSubmitWorkpackageForReview))
                .ReturnsAsync(AuthorizationResult.Success);

            var project = new Project();

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId)).Returns(project);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;
            viewModel
                .SubmitForReviewButton.IsVisibleRequestMentoring
                .Should().BeTrue();
        }

        [Fact]
        public async Task Review_Maps_Latest_Review_To_ViewModel()
        {
            DisableAuthorization();

            var expectedReview = Mock.Of<WorkpackageReview>();
            var workpackageMock = new Mock<WorkpackageOne> { CallBase = true };

            workpackageMock.Setup(wp => wp.GetLatestReviewOrNull())
                .Returns(expectedReview);

            var expectedReviewViewModel = new WorkpackageReviewViewModel();
            AutoMapperMock.Setup(m => m.Map<WorkpackageReviewViewModel>(expectedReview))
                .Returns(expectedReviewViewModel);

            var project = new Project();
            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId)).Returns(project);

            QueryProcessorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageMock.Object);

            // Act
            var result = (ViewResult)await _workpackageOneReviewController.Review();

            // Assert
            var viewModel = (WorkpackageReviewListViewModel)result.Model;

            viewModel.LatestReview.Should().BeSameAs(expectedReviewViewModel);
        }

        [Fact]
        public async Task ReopenWorkpackageAfterAcceptance_Opens_Workpackage_For_Edits_After_It_Was_Accepted_By_Review()
        {
            var model = new ReopenWorkpackageAfterAcceptanceByReviewPostModel
            {
                LatestReviewId = Guid.NewGuid()
            };

            ReopenWorkpackageAfterAcceptanceByReviewCommand executedCommand = null;

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<ReopenWorkpackageAfterAcceptanceByReviewCommand>()))
                .Callback<ReopenWorkpackageAfterAcceptanceByReviewCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)await _workpackageOneReviewController.ReopenWorkpackageAfterAcceptance(model);

            // Assert
            executedCommand.LatestReviewId.Should().Be(model.LatestReviewId);

            result.ActionName.Should().Be(nameof(WorkpackageOneReviewController.Review));
        }

        [Fact]
        public async Task ReopenWorkpackageAfterAcceptance_Does_Not_Execute_Command_When_ModelState_Failure()
        {
            var model = new ReopenWorkpackageAfterAcceptanceByReviewPostModel();
            _workpackageOneReviewController.ViewData.ModelState.AddModelError("", "test_error");

            var expectedResult = Mock.Of<IActionResult>();
            _workpackageOneReviewControllerMock.Setup(c => c.Review())
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _workpackageOneReviewController.ReopenWorkpackageAfterAcceptance(model);

            // Assert
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ReopenWorkpackageAfterAcceptanceByReviewCommand>()), Times.Never());

            result.Should().Be(expectedResult);
        }

        private void DisableAuthorization()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(this.User, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success);
        }
    }
}
