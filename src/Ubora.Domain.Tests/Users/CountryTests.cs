using FluentAssertions;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Users
{
    public class CountryTests
    {
        [Fact]
        public void EnglishName_Returns_Correct_Country_Name_Based_On_Three_Letter_Code()
        {
            var country = new Country(code: "ken");

            // Act
            var result = country.EnglishName;

            // Assert
            result.Should().Be("Kenya");
        }
    }
}
