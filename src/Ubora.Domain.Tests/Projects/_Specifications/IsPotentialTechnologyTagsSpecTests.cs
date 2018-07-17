using FluentAssertions;
using System;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsPotentialTechnologyTagsSpecTests
    {
        [Fact]
        public void Returns_True_When_Project_Is_Software()
        {
            var potentialTechnologyTags = "Software";
            var project = new Project().Set(x => x.PotentialTechnologyTags, potentialTechnologyTags);

            // Act
            var result = new IsPotentialTechnologyTagsSpec(potentialTechnologyTags).IsSatisfiedBy(project);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_Project_Isnt_Software()
        {
            var project = new Project().Set(x => x.PotentialTechnologyTags, "");

            // Act
            var result = new IsPotentialTechnologyTagsSpec("Software").IsSatisfiedBy(project);

            // Assert
            result.Should().BeFalse();
        }
    }
}
