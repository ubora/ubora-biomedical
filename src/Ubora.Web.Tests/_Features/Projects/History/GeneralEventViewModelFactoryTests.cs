using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;
using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Web._Features._Shared.Tokens;
using Ubora.Web._Features.Projects.History;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.History
{
    public class GeneralEventViewModelFactoryTests
    {
        private readonly GeneralEventViewModel.Factory _factoryUnderTest;
        private readonly Mock<TokenReplacerMediator> _tokenReplacerMediatorMock;

        public GeneralEventViewModelFactoryTests()
        {
            _tokenReplacerMediatorMock = new Mock<TokenReplacerMediator>();
            _factoryUnderTest = new GeneralEventViewModel.Factory(_tokenReplacerMediatorMock.Object);
        }

        [Fact]
        public void CanCreateFor_Returns_True_When_Given_Notification_Type_Is_Inherited_From_General_Notification_Base_Class()
        {
            // Act
            var result = _factoryUnderTest.CanCreateFor(typeof(TestUboraEvent));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Create_Creates_General_ViewModel_With_Text_Encoded_And_Tokens_Replaced()
        {
            var userInfo = new UserInfo(Guid.NewGuid(), "name");
            var uboraEvent = new TestUboraEvent(userInfo);
            var encodedAndReplacedText = Mock.Of<IHtmlContent>();

            _tokenReplacerMediatorMock.Setup(x => x.EncodeAndReplaceAllTokens(uboraEvent.ToString()))
                .Returns(encodedAndReplacedText);

            var timestamp = DateTimeOffset.Now;

            // Act
            var result = _factoryUnderTest.Create(uboraEvent, timestamp);

            // Assert
            result.Message.Should().BeSameAs(encodedAndReplacedText);
            result.Timestamp.Should().Be(timestamp);
        }

        public class TestUboraEvent : UboraEvent
        {
            public TestUboraEvent(UserInfo initiatedBy) : base(initiatedBy)
            {
            }

            public override string GetDescription()
            {
                return "";
            }
        }
    }
}
