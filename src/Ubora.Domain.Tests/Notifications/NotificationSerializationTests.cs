using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Marten.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Notifications;
using Ubora.Domain.Tests.Helpers;
using Xunit;

namespace Ubora.Domain.Tests.Notifications
{
    /// <summary>
    /// Very important to test serialization/deserialization of immutable fields!
    /// </summary>
    public class NotificationSerializationTests
    {
        [Fact]
        public void Notifications_Can_Be_Serialized_And_Deserialized_Without_Data_Loss()
        {
            var autoFixture = new AutoFixture.Fixture();
            autoFixture.Register<IEvent>(() => new Event<TestEvent>(new TestEvent()));

            var notificationTypes = typeof(INotification).Assembly
                .GetTypes()
                .Where(type => typeof(INotification).IsAssignableFrom(type)
                               && !type.IsAbstract);

            var serializer = UboraStoreOptionsConfigurer.CreateConfiguredJsonSerializer();

            foreach (var notificationType in notificationTypes)
            {
                var notification = autoFixture.Create(notificationType, new SpecimenContext(autoFixture));

                // Act
                var serialized = serializer.ToJson(notification);
                var deserialized = serializer.FromJson(notificationType, serialized);

                // Assert
                try
                {
                    notification.ShouldBeEquivalentTo(deserialized, opt => opt.RespectingRuntimeTypes());
                }
                catch (Exception ex)
                {
                    // .NET Core does not log to the console so I wrap with an Exception to display the offending type.
                    throw new Exception($"Type which can not be serialized and deserialized without loss: {notificationType.FullName}", ex);
                }
            }
        }

        public class TestEvent
        {
        }
    }
}
