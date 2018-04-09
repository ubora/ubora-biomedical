using System;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications;
using Ubora.Web._Features.Notifications._Base;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class NotificationViewModelFactoryMediatorTests
    {
        private readonly GeneralNotificationViewModel.Factory _generalFactory;

        public NotificationViewModelFactoryMediatorTests()
        {
            _generalFactory = new GeneralNotificationViewModel.Factory(Mock.Of<TokenReplacerMediator>());
        }

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

            var mediator = new NotificationViewModelFactoryMediator(factories, _generalFactory);

            // Act
            var result = mediator.Create(notification);

            // Assert
            result.Should().Be(expectedViewModel);
        }

        [Fact]
        public void Create_Tries_To_Return_ViewModel_From_General_Factory_When_Could_Not_Find_Concrete_Factory()
        {
            var factories = new[]
            {
                Mock.Of<INotificationViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
                Mock.Of<INotificationViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
            };

            var mediator = new NotificationViewModelFactoryMediator(factories, _generalFactory);
            var notification = new TestGeneralNotification();

            // Act
            var result = mediator.Create(notification);

            // Assert
            result.Should().BeOfType<GeneralNotificationViewModel>();
        }

        [Fact]
        public void Create_Throws_When_Does_Not_Find_Factory_For_Notification_Type()
        {
            var factories = new[]
            {
                Mock.Of<INotificationViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
                Mock.Of<INotificationViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
            };

            var mediator = new NotificationViewModelFactoryMediator(factories, _generalFactory);
            var notification = new TestNotification();

            // Act
            Action act = () => mediator.Create(notification);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        public class TestNotification : UserBinaryAction
        {
            public TestNotification() : base(Guid.Empty)
            {
            }
        }

        public class TestNotificationViewModel : INotificationViewModel<TestNotification>
        {
            public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
            {
                throw new NotImplementedException();
            }

            public bool IsUnread { get; }

            public DateTime CreatedAt => DateTime.UtcNow;
        }

        public class TestGeneralNotification : GeneralNotification
        {
            public TestGeneralNotification() : base(Guid.Empty)
            {
            }

            public override string GetDescription()
            {
                return "";
            }
        }
    }
}
