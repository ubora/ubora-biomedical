using System;
using System.Collections.Generic;
using FluentAssertions;
using Ubora.Domain.Users.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class FindFullNamesOfAllUboraUsersQueryIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Query_Returns_Full_Names_Of_Ubora_Users()
        {
            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            this.Create_User(user1Id, firstName: "John", lastName: "Doe");
            this.Create_User(user2Id, firstName: "Jane", lastName: "Doe");

            var query = new FindFullNamesOfAllUboraUsersQuery();

            // Act
            var result = Processor.ExecuteQuery(query);

            // Assert
            var expectedResult = new Dictionary<Guid, string>
            {
                { user1Id, "John Doe" },
                { user2Id, "Jane Doe" },
            };

            result.ShouldBeEquivalentTo(expectedResult);
        }
    }
}
