using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Users;
using Ubora.Domain.Users.SortSpecifications;
using Xunit;

namespace Ubora.Domain.Tests.Users.SortSpecifications
{
    public class SortByMultipleUserProfileSortSpecificationTests
    {
        [Fact]
        public void Sorts_By_Multiple()
        {
            var userProfile1 = new UserProfile(Guid.NewGuid()).Set(x => x.FirstName, "A");
            var userProfile2 = new UserProfile(Guid.NewGuid()).Set(x => x.FirstName, "B");
            var userProfile3 = new UserProfile(Guid.NewGuid()).Set(x => x.FirstName, "C");

            var userProfiles = new List<UserProfile>()
            {
                userProfile1,
                userProfile2,
                userProfile3
            };

            var sortSpecifications = new List<ISortSpecification<UserProfile>>();
            sortSpecifications.Add(new SortByFirstNameSpecification(SortOrder.Descending));

            var sut = new SortByMultipleUserProfileSortSpecification(sortSpecifications);

            //Act
            var sortedResult = sut.Sort(userProfiles.AsQueryable());

            //Assert
            sortedResult.Should().Equal(userProfile3, userProfile2, userProfile1);
        }
    }
}
