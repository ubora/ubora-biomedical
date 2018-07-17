using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._SortSpecifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._SortSpecifications
{
    public class SortByMultipleSpecificationTests
    {
        [Fact]
        public void Sorts_By_Multiple()
        {
            var project1 = new Project()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.CreatedDateTime, new DateTime(year: 2017, month: 12, day: 12, hour: 12, minute: 0, second: 0))
                .Set(x => x.Title, "First");
            var project2 = new Project()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.CreatedDateTime, new DateTime(year: 2017, month: 11, day: 12, hour: 12, minute: 0, second: 0))
                .Set(x => x.Title, "Secound");
            var project3 = new Project()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.CreatedDateTime, new DateTime(year: 2017, month: 12, day: 10, hour: 12, minute: 0, second: 0))
                .Set(x => x.Title, "Last");
            var project4 = new Project()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.CreatedDateTime, new DateTime(year: 2017, month: 12, day: 10, hour: 12, minute: 0, second: 0))
                .Set(x => x.Title, "Last2");

            var projects = new List<Project>()
            {
                project1,
                project2,
                project3,
                project4
            };

            var sortSpecifications = new List<ISortSpecification<Project>>();
            sortSpecifications.Add(new SortByCreatedDateTimeSpecfication(SortOrder.Ascending));
            sortSpecifications.Add(new SortByTitleSpecification(SortOrder.Ascending));

            var sut = new SortByMultipleSpecification<Project>(sortSpecifications);

            //Act
            var sortedResult = sut.Sort(projects.AsQueryable());

            //Assert
            sortedResult.Should().Equal(project1, project3, project4, project2);
        }
    }
}
