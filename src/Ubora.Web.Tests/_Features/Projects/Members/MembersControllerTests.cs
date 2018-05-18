using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;

using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.Projects.Members;
using Ubora.Web._Features.Users.Account;
using Ubora.Web.Tests.Fakes;
using Ubora.Web.Tests.Helper;
using Xunit;
using Ubora.Web._Features.Projects.Dashboard;

namespace Ubora.Web.Tests._Features.Projects.Members
{
    public class MembersControllerTests : ProjectControllerTestsBase
    {
        private readonly MembersController _membersController;
        private readonly Mock<FakeSignInManager> _signInManagerMock;

        public MembersControllerTests()
        {
            _signInManagerMock = new Mock<FakeSignInManager>();
            _membersController = new MembersController(_signInManagerMock.Object, Mock.Of<ImageStorageProvider>())
            {
                Url = Mock.Of<IUrlHelper>()
            };

            SetUpForTest(_membersController);
        }

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var methodPolicies = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
            {
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(MembersController.RemoveMember),
                    Policies = new []{ nameof(Policies.CanRemoveProjectMember) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(MembersController.RemoveMentor),
                    Policies = new []{ nameof(Policies.CanRemoveProjectMentor) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(MembersController.PromoteMember),
                    Policies = new []{ nameof(Policies.CanPromoteMember) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(MembersController.Join),
                    Policies = new []{ nameof(Policies.CanJoinProject) }
                },
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
                .Returns(CommandResult.Success);

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
               .Returns(CommandResult.Failed("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.RemoveMember(viewModel);

            // Assert
            _membersController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void RemoveMentor_Removes_Member_From_Project()
        {
            var viewModel = new RemoveMentorViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)_membersController.RemoveMentor(viewModel);


            // Assert
            result.ActionName.Should().Be(nameof(MembersController.Members));
        }

        [Fact]
        public void RemoveMentor_Returns_Message_If_Command_Failed()
        {
            var viewModel = new RemoveMentorViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            CommandProcessorMock
               .Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
               .Returns(CommandResult.Failed("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.RemoveMentor(viewModel);

            // Assert
            _membersController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void PromoteMember_Promotes_Member_From_Project()
        {
            var viewModel = new PromoteMemberViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<PromoteProjectLeaderCommand>()))
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)_membersController.PromoteMember(viewModel);

            // Assert
            result.ActionName.Should().Be(nameof(MembersController.Members));
        }

        [Fact]
        public void PromoteMember_Returns_Message_If_Command_Failed()
        {
            var viewModel = new PromoteMemberViewModel
            {
                MemberId = UserId,
                MemberName = "MemberName"
            };

            CommandProcessorMock
               .Setup(x => x.Execute(It.IsAny<PromoteProjectLeaderCommand>()))
               .Returns(CommandResult.Failed("Something went wrong"));

            // Act
            var result = (ViewResult)_membersController.PromoteMember(viewModel);

            // Assert
            _membersController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Leave_Removes_Current_Member_From_Project()
        {
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<RemoveMemberFromProjectCommand>()))
                .Returns(CommandResult.Success);

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
                .Returns(CommandResult.Failed("Something went wrong"));

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
                .Returns(CommandResult.Success);

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
                .Returns(CommandResult.Failed("Something went wrong"));

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
