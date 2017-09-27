using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class FileTokenReplacer : ITokenReplacer
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUrlHelper _urlHelper;
        private readonly HtmlEncoder _htmlEncoder;

        public FileTokenReplacer(IQueryProcessor queryProcessor, IUrlHelper urlHelper, HtmlEncoder htmlEncoder)
        {
            _queryProcessor = queryProcessor;
            _urlHelper = urlHelper;
            _htmlEncoder = htmlEncoder;
        }

        public static Regex Regex = new Regex("\\#file{([0-9A-f-]+)\\}");

        public string ReplaceTokens(string text)
        {
            var replacedText = Regex.Replace(text, match =>
            {
                var fileId = new Guid(match.Groups[1].Value);

                var file = _queryProcessor.FindById<ProjectFile>(fileId);

                var encodedFileName = _htmlEncoder.Encode(file.FileName);

                return $"{encodedFileName}";
            });

            return replacedText;
        }
    }
}
