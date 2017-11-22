using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Text.Encodings.Web;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class CandidateTokenReplacerTests
    {
        private readonly CandidateTokenReplacer _candidateTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<HtmlEncoder> _htmlEncoderMock;

        public CandidateTokenReplacerTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _htmlEncoderMock = new Mock<HtmlEncoder>();
            _candidateTokenReplacer = new CandidateTokenReplacer(_queryProcessorMock.Object, _urlHelperMock.Object, _htmlEncoderMock.Object);
        }

        [Fact]
        public void Replaces_Candidate_Tokens_With_Anchor_Tags()
        {
            var projectId = Guid.NewGuid();
            var candidate1Title = "candidate1Title";
            var candidate1 = new Candidate()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, candidate1Title)
                .Set(x => x.ProjectId, projectId);

            var candidate2Title = "candidate2Title";
            var candidate2 = new Candidate()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.Title, candidate2Title)
                .Set(x => x.ProjectId, projectId);

            var text = $"test1 {StringTokens.Candidate(candidate1.Id)} test2 {StringTokens.Candidate(candidate2.Id)} test3";

            _queryProcessorMock.Setup(x => x.FindById<Candidate>(candidate1.Id))
                .Returns(candidate1);

            _queryProcessorMock.Setup(x => x.FindById<Candidate>(candidate2.Id))
                .Returns(candidate2);

            var candidate1Link = "candidate1Link";
            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Candidate" && x.Controller == "ConceptualDesign"
                    && x.Values.GetPropertyValue<Guid>("projectId") == projectId
                    && x.Values.GetPropertyValue<Guid>("candidateId") == candidate1.Id)))
                .Returns(candidate1Link);

            var candidate2Link = "candidate2Link";
            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "Candidate" && x.Controller == "ConceptualDesign"
                     && x.Values.GetPropertyValue<Guid>("projectId") == projectId
                    && x.Values.GetPropertyValue<Guid>("candidateId") == candidate2.Id)))
                .Returns(candidate2Link);

            var encodedCandidate1Title = "encodedCandidate1Title";
            _htmlEncoderMock.Setup(x => x.Encode(candidate1.Title))
                .Returns(encodedCandidate1Title);

            var encodedCandidate2Title = "encodedCandidate2Title";
            _htmlEncoderMock.Setup(x => x.Encode(candidate2.Title))
                .Returns(encodedCandidate2Title);

            // Act
            var result = _candidateTokenReplacer.ReplaceTokens(text);

            // Assert
            var expected = $"test1 <a href=\"{candidate1Link}\">{encodedCandidate1Title}</a> test2 <a href=\"{candidate2Link}\">{encodedCandidate2Title}</a> test3";

            result.Should().Be(expected);
        }

        [Fact]
        public void Works_Without_Tokens_In_String()
        {
            // Act
            var result = _candidateTokenReplacer.ReplaceTokens("text");

            // Assert
            result.Should().Be("text");
        }
    }
}
