using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._SortSpecifications;
using Xunit;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Tests.Projects._SortSpecifications
{
    public class SortByCreatedDateTimeSpecficationTests
    {
        [Theory]
        [InlineData(SortOrder.Ascending)]
        [InlineData(SortOrder.Descending)]
        public void Sorts_By_CreatedDateTime(SortOrder sortOrder)
        {
            var project1 = new Project().Set(x => x.CreatedDateTime, new DateTime(year: 2017, month: 12, day: 12, hour: 12, minute: 0, second: 0));
            var project2 = new Project().Set(x => x.CreatedDateTime, new DateTime(year: 2017, month: 11, day: 12, hour: 12, minute: 0, second: 0));
            var project3 = new Project().Set(x => x.CreatedDateTime, new DateTime(year: 2017, month: 12, day: 10, hour: 12, minute: 0, second: 0));

            var projects = new List<Project>()
            {
                project1,
                project2,
                project3,
            };

            var sut = new SortByCreatedDateTimeSpecfication(sortOrder);

            //Act
            var sortedResult = sut.Sort(projects.AsQueryable());

            //Assert
            if (sortOrder == SortOrder.Ascending)
            {
                sortedResult.Should().Equal(project2, project3, project1);
            }
            else
            {
                sortedResult.Should().Equal(project1, project3, project2);
            }
        }
    }
}
