using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class ProjectTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;

        public ProjectTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
        }

        public static Regex Regex = new Regex("\\#project{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var projectId = new Guid(match.Groups[1].Value);

                var project = _queryProcessor.FindById<Project>(projectId);
                var projectLink = _urlHelper.Action("Dashboard", "Dashboard", new { projectId = project.Id });

                return $"<a href=\"{projectLink}\">{project.Title}</a>";
            });

            return replacedText;
        }
    }
}