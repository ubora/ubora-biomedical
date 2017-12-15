using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations.Portabilities;
using Xunit;

namespace Ubora.Domain.Tests.Projects.StructuredInformations
{
    public class PortabilityTests
    {
        [Fact]
        public void All_Types_Of_Portability_Are_Represented_In_Dictionary()
        {
            var assemblyTypes = typeof(Portability).Assembly.GetTypes();
            var portabilityTypes = assemblyTypes.Where(t => typeof(Portability).IsAssignableFrom(t) && !t.IsAbstract);

            portabilityTypes.Should().NotBeEmpty();

            // Act
            foreach (var portabilityType in portabilityTypes)
            {
                var containsPortabilityType = Portability.PortabilityKeyTypeMap.Values.Any(v => v == portabilityType);

                // Assert
                containsPortabilityType.Should().BeTrue($"{portabilityType} not found!");
            }
        }
    }
}
