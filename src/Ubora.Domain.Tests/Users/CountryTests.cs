using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class CountryTests
    {
        [Theory]
        [InlineData("ken", "Kenya")]
        [InlineData("KEN", "Kenya")]
        [InlineData("EST", "Estonia")]
        [InlineData("", "")]
        [InlineData("falseCode", "falseCode")]
        public void EnglishName_Returns_Correct_Country_Name_Based_On_Three_Letter_Code(
            string countryCode, string expectedCountry)
        {
            var country = new Country(code: countryCode);

            // Act
            var result = country.EnglishName;

            // Assert
            result.Should().Be(expectedCountry);
        }
    }
}
