using System;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Tests.Helpers;
using Xunit;

namespace Ubora.Domain.Tests
{
    public class UboraEventSerializationTests
    {
        [Fact]
        public void Events_Can_Be_Serialized_And_Deserialized_Without_Data_Loss()
        {
            var autoFixture = new AutoFixture.Fixture();
            autoFixture.Register<DeviceClassificationQuestionnaireTree>(() => new DeviceClassificationQuestionnaireTreeFactory().CreateDeviceClassificationVersionOne());
            autoFixture.Register<ApplicableRegulationsQuestionnaireTree>(() => ApplicableRegulationsQuestionnaireTreeFactory.Create());
            autoFixture.Register<IntendedUser>(() => new FamilyMember());
            autoFixture.Register<QuillDelta>(() => new QuillDelta("{" + Guid.NewGuid() + "}"));

            var uboraEventTypes = DomainAutofacModule.FindDomainEventConcreteTypes();

            var serializer = UboraStoreOptionsConfigurer.CreateConfiguredJsonSerializer();

            foreach (var notificationType in uboraEventTypes)
            {
                var notification = autoFixture.Create(notificationType, new SpecimenContext(autoFixture));

                try
                {
                    // Act
                    var serialized = serializer.ToJson(notification);
                    var deserialized = serializer.FromJson(notificationType, serialized);
    
                    // Assert
                    notification.ShouldBeEquivalentTo(deserialized, opt => opt.RespectingRuntimeTypes());
                }
                catch (Exception ex)
                {
                    // .NET Core does not log to the console so I wrap with an Exception to display the offending type.
                    throw new Exception($"Type which can not be serialized and deserialized without loss: {notificationType.FullName}", ex);
                }
            }
        }
    }
}