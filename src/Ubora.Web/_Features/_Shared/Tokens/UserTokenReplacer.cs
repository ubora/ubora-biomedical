using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class UserTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;

        public UserTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
        }

        public static Regex Regex = new Regex("\\#user{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var userId = new Guid(match.Groups[1].Value);

                var userProfile = _queryProcessor.FindById<UserProfile>(userId);
                var profileLink = _urlHelper.Action("View", "Profile", new { userId = userProfile.UserId });

                return $"<a href=\"{profileLink}\">{userProfile.FullName}</a>";
            });

            return replacedText;
        }
    }
}