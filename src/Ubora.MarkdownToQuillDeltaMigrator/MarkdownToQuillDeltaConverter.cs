using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain;
using Ubora.Domain.Projects._Commands;

namespace Ubora.MarkdownToQuillDeltaMigrator
{
    public class MarkdownToQuillDeltaConverter : IMarkdownToQuillDeltaConverter
    {
        private readonly INodeServices _nodeServices;

        public MarkdownToQuillDeltaConverter(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }

        public QuillDelta Convert(string markdown)
        {
            return ConvertAsync(markdown).GetAwaiter().GetResult();
        }

        public async Task<QuillDelta> ConvertAsync(string markdown)
        {
            var quillDeltaAsString = await _nodeServices.InvokeAsync<string>("./index.js", markdown ?? "");
            return new QuillDelta(quillDeltaAsString);
        }
    }
}
