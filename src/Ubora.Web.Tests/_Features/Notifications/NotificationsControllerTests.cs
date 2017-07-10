using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications;
using Xunit;
using Ubora.Web.Services;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Domain.Notifications.Specifications;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class NotificationsControllerTests : UboraControllerTestsBase
    {
        private Mock<HistoryViewModel.Factory> _historyViewModelFactoryMock;
        private Mock<IndexViewModel.Factory> _indexViewModelFactoryMock;
        private NotificationsController _notificationsController;

        public NotificationsControllerTests()
        {
            _historyViewModelFactoryMock = new Mock<HistoryViewModel.Factory>();
            _indexViewModelFactoryMock = new Mock<IndexViewModel.Factory>();

            _notificationsController = new NotificationsController();
            SetMocks(_notificationsController);
            SetUserContext(_notificationsController);
        }

        [Fact]
        public void History_Returns_Answered_Invitations()
        {
            var invitation1 = new HistoryInvitationViewModel
            {
                ProjectTitle = "Title1",
                WasAccepted = true
            };
            var invitation2 = new HistoryInvitationViewModel
            {
                ProjectTitle = "Title2",
                WasAccepted = false
            };

            var invitations = new List<NotificationViewModel>
            {
                invitation1,
                invitation2
            };

            var historyViewModel = new HistoryViewModel { Notifications = invitations };
            _historyViewModelFactoryMock.Setup(x => x.Create(UserId))
                .Returns(historyViewModel);

            // Act
            var result = (ViewResult)_notificationsController.History(_historyViewModelFactoryMock.Object);

            // Assert
            var viewModel = (HistoryViewModel)result.Model;
            viewModel.Notifications.ShouldBeEquivalentTo(invitations);
        }

        [Fact]
        public void Index_Returns_Unanswered_Invitations()
        {
            var invitation1 = new IndexInvitationViewModel
            {
                ProjectTitle = "Title1",
                IsUnread = false,
                InviteId = Guid.NewGuid()
            };
            var invitation2 = new IndexInvitationViewModel
            {
                ProjectTitle = "Title2",
                IsUnread = true,
                InviteId = Guid.NewGuid()
            };

            var invitations = new List<NotificationViewModel>
            {
                invitation1,
                invitation2
            };

            var indexViewModel = new IndexViewModel { Notifications = invitations };
            _indexViewModelFactoryMock.Setup(x => x.Create(UserId))
                .Returns(indexViewModel);

            CommandProcessorMock
                .Setup(x => x.Execute(It.IsAny<MarkNotificationsAsViewedCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (ViewResult)_notificationsController.Index(_indexViewModelFactoryMock.Object);

            // Assert
            var viewModel = (IndexViewModel)result.Model;
            viewModel.Notifications.ShouldBeEquivalentTo(invitations);
        }

        [Fact]
        public void Index_Marks_Invitations_As_Viewed()
        {
            var invitation = new InvitationToProject(Guid.NewGuid(), UserId, UserId, Guid.NewGuid());
            var invitations = new List<InvitationToProject> { invitation };

            QueryProcessorMock
                .Setup(x => x.Find(new HasUnViewedNotifications(UserId)))
                .Returns(invitations);

            var userInfo = User.GetInfo();

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<MarkNotificationsAsViewedCommand>()))
                .Returns(new CommandResult());

            // Act
            var result = (ViewResult)_notificationsController.Index(_indexViewModelFactoryMock.Object);

            // Assert
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<MarkNotificationsAsViewedCommand>()));
        }
    }
}
