using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Ubora.Domain;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class WorkpackageOneReviewTokenReplacerTests
    {
        private readonly WorkpackageOneReviewTokenReplacer _reviewTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;

        public WorkpackageOneReviewTokenReplacerTests()
        {
            _urlHelperMock = new Mock<IUrlHelper>();
            _reviewTokenReplacer = new WorkpackageOneReviewTokenReplacer(_urlHelperMock.Object);
        }

        [Fact]
        public void Replaces_Project_Tokens_With_Anchor_Tags()
        {
            var text = $"test1 {StringTokens.WorkpackageOneReview()} test2";

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Review" && x.Controller == "WorkpackageOneReview")))
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
