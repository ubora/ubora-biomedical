using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Web._Features.Notifications;
using Ubora.Web._Features.Notifications.Invitations;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications.Invitations
{
    public class InvitationsControllerTests : UboraControllerTestsBase
    {
        private InvitationsController _invitationsController;
        private Mock<ICommandQueryProcessor> _commandQueryProcessorMock;

        public InvitationsControllerTests()
        {
            _commandQueryProcessorMock = new Mock<ICommandQueryProcessor>();
            _invitationsController = new InvitationsController(_commandQueryProcessorMock.Object);
            SetUserContext(_invitationsController);
        }

        [Fact]
        public void Accept_Redirects_To_Notifications_If_Accept_Command_Succeeds()
        {
            var vm = new InvitationPartialViewModel { InviteId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<AcceptInvitationToProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_invitationsController.Accept(vm);

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Accept_Returns_BadResult_If_Command_Fails()
        {
            var vm = new InvitationPartialViewModel { InviteId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<AcceptInvitationToProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = _invitationsController.Accept(vm);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Decline_Redirects_To_Notifications_If_Accept_Command_Succeeds()
        {
            var vm = new InvitationPartialViewModel { InviteId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<DeclineInvitationToProjectCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (RedirectToActionResult)_invitationsController.Decline(vm);

            // Assert
            result.ActionName.Should().Be(nameof(NotificationsController.Index));
        }

        [Fact]
        public void Decline_Returns_BadResult_If_Command_Fails()
        {
            var vm = new InvitationPartialViewModel { InviteId = Guid.NewGuid() };
            _commandQueryProcessorMock.Setup(x => x.Execute(It.IsAny<DeclineInvitationToProjectCommand>()))
                .Returns(new CommandResult("Something went wrong"));

            // Act
            var result = _invitationsController.Decline(vm);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
