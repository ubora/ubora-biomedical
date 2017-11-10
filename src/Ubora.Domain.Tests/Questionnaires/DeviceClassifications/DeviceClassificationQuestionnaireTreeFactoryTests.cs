using FluentAssertions;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Tests.Helpers;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTreeFactoryTests
    {
        [Fact]
        public void Device_Classification_Questionnaire_Can_Be_Serialized_And_Deserialized_With_Marten()
        {
            var questionnaire = new DeviceClassificationQuestionnaireTreeFactory().CreateDeviceClassificationVersionOne();
            var serializer = UboraStoreOptionsConfigurer.CreateConfiguredJsonSerializer();

            // Act
            var serialized = serializer.ToJson(questionnaire);
            var deserialized = serializer.FromJson<DeviceClassificationQuestionnaireTree>(serialized);

            // Assert
            deserialized.ShouldBeEquivalentTo(questionnaire); // Compares object graphs

            // Double check by serializing again
            var serializedAgain = serializer.ToJson(deserialized);
            serializedAgain.Should().Be(serialized);
        }

        [Fact]
        public void Device_Classification_Questionnaire_Should_Not_Be_Too_Big_When_Serialized()
        {
            var questionnaire = new DeviceClassificationQuestionnaireTreeFactory().CreateDeviceClassificationVersionOne();
            var serializer = UboraStoreOptionsConfigurer.CreateConfiguredJsonSerializer();

            // Act
            var serialized = serializer.ToJson(questionnaire);

            // Assert
            serialized.Length.Should().BeLessThan(20000); // Let's keep it at less than ~20KB
        }
    }
}
