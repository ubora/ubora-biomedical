using System;
using System.Collections.Generic;
using FluentAssertions;
using Ubora.Domain.Users.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class FindFullNamesQueryIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Foo123()
        {
            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            var user3Id = Guid.NewGuid();
            this.Create_User(user1Id, firstName: "John", lastName: "Doe");
            this.Create_User(user2Id, firstName: "Jane", lastName: "Doe");
            this.Create_User(user3Id, firstName: "John", lastName: "Smith");


            var query = new FindFullNamesQuery(new [] {user1Id, user2Id});

            // Act
            var result = Processor.ExecuteQuery(query);

            // Assert
            var expectedResult = new Dictionary<Guid, string>
            {
                {user1Id, "John Doe" },
                {user2Id, "Jane Doe" },
            };
            
            result.ShouldBeEquivalentTo(expectedResult);
        }
    }
}
