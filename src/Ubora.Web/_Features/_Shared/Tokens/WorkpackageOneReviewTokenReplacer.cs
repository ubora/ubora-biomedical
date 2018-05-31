using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class WorkpackageOneReviewTokenReplacer : ITokenReplacer
    {
        private readonly IUrlHelper _urlHelper;

        public WorkpackageOneReviewTokenReplacer(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public static Regex Regex = new Regex("\\#review1{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var projectId = new Guid(match.Groups[1].Value);

                var reviewLink = _urlHelper.Action("Review", "WorkpackageOneReview", new { projectId });

                return $"<a href=\"{reviewLink}\">review</a>";
            });

            return replacedText;
        }
    }
}
