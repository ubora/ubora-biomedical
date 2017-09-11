using System;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class GeneralNotificationViewModelFactoryTests
    {
        private readonly GeneralNotificationViewModel.Factory _factoryUnderTest;
        private readonly Mock<TokenReplacerMediator> _tokenReplacerMediatorMock;

        public GeneralNotificationViewModelFactoryTests()
        {
            _tokenReplacerMediatorMock = new Mock<TokenReplacerMediator>();
            _factoryUnderTest = new GeneralNotificationViewModel.Factory(_tokenReplacerMediatorMock.Object);
        }

        [Fact]
        public void CanCreateFor_Returns_True_When_Given_Notification_Type_Is_Inherited_From_General_Notification_Base_Class()
        {
            // Act
            var result = _factoryUnderTest.CanCreateFor(typeof(TestNotification));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Creates_General_View_Model_With_Text_Encoded_And_Tokens_Replaced()
        {
            var notification = new TestNotification();
            var encodedAndReplacedText = Mock.Of<IHtmlContent>();

            _tokenReplacerMediatorMock.Setup(x => x.EncodeAndReplaceAllTokens(notification.GetDescription()))
                .Returns(encodedAndReplacedText);

            // Act
            var result = _factoryUnderTest.Create(notification);

            // Assert
            result.Message.Should().BeSameAs(encodedAndReplacedText);
        }

        private class TestNotification : GeneralNotification
        {
            public TestNotification() : base(Guid.Empty)
            {
            }

            public override string GetDescription()
            {
                return "test";
            }
        }
    }
}
