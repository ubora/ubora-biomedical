using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class WorkpackageTwoReviewTokenReplacer : ITokenReplacer
    {
        private readonly IUrlHelper _urlHelper;
        public WorkpackageTwoReviewTokenReplacer(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public static Regex Regex = new Regex("\\#review2");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var reviewLink = _urlHelper.Action("Review", "WorkpackageTwoReview");

                return $"<a href=\"{reviewLink}\">review</a>";
            });

            return replacedText;
        }
    }
}
