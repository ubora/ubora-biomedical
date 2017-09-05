using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class ReviewTokenReplacer : ITokenReplacer
    {
        private readonly IUrlHelper _urlHelper;
        public ReviewTokenReplacer(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public static Regex Regex = new Regex(@"\#review{([0-9A-z-]+)\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var controllerName = match.Groups[1].Value;

                var reviewLink = _urlHelper.Action("Review", controllerName);

                return $"<a href=\"{reviewLink}\">review</a>";
            });

            return replacedText;
        }
    }
}
