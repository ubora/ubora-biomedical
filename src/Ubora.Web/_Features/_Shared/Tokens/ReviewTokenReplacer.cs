using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class ReviewTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;
        public ReviewTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
        }

        public static Regex Regex = new Regex("\\#review");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var reviewLink = _urlHelper.Action("Review", "WorkpackageOneReview");

                return $"<a href=\"{reviewLink}\">review</a>";
            });

            return replacedText;
        }
    }
}
