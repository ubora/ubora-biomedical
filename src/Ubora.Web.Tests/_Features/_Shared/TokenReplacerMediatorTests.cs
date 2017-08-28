using System.Text.Encodings.Web;
using FluentAssertions;
using Moq;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class TokenReplacerMediatorTests
    {
        [Fact]
        public void Encodes_Text_And_Replaces_All_Tokens_Inside_Text()
        {
            var htmlEncoderMock = new Mock<HtmlEncoder>();

            htmlEncoderMock.Setup(x => x.Encode("initialText"))
                .Returns("encodedText");

            var firstReplacerMock = new Mock<ITokenReplacer>();
            var secondReplacerMock = new Mock<ITokenReplacer>();

            firstReplacerMock.Setup(x => x.ReplaceTokens("encodedText"))
                .Returns("secondText");

            secondReplacerMock.Setup(x => x.ReplaceTokens("secondText"))
                .Returns("expectedText");

            var replacers = new[]
            {
                firstReplacerMock.Object,
                secondReplacerMock.Object
            };

            var underTest = new TokenReplacerMediator(htmlEncoderMock.Object, replacers);

            // Act
            var result = underTest.EncodeAndReplaceAllTokens("initialText");

            // Assert
            result.ToString()
                .Should().Be("expectedText");
        }
    }
}
