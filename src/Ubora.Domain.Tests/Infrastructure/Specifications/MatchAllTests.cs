using FluentAssertions;
using Ubora.Domain.Infrastructure.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Infrastructure.Specifications
{
    public class MatchAllTests
    {

        [Fact]
        public void Satisfies_All_And_Allways()
        {
            // Act
            var result = new MatchAll<object>().IsSatisfiedBy(new object());

            // Assert
            result.Should().BeTrue();
        }
    }
}
