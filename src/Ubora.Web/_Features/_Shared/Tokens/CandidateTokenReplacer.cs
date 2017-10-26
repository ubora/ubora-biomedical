using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Candidates;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class CandidateTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;
        private readonly HtmlEncoder _htmlEncoder;

        public CandidateTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper, HtmlEncoder htmlEncoder)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
            _htmlEncoder = htmlEncoder;
        }

        public static Regex Regex = new Regex("\\#candidate{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var candidateId = new Guid(match.Groups[1].Value);

                var candidate = _queryProcessor.FindById<Candidate>(candidateId);
                var candidatesLink = _urlHelper.Action("Candidate", "ConceptualDesign", new { projectId = candidate.ProjectId, candidateId = candidateId });

                var encodedCandidateTitle = _htmlEncoder.Encode(candidate.Title);

                return $"<a href=\"{candidatesLink}\">{encodedCandidateTitle}</a>";
            });

            return replacedText;
        }
    }
}
