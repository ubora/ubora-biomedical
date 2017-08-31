using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class TaskTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;

        public TaskTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
        }

        public static Regex Regex = new Regex("\\#task{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var taskId = new Guid(match.Groups[1].Value);

                var task = _queryProcessor.FindById<ProjectTask>(taskId);
                var tasksLink = _urlHelper.Action("Assignments", "Assignments");

                return $"<a href=\"{tasksLink}\">{task.Title}</a>";
            });

            return replacedText;
        }
    }
}
