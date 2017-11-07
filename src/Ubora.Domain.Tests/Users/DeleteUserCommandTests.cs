using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Marten.Linq.SoftDeletes;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Users
{

    public class DeleteUserCommandTests : IntegrationFixture
    {
        [Fact]
        public void foo_()
        {

            var deletedUserId = Guid.NewGuid();
            var notDeletedUserId = Guid.NewGuid();

            this.Create_User(deletedUserId);
            this.Create_User(notDeletedUserId);

            var command = new DeleteUserCommand
            {
                Actor = new DummyUserInfo(),
                UserId = deletedUserId
            };
            // Act
            var result = Processor.Execute(command);


            // Assert
            result.IsSuccess.Should().BeTrue();

            AssertDeletedUserIsFilteredFromRegularQueries();
            AssertDeletedAndRegularUsersCanBeQueriedTogether();
            AssertOnlyDeletedUsersCanBeQueried();
            AssertDeletedUserCanBeLoadedDirectly(deletedUserId);
        }

        private void AssertOnlyDeletedUsersCanBeQueried()
        {
            var deletedUsers = Session.Query<UserProfile>().Where(x => x.IsDeleted()).ToList();
            deletedUsers.Count.Should().Be(1);
        }

        private void AssertDeletedAndRegularUsersCanBeQueriedTogether()
        {
            var usersAndDeletedUsers = Session.Query<UserProfile>().Where(x => x.MaybeDeleted()).ToList();
            usersAndDeletedUsers.Count.Should().Be(2);
        }

        private void AssertDeletedUserIsFilteredFromRegularQueries()
        {
            var users = Session.Query<UserProfile>().ToList();
            users.Count.Should().Be(1);
        }

        private void AssertDeletedUserCanBeLoadedDirectly(Guid deletedUserId)
        {
            var deletedUser = Session.Load<UserProfile>(deletedUserId);
            deletedUser.Should().NotBeNull();
        }
    }
}
