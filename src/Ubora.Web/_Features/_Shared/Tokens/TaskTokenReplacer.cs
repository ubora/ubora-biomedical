using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class TaskTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;
        private readonly HtmlEncoder _htmlEncoder;

        public TaskTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper, HtmlEncoder htmlEncoder)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
            _htmlEncoder = htmlEncoder;
        }

        public static Regex Regex = new Regex("\\#task{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var taskId = new Guid(match.Groups[1].Value);

                var task = _queryProcessor.FindById<ProjectTask>(taskId);
                var tasksLink = _urlHelper.Action("Edit", "Assignments", new { projectId = task.ProjectId, id = task.Id });

                var encodedTaskTitle = _htmlEncoder.Encode(task.Title);

                return $"<a href=\"{tasksLink}\">{encodedTaskTitle}</a>";
            });

            return replacedText;
        }
    }
}
