using FluentAssertions;
using System;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Candidates.Specifications
{
    public class HasCreatedByUserIdTests
    {
        [Fact]
        public void Returns_True_When_Candidate_Has_CreatedByUserId()
        {
            var userId = Guid.NewGuid();
            var candidate = new Candidate().Set(c => c.CreatedByUserId, userId);
            var specification = new HasCreatedByUserId(userId);

            // Act
            var result = specification.IsSatisfiedBy(candidate);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_Candidate_Hasnt_CreatedByUserId()
        {
            var candidate = new Candidate().Set(c => c.CreatedByUserId, Guid.NewGuid());
            var specification = new HasCreatedByUserId(Guid.NewGuid());

            // Act
            var result = specification.IsSatisfiedBy(candidate);

            //Assert
            result.Should().BeFalse();
        }
    }
}
