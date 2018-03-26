using System;
using FluentAssertions;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsDraftSpecTests
    {
        [Fact]
        public void Returns_TRUE_When_Project_In_Draft()
        {
            var project = new Project().Set(x => x.IsInDraft, true);

            // Act
            var result = new IsDraftSpec().IsSatisfiedBy(project);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_FALSE_When_Project_NOT_In_Draft()
        {
            var project = new Project().Set(x => x.IsInDraft, false);

            // Act
            var result = new IsDraftSpec().IsSatisfiedBy(project);

            // Assert
            result.Should().BeFalse();
        }
    }
}
