using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain.Resources;

namespace Ubora.Web._Features.Resources
{
    public class QuillDeltaToHtmlConverter
    {
        private readonly INodeServices _nodeServices;

        public QuillDeltaToHtmlConverter(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }

        protected QuillDeltaToHtmlConverter()
        {
        }

        public virtual async Task<IHtmlDocument> ConvertQuillDeltaToHtml(QuillDelta quillDelta)
        {
            var htmlString = await _nodeServices.InvokeExportAsync<string>("./Scripts/app-backend", "convertQuillDeltaToHtml", quillDelta.Value);
            var html = await new HtmlParser().ParseAsync(htmlString);
            return html;
        }
    }
}