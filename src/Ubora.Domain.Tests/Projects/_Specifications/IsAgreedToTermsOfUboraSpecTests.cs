using System;
using FluentAssertions;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsAgreedToTermsOfUboraSpecTests
    {
        [Fact]
        public void Returns_TRUE_When_Project_IS_Agreed_To_Terms_Of_UBORA()
        {
            var project = new Project().Set(x => x.IsAgreedToTermsOfUbora, true);

            // Act
            var result = new IsAgreedToTermsOfUboraSpec().IsSatisfiedBy(project);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_FALSE_When_Project_NOT_Is_Agreed_To_Terms_Of_UBORA()
        {
            var project = new Project().Set(x => x.IsAgreedToTermsOfUbora, false);

            // Act
            var result = new IsAgreedToTermsOfUboraSpec().IsSatisfiedBy(project);

            // Assert
            result.Should().BeFalse();
        }
    }
}
