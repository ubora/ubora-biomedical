using FluentAssertions;
using System;
using Ubora.Domain.Tests.Helper;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Users.Specifications
{
    public class UserEmailContainsPhraseSpecTests
    {
        [Fact]
        public void Returns_True_When_User_Email_Consists_Search_Phrase()
        {
            var userProfile = new UserProfile(Guid.NewGuid());
            userProfile.SetPropertyValue(nameof(UserProfile.Email), "tEst@agileworks.eu");

            var searchPhrase = "es";
            var specification = new UserEmailContainsPhraseSpec(searchPhrase);

            // Act
            var result = specification.IsSatisfiedBy(userProfile);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_User_Email_Does_Not_Consist_Search_Phrase()
        {
            var userProfile = new UserProfile(Guid.NewGuid());
            userProfile.SetPropertyValue(nameof(UserProfile.Email), "test@agileworks.eu");

            var searchPhrase = "as";
            var specification = new UserEmailContainsPhraseSpec(searchPhrase);

            // Act
            var result = specification.IsSatisfiedBy(userProfile);

            // Assert
            result.Should().BeFalse();
        }
    }
}
