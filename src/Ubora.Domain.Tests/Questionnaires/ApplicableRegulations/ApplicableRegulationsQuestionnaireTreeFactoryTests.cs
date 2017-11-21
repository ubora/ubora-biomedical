using FluentAssertions;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Tests.Helpers;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireTreeFactoryTests
    {
        [Fact]
        public void Applicable_Regulations_Decision_Tree_Can_Be_Serialized_And_Deserialized_With_Marten()
        {
            var questionnaire = Domain.Questionnaires.ApplicableRegulations.ApplicableRegulationsQuestionnaireTreeFactory.Create();
            var serializer = UboraStoreOptionsConfigurer.CreateConfiguredJsonSerializer();

            // Act
            var serialized = serializer.ToJson(questionnaire);
            var deserialized = serializer.FromJson<ApplicableRegulationsQuestionnaireTree>(serialized);

            // Assert
            deserialized.ShouldBeEquivalentTo(questionnaire); // Compares object graphs

            // Double check by serializing again
            var serializedAgain = serializer.ToJson(deserialized);
            serializedAgain.Should().Be(serialized);
        }
    }
}
