using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Queries;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web._Features.Admin;
using Xunit;

namespace Ubora.Web.Tests._Features.Admin
{
    public class AdminControllerTests : UboraControllerTestsBase
    {
        private readonly AdminController _controller;
        private readonly Mock<IApplicationUserManager> _userManagerMock;

        public AdminControllerTests()
        {
            _userManagerMock = new Mock<IApplicationUserManager>();

            _controller = new AdminController(_userManagerMock.Object);
            SetUpForTest(_controller);
        }

        [Fact]
        public async Task ReturnViewWhenInvalidModelState()
        {
            var model = new DeleteUserViewModel
            {
                UserEmail = "test@test.ee"
            };

            _controller.ModelState.AddModelError("", "");

            // Act
            IActionResult result = await _controller.DeleteUser(model);

            // Assert
            _userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task ReturnViewAndDeleteAsyncIsNeverCalledWhenNoUserIsFound()
        {
            var model = new DeleteUserViewModel
            {
                UserEmail = "test@test.ee"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = (ViewResult)await _controller.DeleteUser(model);

            // Assert
            _userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);

            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUserCommandIsNeverCalledAndViewisReturnedWhenDeleteAsyncFailed()
        {
            var model = new DeleteUserViewModel
            {
                UserEmail = "test@test.ee"
            };
            var user = new ApplicationUser { Id = Guid.NewGuid() };

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test-error" }));

            // Act
            var result = (ViewResult)await _controller.DeleteUser(model);

            // Assert
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<DeleteUserCommand>()), Times.Never);

            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task RedirectToActionWhenDeleteUserCommandIsSuccess()
        {
            DeleteUserCommand executedCommand = null;

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<DeleteUserCommand>()))
                .Callback<DeleteUserCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var model = new DeleteUserViewModel
            {
                UserEmail = "test@test.ee"
            };
            var expectedUserId = Guid.NewGuid();
            var user = new ApplicationUser { Id = expectedUserId };

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = (RedirectToActionResult)await _controller.DeleteUser(model);

            // Assert
            executedCommand.UserId.Should().Be(user.Id);

            result.ActionName.Should().Be("ManageUsers");
        }

        [Fact]
        public async Task ReturnViewWhenDeleteUserCommandIsFailure()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<DeleteUserCommand>()))
                .Returns(CommandResult.Failed("test viga"));

            var model = new DeleteUserViewModel
            {
                UserEmail = "test@test.ee"
            };
            var user = new ApplicationUser { Id = Guid.NewGuid() };

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.UserEmail))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = (ViewResult)await _controller.DeleteUser(model);

            // Assert
            result.ViewName.Should().Be("ManageUsers");

            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ProjectsUnderReview_Returns_View_With_Expected_Model()
        {
            var queryProcessorMock = new Mock<IQueryProcessor>();
            var projectUnderReviewModelFactoryMock = new Mock<ProjectUnderReviewViewModel.Factory>(queryProcessorMock.Object);

            var project1 = new Project();
            var project2 = new Project();

            QueryProcessorMock.Setup(x => x.ExecuteQuery(It.Is<GetProjectsUnderReviewQuery>(q => q == q)))
                .Returns(new[] { project1, project2 });

            var project1Model = new ProjectUnderReviewViewModel();
            projectUnderReviewModelFactoryMock.Setup(x => x.Create(project1))
                .Returns(project1Model);

            var project2Model = new ProjectUnderReviewViewModel();
            projectUnderReviewModelFactoryMock.Setup(x => x.Create(project2))
                .Returns(project2Model);

            // Act
            var result = (ViewResult)_controller.ProjectsUnderReview(projectUnderReviewModelFactoryMock.Object);

            // Assert
            result.ViewName.Should().Be(nameof(AdminController.ProjectsUnderReview));

            var expectedModel = new ProjectsUnderReviewViewModel
            {
                ProjectsUnderReview = new[] { project1Model, project2Model }
            };

            result.Model.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
