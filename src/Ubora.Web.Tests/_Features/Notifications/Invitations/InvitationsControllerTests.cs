using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web._Features.Notifications;
using Ubora.Web._Features.Notifications.Invitations;
using Ubora.Web.Authorization;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications.Invitations
{
    public class InvitationsControllerTests : UboraControllerTestsBase
    {
        private readonly InvitationsController _invitationsController;

        public InvitationsControllerTests()
        {
            _invitationsController = new InvitationsController();
            SetUpForTest(_invitationsController);
        }

        [Fact]
        public async Task Accept_Redirects_To_Notifications_With_ModelState_Errors_If_User_Not_Authorized_To_Join_Project()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(User, null, Policies.CanJoinProject))
                .ReturnsAsync(AuthorizationResult.Failed);
            
            // Act
            var result = (RedirectToActionResult)await _invitationsController.Accept(invitationId: Guid.NewGuid());

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
            _invitationsController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public async Task Accept_Redirects_To_Notifications_If_Accept_Command_Succeed()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(User, null, Policies.CanJoinProject))
                .ReturnsAsync(AuthorizationResult.Success);

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<AcceptInvitationToProjectCommand>()))
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult) await _invitationsController.Accept(invitationId: Guid.NewGuid());

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public async Task Accept_Returns_ModelState_With_Error_If_Command_Fails()
        {
            AuthorizationServiceMock
                .Setup(x => x.AuthorizeAsync(User, null, Policies.CanJoinProject))
                .ReturnsAsync(AuthorizationResult.Success);

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<AcceptInvitationToProjectCommand>()))
                .Returns(CommandResult.Failed("Something went wrong"));

            // Act
            var result = (RedirectToActionResult) await _invitationsController.Accept(invitationId: Guid.NewGuid());

            // Assert
            _invitationsController.ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public void Decline_Redirects_To_Notifications_If_Accept_Command_Succeeds()
        {
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<DeclineInvitationToProjectCommand>()))
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)_invitationsController.Decline(invitationId: Guid.NewGuid());

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Decline_Returns_ModelState_With_Error_If_Command_Fails()
        {
            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<DeclineInvitationToProjectCommand>()))
                .Returns(CommandResult.Failed("Something went wrong"));

            // Act
            var result = (RedirectToActionResult)_invitationsController.Decline(invitationId: Guid.NewGuid());

            // Assert
            _invitationsController.ModelState.ErrorCount.Should().Be(1);
        }
    }
}
