﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Marten.Util;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Projects.Members;
using Ubora.Web._Features.Users.Account;
using Ubora.Web.Tests.Fakes;
using Xunit;
using Ubora.Web._Features.Projects.Dashboard;
using Ubora.Web._Features.Projects.Repository;

namespace Ubora.Web.Tests._Features.Projects.Members
{
    public class MembersControllerTests : ProjectControllerTestsBase
    {
        private readonly MembersController _membersController;
        private readonly Mock<FakeSignInManager> _signInManagerMock;

        public MembersControllerTests()
        {
            _signInManagerMock = new Mock<FakeSignInManager>();
            _membersController = new MembersController(_signInManagerMock.Object)
            {
                Url = Mock.Of<IUrlHelper>()
            };
            
            SetUpForTest(_membersController);
        }

        [Fact]
        public void Actions_Have_Authorize_Attributes()
        {
            var methodPolicies = new List<RolesAndPoliciesAuthorization>()
            {
                new RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(MembersController.RemoveMember),
                    Policies = new List<string>{ nameof(Policies.CanRemoveProjectMember) }
                }
            };

            AssertHasAuthorizeAttributes(typeof(MembersController), methodPolicies);


        }

        [Fact]
        public void RemoveMember_Removes_Member_From_Project()
        {
            var viewModel = new RemoveMemberViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
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

             CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.RemoveMember(viewModel);

            // Assert
            _membersController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Leave_Removes_Current_Member_From_Project()
        {
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_membersController.LeaveProject();

            // Assert
            result.ActionName.Should().Be(nameof(DashboardController.Dashboard));
        }

        [Fact]
        public void Leave_Returns_Message_If_Command_Failed()
        {
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.LeaveProject();

            // Assert
            _membersController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Join_Sends_Request()
        {
            CommandProcessorMock
                .Setup(x => x.Execute(It.Is<JoinProjectCommand>(y => y.Actor.UserId == UserId)))
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
            CommandProcessorMock
                .Setup(x => x.Execute(It.Is<JoinProjectCommand>(y => y.Actor.UserId == UserId)))
                .Returns(new CommandResult("Something went wrong"));

            var viewModel = new JoinProjectViewModel();

            // Act
            var result = (ViewResult)_membersController.Join(viewModel);

            // Assert
            _membersController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Join_Redirect_To_SignInSignUp_Page_If_User_Is_Not_Authenticated()
        {
            var projectId = Guid.NewGuid();

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

            QueryProcessorMock.Setup(x => x.FindById<Project>(projectId))
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
