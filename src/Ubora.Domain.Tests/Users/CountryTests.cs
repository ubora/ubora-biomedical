using System;
using FluentAssertions;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class CountryTests
    {
        public class TestCountry : Country
        {
        }

        [Theory]
        [InlineData("ken", "Kenya")]
        [InlineData("KEN", "Kenya")]
        [InlineData("EST", "Estonia")]
        [InlineData("", "")]
        [InlineData("falseCode", "falseCode")]
        public void DisplayName_Returns_Correct_Country_Name_Based_On_Three_Letter_Code(
            string countryCode, string expectedCountry)
        {
            var country = new TestCountry().Set(x => x.Code, countryCode);

            // Act
            var result = country.DisplayName;

            // Assert
            result.Should().Be(expectedCountry);
        }

        [Fact]
        public void Constructor_Throws_When_Unknown_Country_Code()
        {
            // Act
            Action act = () => new Country("UNKNOWN");

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
