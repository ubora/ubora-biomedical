using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Web.Services;
using Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments;

namespace Ubora.Web.Infrastructure
{
    public class UniversalDocumentConverter : IWordProcessingDocumentConverter, IMarkdownConverter
    {
        private readonly PandocService _pandocService;
        private readonly INodeServices _nodeServices;

        public UniversalDocumentConverter(PandocService pandocService, INodeServices nodeServices)
        {
            _pandocService = pandocService;
            _nodeServices = nodeServices;
        }

        public async Task<Stream> GetDocumentStreamAsync(string view)
        {    
            return await _pandocService.ConvertDocumentAsync(view);
        }

        public async Task<string> GetHtmlAsync(string markdown)
        {
            return await _nodeServices.InvokeAsync<string>("./Scripts/backend/ConvertMarkdownToHtml.js", markdown);
        }
    }
}