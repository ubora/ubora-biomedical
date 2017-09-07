using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications;
using Xunit;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Notifications._Base;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class NotificationsControllerTests : UboraControllerTestsBase
    {
        private readonly NotificationsController _notificationsController;
        private readonly Mock<NotificationViewModelFactoryMediator> _notificationViewModelFactoryMediatorMock;

        public NotificationsControllerTests()
        {
            _notificationViewModelFactoryMediatorMock = new Mock<NotificationViewModelFactoryMediator>();
            _notificationsController = new NotificationsController(_notificationViewModelFactoryMediatorMock.Object);
            SetUpForTest(_notificationsController);
        }

        [Fact]
        public void Index_Marks_Invitations_As_Viewed()
        {
            var invitation = new InvitationToProject(UserId, Guid.NewGuid());
            var invitations = new List<InvitationToProject> { invitation };

            QueryProcessorMock
                .Setup(x => x.Find(new HasPendingNotifications(UserId)))
                .Returns(invitations);

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<MarkNotificationsAsViewedCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (ViewResult)_notificationsController.Index();

            // Assert
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<MarkNotificationsAsViewedCommand>()));
        }
    }
}
