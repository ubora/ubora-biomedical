using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Users.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Users.Queries
{
    public class SearchUsersQueryTests : IntegrationFixture
    {
        [Fact]
        public void SearchUsersQuery_Returns_UserProfiles_Which_Name_Or_Email_Contains_Search_Phrase()
        {
            this.Create_User(Guid.NewGuid(), "test@gmail.com", "One", "Number");
            this.Create_User(Guid.NewGuid(), "two@gmail.com", "Two", "Number");
            this.Create_User(Guid.NewGuid(), "one@gmail.com", "Test", "Number");


            var searchPhrase = "test";
            var query = new SearchUsersQuery(searchPhrase);

            // Act
            var result = Processor.ExecuteQuery(query);

            // Assert
            result.Count().Should().Be(2);
        }
    }
}
