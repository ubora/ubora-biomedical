using System;
using FluentAssertions;
using Ubora.Domain.Notifications;
using Ubora.Web._Features.Notifications;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class GeneralNotificationViewModelFactoryTests
    {
        private readonly GeneralNotificationViewModel.Factory _factoryUnderTest;

        public GeneralNotificationViewModelFactoryTests()
        {
            _factoryUnderTest = new GeneralNotificationViewModel.Factory();
        }

        [Fact]
        public void CanCreateFor_Returns_True_When_Given_Notification_Type_Is_Inherited_From_General_Notification_Base_Class()
        {
            // Act
            var result = _factoryUnderTest.CanCreateFor(typeof(TestNotification));

            // Assert
            result.Should().BeTrue();
        }

        private class TestNotification : GeneralNotification
        {
            public TestNotification() : base(Guid.Empty)
            {
            }

            public override string GetDescription()
            {
                throw new NotImplementedException();
            }
        }
    }
}
