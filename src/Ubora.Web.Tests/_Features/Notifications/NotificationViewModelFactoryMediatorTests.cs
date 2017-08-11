using System;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications._Base;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class NotificationViewModelFactoryMediatorTests
    {
        [Fact]
        public void Create_Returns_ViewModel_From_Correct_Notification_Factory()
        {
            var correctFactoryMock = new Mock<NotificationViewModelFactory<TestNotification, TestNotificationViewModel>>
            {
                CallBase = true
            };

            var factories = new[]
            {
                Mock.Of<INotificationViewModelFactory>(x => x.CanCreateFor(typeof(TestNotification)) == false),
                correctFactoryMock.Object,
                new Mock<INotificationViewModelFactory>(MockBehavior.Strict).Object
            };

            var notification = new TestNotification();
            var expectedViewModel = new TestNotificationViewModel();

            correctFactoryMock.Setup(x => x.Create(notification))
                .Returns(expectedViewModel);

            var mediator = new NotificationViewModelFactoryMediator(factories);

            // Act
            var result = mediator.Create(notification);

            // Assert
            result.Should().Be(expectedViewModel);
        }

        [Fact]
        public void Create_Throws_When_Does_Not_Find_Factory_For_Notification_Type()
        {
            var factories = new[]
            {
                Mock.Of<INotificationViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
                Mock.Of<INotificationViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
            };
            var mediator = new NotificationViewModelFactoryMediator(factories);
            var notification = new TestNotification();

            // Act
            Action act = () => mediator.Create(notification);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        public class TestNotification : BaseNotification
        {
            public TestNotification() : base(Guid.Empty, Guid.Empty)
            {
            }

            public override bool IsArchived { get; }
            public override bool IsPending { get; }
        }

        public class TestNotificationViewModel : INotificationViewModel<TestNotification>
        {
            public IHtmlContent GetPartialView(IHtmlHelper htmlHelper, bool isHistory)
            {
                throw new NotImplementedException();
            }
        }
    }
}
