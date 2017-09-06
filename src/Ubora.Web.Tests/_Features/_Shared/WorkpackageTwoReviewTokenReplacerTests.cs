using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Ubora.Domain;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class WorkpackageTwoReviewTokenReplacerTests
    {
        private readonly WorkpackageTwoReviewTokenReplacer _reviewTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;

        public WorkpackageTwoReviewTokenReplacerTests()
        {
            _urlHelperMock = new Mock<IUrlHelper>();
            _reviewTokenReplacer = new WorkpackageTwoReviewTokenReplacer(_urlHelperMock.Object);
        }

        [Fact]
        public void Replaces_Review_Tokens_With_Review_Link()
        {
            var text = $"test1 {StringTokens.WorkpackageTwoReview()} test2";

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Review" && x.Controller == "WorkpackageTwoReview")))
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
