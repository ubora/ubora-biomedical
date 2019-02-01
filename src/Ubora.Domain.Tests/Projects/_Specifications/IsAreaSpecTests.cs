using FluentAssertions;
using System;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsAreaSpecTests
    {
        [Fact]
        public void Returns_True_When_Project_Is_Colorectal_surgery()
        {
            var area = "Colorectal surgery";
            var project = new Project().Set(x => x.AreaOfUsageTag, area);

            // Act
            var result = new IsAreaSpec(area).IsSatisfiedBy(project);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_Project_Isnt_Colorectal_surgery()
        {
            var project = new Project().Set(x => x.AreaOfUsageTag, "");

            // Act
            var result = new IsAreaSpec("Colorectal surgery").IsSatisfiedBy(project);

            // Assert
            result.Should().BeFalse();
        }
    }
}
