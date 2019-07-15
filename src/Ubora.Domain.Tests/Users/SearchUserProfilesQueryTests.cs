using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class SearchUserProfilesQueryTests : IntegrationFixture
    {
        [Fact]
        public void Returns_UserProfiles_Where_FullName_Contains_Search_Term()
        {
            this.Create_User(Guid.NewGuid(), firstName: "apple1", lastName: "gReEn");
            this.Create_User(Guid.NewGuid(), firstName: "orange1", lastName: "RED");
            this.Create_User(Guid.NewGuid(), firstName: "orange2", lastName: "Red");
            this.Create_User(Guid.NewGuid(), firstName: "apple2", lastName: "green");

            this.Create_User(Guid.NewGuid(), firstName: "joi", lastName: "ned");

            this.Create_User(Guid.NewGuid(), firstName: "a_order", lastName: "green");
            this.Create_User(Guid.NewGuid(), firstName: "c_order", lastName: "green");
            this.Create_User(Guid.NewGuid(), firstName: "b_order", lastName: "green");


            // Act
            var emptyResult = this.Processor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = "notInTheName",
                Paging = new Paging(1, 1)
            });
            var allResult = this.Processor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = ""
            });
            var allResult2 = this.Processor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = null
            });
            var foundResult = this.Processor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = "red",
                Paging = new Paging(1, 4)
            });
            var foundResult2 = this.Processor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = "green",
                Paging = new Paging(2, 2)
            });
            var joinedResult = this.Processor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = "joi ned",
            });
            var orderResult = this.Processor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = "order",
            });

            // Assert
            emptyResult.TotalItemCount.Should().Be(0);
            emptyResult.PageSize.Should().Be(1);

            allResult.TotalItemCount.Should().Be(8);
            allResult2.TotalItemCount.Should().Be(8);

            foundResult.TotalItemCount.Should().Be(2);
            foundResult.PageSize.Should().Be(4);

            foundResult2.TotalItemCount.Should().Be(5);
            foundResult2.PageNumber.Should().Be(2);

            joinedResult.Single().FullName.Should().Be("joi ned");

            orderResult.TotalItemCount.Should().Be(3);
            orderResult.First().FirstName.Should().Be("a_order");
            orderResult.Last().FirstName.Should().Be("c_order");
        }
    }
}
