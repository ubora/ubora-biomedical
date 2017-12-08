using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;
using Xunit;

namespace Ubora.Domain.Tests.Projects.StructuredInformations
{
    public class IntendedUserTests
    {
        [Fact]
        public void All_Types_Of_Intended_User_Are_Represented_In_Dictionary()
        {
            var assemblyTypes = typeof(IntendedUser).Assembly.GetTypes();
            var intendedUserTypes = assemblyTypes.Where(t => typeof(IntendedUser).IsAssignableFrom(t) && !t.IsAbstract);

            intendedUserTypes.Should().NotBeEmpty();

            // Act
            foreach (var intendedUserType in intendedUserTypes)
            {
                var containsIntendedUserType = IntendedUser.IntendedUserKeyTypeMap.Values.Any(v => v == intendedUserType);

                // Assert
                containsIntendedUserType.Should().BeTrue($"{intendedUserType} not found!");
            }
        }
    }
}
