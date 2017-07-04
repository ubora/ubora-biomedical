﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Security.Claims;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications.Join;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Web._Features.Projects.Members;
using Ubora.Web._Features.Users.Account;
using Ubora.Web.Tests.Fakes;
using Ubora.Web.Tests.Helper;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using Ubora.Web._Features.Projects.Dashboard;

namespace Ubora.Web.Tests._Features.Projects.Members
{
    public class MembersControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private readonly Mock<IAuthorizationService> _authorizationService;
        private readonly MembersController _membersController;
        private readonly Mock<FakeSignInManager> _signInManagerMock;
        private readonly Mock<IUrlHelperFactory> _urlHelperFactoryMock;

        public MembersControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _signInManagerMock = new Mock<FakeSignInManager>();
            _urlHelperFactoryMock = new Mock<IUrlHelperFactory>();
            _authorizationService = new Mock<IAuthorizationService>();
            _membersController = new MembersController(_processorMock.Object, _signInManagerMock.Object, _urlHelperFactoryMock.Object, _authorizationService.Object);
            SetProjectAndUserContext(_membersController);
        }

        [Fact]
        public void RemoveMember_Removes_Member_From_Project()
        {
            var viewModel = new RemoveMemberViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            _processorMock.Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_membersController.RemoveMember(viewModel);

            // Assert
            result.ActionName.Should().Be(nameof(MembersController.Members));
        }

        [Fact]
        public void RemoveMember_Returns_Message_If_Command_Failed()
        {
            var viewModel = new RemoveMemberViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            _processorMock.Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.RemoveMember(viewModel);

            // Assert
            ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Leave_Removes_Current_Member_From_Project()
        {
            _processorMock.Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_membersController.LeaveProject();

            // Assert
            result.ActionName.Should().Be(nameof(DashboardController.Dashboard));
        }

        [Fact]
        public void Leave_Returns_Message_If_Command_Failed()
        {
            _processorMock.Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.LeaveProject();

            // Assert
            ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Join_Sends_Request()
        {
            _processorMock.Setup(x => x.Execute(It.Is<JoinProjectCommand>(y => y.Actor.UserId == UserId)))
                .Returns(new CommandResult());

            var viewModel = new JoinProjectViewModel();

            // Act
            var result = (RedirectToActionResult)_membersController.Join(viewModel);

            // Assert
            result.ActionName.Should().Be(nameof(DashboardController.Dashboard));
        }

        [Fact]
        public void Join_Returns_Message_If_Command_Failed()
        {
            _processorMock.Setup(x => x.Execute(It.Is<JoinProjectCommand>(y => y.Actor.UserId == UserId)))
                .Returns(new CommandResult("Something went wrong"));

            var viewModel = new JoinProjectViewModel();

            // Act
            var result = (ViewResult)_membersController.Join(viewModel);

            // Assert
            ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Join_Redirect_To_SignInSignUp_Page_If_User_Is_Not_Authenticated()
        {
            var projectId = Guid.NewGuid();
            var urlHelper = Mock.Of<IUrlHelper>();
            _urlHelperFactoryMock.Setup(x => x.GetUrlHelper(_membersController.ControllerContext))
                .Returns(urlHelper);

            // Act
            var result = (RedirectToActionResult)_membersController.Join(projectId);

            // Assert
            result.ActionName.Should().Be(nameof(AccountController.SignInSignUp));
        }

        [Fact]
        public void Join_Returns_View_If_User_Is_Signed_In()
        {
            _signInManagerMock.Setup(x => x.IsSignedIn((ClaimsPrincipal)User))
                .Returns(true);

            var projectId = Guid.NewGuid();
            var project = new Project()
                .Set(x => x.Id, projectId)
                .Set(x => x.Title, "projectTitle");

            _processorMock.Setup(x => x.FindById<Project>(projectId))
                .Returns(project);

            // Act
            var result = (ViewResult)_membersController.Join(projectId);

            // Assert
            var viewModel = (JoinProjectViewModel)result.Model;
            viewModel.ProjectId.Should().Be(projectId);
            viewModel.UserId.Should().Be(UserId);
            viewModel.ProjectName.Should().Be("projectTitle");
        }
    }
}
