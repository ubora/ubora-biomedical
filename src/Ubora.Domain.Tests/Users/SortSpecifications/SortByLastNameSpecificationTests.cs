using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Users;
using Ubora.Domain.Users.SortSpecifications;
using Xunit;

namespace Ubora.Domain.Tests.Users.SortSpecifications
{
    public class SortByLastNameSpecificationTests
    {
        [Theory]
        [InlineData(SortOrder.Ascending)]
        [InlineData(SortOrder.Descending)]
        public void Sorts_By_FirstName(SortOrder sortOrder)
        {
            var userProfile1 = new UserProfile(Guid.NewGuid()).Set(x => x.LastName, "A");
            var userProfile2 = new UserProfile(Guid.NewGuid()).Set(x => x.LastName, "B");
            var userProfile3 = new UserProfile(Guid.NewGuid()).Set(x => x.LastName, "C");

            var userProfiles = new List<UserProfile>()
            {
                userProfile1,
                userProfile2,
                userProfile3
            };

            var sut = new SortByLastNameSpecification(sortOrder);

            //Act
            var sortedResult = sut.Sort(userProfiles.AsQueryable());

            //Assert
            if (sortOrder == SortOrder.Ascending)
            {
                sortedResult.Should().Equal(userProfile1, userProfile2, userProfile3);
            }
            else
            {
                sortedResult.Should().Equal(userProfile3, userProfile2, userProfile1);
            }
        }
    }
}
