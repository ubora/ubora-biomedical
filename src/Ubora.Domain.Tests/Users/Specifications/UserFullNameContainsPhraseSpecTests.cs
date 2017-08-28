using FluentAssertions;
using System;
using Ubora.Domain.Tests.Helper;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Users.Specifications
{
    public class UserFullNameContainsPhraseSpecTests
    {
        [Fact]
        public void Returns_True_When_User_FullName_Consists_Search_Phrase()
        {
            var userProfile = new UserProfile(Guid.NewGuid());
            userProfile.SetPropertyValue(nameof(UserProfile.FirstName), "FirstName");
            userProfile.SetPropertyValue(nameof(UserProfile.LastName), "LastName");

            var searchPhrase = "na";
            var specification = new UserFullNameContainsPhraseSpec(searchPhrase);

            // Act
            var result = specification.IsSatisfiedBy(userProfile);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_User_FullName_Does_Not_Consist_Search_Phrase()
        {
            var userProfile = new UserProfile(Guid.NewGuid());
            userProfile.SetPropertyValue(nameof(UserProfile.LastName), "LastName");
            userProfile.SetPropertyValue(nameof(UserProfile.FirstName), "FirstName");

            var searchPhrase = "no";
            var specification = new UserFullNameContainsPhraseSpec(searchPhrase);

            // Act
            var result = specification.IsSatisfiedBy(userProfile);

            // Assert
            result.Should().BeFalse();
        }
    }
}
