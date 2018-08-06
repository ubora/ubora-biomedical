using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ubora.Domain.Projects.Members;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class ProjectMemberTests
    {
        [Fact]
        public void All_Inherited_ProjectMembers_Must_Implement_Unique_Role_Key()
        {
            IEnumerable<Type> inheritedProjectMemberTypes = typeof(UserProfile).Assembly
                .GetTypes()
                .Where(type => typeof(UserProfile).IsAssignableFrom(type));

            var userId = Guid.NewGuid();
            IEnumerable<UserProfile> projectMemberInstances = inheritedProjectMemberTypes
                .Select(type => Activator.CreateInstance(type, userId))
                .Cast<UserProfile>()
                .ToList();

            // Act
            var roleKeys = projectMemberInstances.Select(x => x.RoleKey).ToList();

            // Assert
            AssertKeysAreUnique(roleKeys);
        }

        private static void AssertKeysAreUnique(List<string> roleKeys)
        {
            var isUnique = roleKeys.Distinct().Count() == roleKeys.Count();
            isUnique.Should().BeTrue();
        }
    }
}
