using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance;
using Xunit;

namespace Ubora.Domain.Tests.Projects.StructuredInformations
{
    public class ProviderOfMaintenanceTests
    {
        [Fact]
        public void All_Types_Of_Portability_Are_Represented_In_Dictionary()
        {
            var assemblyTypes = typeof(ProviderOfMaintenance).Assembly.GetTypes();
            var providerOfMaintenanceTypes = assemblyTypes.Where(t => typeof(ProviderOfMaintenance).IsAssignableFrom(t) && !t.IsAbstract);

            providerOfMaintenanceTypes.Should().NotBeEmpty();

            // Act
            foreach (var providerOfMaintenance in providerOfMaintenanceTypes)
            {
                var containsProviderOfMaintenanceType = ProviderOfMaintenance.ProviderOfMaintenanceKeyTypeMap.Values.Any(v => v == providerOfMaintenance);

                // Assert
                containsProviderOfMaintenanceType.Should().BeTrue($"{providerOfMaintenance} not found!");
            }
        }
    }
}
