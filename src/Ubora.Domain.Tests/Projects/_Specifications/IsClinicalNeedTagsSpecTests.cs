using FluentAssertions;
using System;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsClinicalNeedTagsSpecTests
    {
        [Fact]
        public void Returns_True_When_Project_Is_Rehabilitation()
        {
            var clinicalNeedTags = "Rehabilitation";
            var project = new Project().Set(x => x.ClinicalNeedTag, clinicalNeedTags);

            // Act
            var result = new IsClinicalNeedTagsSpec(clinicalNeedTags).IsSatisfiedBy(project);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_Project_Isnt_Rehabilitation()
        {
            var project = new Project().Set(x => x.ClinicalNeedTag, "");

            // Act
            var result = new IsClinicalNeedTagsSpec("Rehabilitation").IsSatisfiedBy(project);

            // Assert
            result.Should().BeFalse();
        }
    }
}
