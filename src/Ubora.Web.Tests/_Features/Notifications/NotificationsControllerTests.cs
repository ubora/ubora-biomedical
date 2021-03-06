﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Marten.Pagination;
using Ubora.Web._Features.Notifications;
using Xunit;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Commands;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members;
using Ubora.Web._Features.Notifications._Base;
using Ubora.Domain.Notifications.SortSpecifications;

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
            var invitations = new Domain.Tests.PagedListStub<INotification> { invitation };

            QueryProcessorMock
                .Setup(x => x.Find(new IsForUser(UserId), It.IsAny<SortByCreatedAtSpecification>(), 10, 1))
                .Returns(invitations);

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<MarkNotificationsAsViewedCommand>()))
                .Returns(CommandResult.Success);

            // Act
            var result = (ViewResult)_notificationsController.Index(1);

            // Assert
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<MarkNotificationsAsViewedCommand>()));
        }
    }
}
