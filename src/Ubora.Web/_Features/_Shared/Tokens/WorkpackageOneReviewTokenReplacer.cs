using Microsoft.AspNetCore.Mvc;
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

        public static Regex Regex = new Regex("\\#review1");

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
