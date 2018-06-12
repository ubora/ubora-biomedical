using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Data;
using Ubora.Web.Tests.Fakes;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features.Projects.InviteMentors;
using Ubora.Web._Features._Shared.Notices;
using Xunit;
using Ubora.Web.Authorization;

namespace Ubora.Web.Tests._Features.Projects.InviteMentors
{
    public class InviteMentorsControllerTests : ProjectControllerTestsBase
    {
        private readonly InviteMentorsController _inviteMentorsController;
        private readonly Mock<FakeUserManager> _userManagerMock;

        public InviteMentorsControllerTests()
        {
            _userManagerMock = new Mock<FakeUserManager>();
            _inviteMentorsController = new InviteMentorsController(_userManagerMock.Object);

            SetUpForTest(_inviteMentorsController);
        }

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var rolesAndPoliciesAuthorizations = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
            {
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(InviteMentorsController.InviteMentor),
                    Policies = new []{ nameof(Policies.CanInviteMentors) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(InviteMentorsController.InviteMentors),
                    Policies = new []{ nameof(Policies.CanInviteMentors) }
                }
            };

            AssertHasAuthorizeAttributes(typeof(InviteMentorsController), rolesAndPoliciesAuthorizations);
        }

        [Fact]
        public void Throws_When_Invitee_Can_Not_Be_Found_By_Id()
        {
            // Act
            Action act = () =>
            {
                _inviteMentorsController.InviteMentor(Guid.Empty, Mock.Of<MentorsViewModel.Factory>())
                    .GetAwaiter().GetResult();
            };

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public async Task Returns_Error_When_Invitee_Is_Not_In_Ubora_Mentor_Role()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };

            _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            // Act
            var result = (ViewResult)await _inviteMentorsController.InviteMentor(user.Id, Mock.Of<MentorsViewModel.Factory>());

            // Assert
            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Executes_Command_With_Notice_When_Valid()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };

            _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, ApplicationRole.Mentor))
                .ReturnsAsync(true);

            InviteProjectMentorCommand executedCommand = null;
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<InviteProjectMentorCommand>()))
                .Callback<InviteProjectMentorCommand>(cmd => executedCommand = cmd)
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)await _inviteMentorsController.InviteMentor(user.Id, Mock.Of<MentorsViewModel.Factory>());

            // Assert
            executedCommand.UserId.Should().Be(user.Id);

            result.ActionName.Should().Be(nameof(InviteMentorsController.InviteMentors));

            _inviteMentorsController.Notices.Any(x => x.Type == NoticeType.Success).Should().BeTrue();
        }

        [Fact]
        public async Task Returns_Error_When_Command_Execution_Failed()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };

            _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, ApplicationRole.Mentor))
                .ReturnsAsync(true);

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<InviteProjectMentorCommand>()))
                .Returns(CommandResult.Failed(""));

            // Act
            var result = (ViewResult)await _inviteMentorsController.InviteMentor(user.Id, Mock.Of<MentorsViewModel.Factory>());

            // Assert
            result.ViewData.ModelState.IsValid.Should().BeFalse();

            _inviteMentorsController.Notices.Any().Should().BeFalse();
        }
    }
}
