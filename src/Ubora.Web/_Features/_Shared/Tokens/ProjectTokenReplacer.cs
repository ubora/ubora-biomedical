using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using System.Text.Encodings.Web;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class ProjectTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;
        private readonly HtmlEncoder _htmlEncoder;

        public ProjectTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper, HtmlEncoder htmlEncoder)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
            _htmlEncoder = htmlEncoder;
        }

        public static Regex Regex = new Regex("\\#project{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var projectId = new Guid(match.Groups[1].Value);

                var project = _queryProcessor.FindById<Project>(projectId);
                var projectLink = _urlHelper.Action("Dashboard", "Dashboard", new { projectId = project.Id });

                var encodedProjectTitle = _htmlEncoder.Encode(project.Title);

                return $"<a href=\"{projectLink}\">{encodedProjectTitle}</a>";
            });

            return replacedText;
        }
    }
}