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
            var expectedUserEmail = "test@gmail.com";
            var ecpectedUserFirstName = "Test";
            this.Create_User(Guid.NewGuid(), expectedUserEmail, "Expected", "User1");
            this.Create_User(Guid.NewGuid(), "two@gmail.com", "Two", "Number");
            this.Create_User(Guid.NewGuid(), "expectedUser2@gmail.com", ecpectedUserFirstName, "ExpectedUser2");

            var searchPhrase = "test";
            var query = new SearchUsersQuery(searchPhrase);

            // Act
            var result = Processor.ExecuteQuery(query).ToList();

            // Assert
            result.Count().Should().Be(2);

            var firstExpectedUser = result.Single(x => x.Email.Equals(expectedUserEmail));
            firstExpectedUser.FirstName.Should().Be("Expected");
            firstExpectedUser.LastName.Should().Be("User1");

            var secondExpectedUser = result.Single(x => x.FirstName.Equals(ecpectedUserFirstName));
            secondExpectedUser.Email.Should().Be("expectedUser2@gmail.com");
            secondExpectedUser.LastName.Should().Be("ExpectedUser2");
        }
    }
}
