using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations.TypesOfUse;
using Xunit;

namespace Ubora.Domain.Tests.Projects.StructuredInformations
{
    public class TypeOfUseTests
    {
        [Fact]
        public void All_Types_Of_Portability_Are_Represented_In_Dictionary()
        {
            var assemblyTypes = typeof(TypeOfUse).Assembly.GetTypes();
            var typeOfUseTypes = assemblyTypes.Where(t => typeof(TypeOfUse).IsAssignableFrom(t) && !t.IsAbstract);

            typeOfUseTypes.Should().NotBeEmpty();

            // Act
            foreach (var typeOfUseType in typeOfUseTypes)
            {
                var containsTypeOfUseType = TypeOfUse.TypeOfUseKeyTypeMap.Values.Any(v => v == typeOfUseType);

                // Assert
                containsTypeOfUseType.Should().BeTrue($"{typeOfUseType} not found!");
            }
        }
    }
}
