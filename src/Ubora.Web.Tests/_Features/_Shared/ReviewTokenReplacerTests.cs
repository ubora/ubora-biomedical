using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Ubora.Domain;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class ReviewTokenReplacerTests
    {
        private readonly ReviewTokenReplacer _reviewTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;

        public ReviewTokenReplacerTests()
        {
            _urlHelperMock = new Mock<IUrlHelper>();
            _reviewTokenReplacer = new ReviewTokenReplacer(_urlHelperMock.Object);
        }

        [Fact]
        public void Replaces_Project_Tokens_With_Anchor_Tags()
        {
            var controller = "controller";
            var text = $"test1 {StringTokens.Review(controller)} test2";

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Review" && x.Controller == controller)))
                .Returns("reviewLink");

            // Act
            var result = _reviewTokenReplacer.ReplaceTokens(text);

            // Assert
            var expected = "test1 <a href=\"reviewLink\">review</a> test2";

            result.Should().Be(expected);
        }

        [Fact]
        public void Works_Without_Tokens_In_String()
        {
            // Act
            var result = _reviewTokenReplacer.ReplaceTokens("text");

            // Assert
            result.Should().Be("text");
        }
    }
}
