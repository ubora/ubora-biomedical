using System;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Users.Queries
{
    public class FindByEmailQueryTests : IntegrationFixture
    {
        [Fact]
        public void Returns_Users_With_Email()
        {
            var expectedUserId = Guid.NewGuid();

            this.Create_User(Guid.NewGuid(), "other1@test.com");
            this.Create_User(expectedUserId, "test@test.com");
            this.Create_User(Guid.NewGuid(), "other2@test.com");

            var query = new FindByEmailQuery
            {
                Email = " TeSt@test.COM "
            };

            // Act
            var result = Processor.ExecuteQuery(query);

            // Assert
            result.UserId.Should().Be(expectedUserId);
        }

        [Fact]
        public void Throws_When_More_Than_One_User_Found()
        {
            var duplicateEmail = "duplicate@test.com";
            var users = new[]
            {
                new UserProfile(Guid.NewGuid()) { Email = duplicateEmail }, 
                new UserProfile(Guid.NewGuid()) { Email = duplicateEmail }
            };
            var queryProcessor = Mock.Of<IQueryProcessor>(x => x.Find<UserProfile>(null) == users);

            var handler = new FindByEmailQuery.Handler(queryProcessor);
            var query = new FindByEmailQuery
            {
                Email = duplicateEmail
            };

            // Act
            Action act = () => handler.Handle(query);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }
    }
}
