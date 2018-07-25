using System.IO;
using System.Threading.Tasks;
using Ubora.Web.Services;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web._Features._Shared.Documents;

namespace Ubora.Web.Infrastructure
{
    public class UniversalDocumentConverter : IWordProcessingCreationConverter
    {
        private readonly PandocService _pandocService;
        private readonly ViewRender _viewRender;

        public UniversalDocumentConverter(ViewRender viewRender, PandocService pandocService)
        {
            _viewRender = viewRender;
            _pandocService = pandocService;
        }

        public async Task<Stream> GetDocumentAsync(string description)
        {
            var viewModel = new WP1TemplateViewModel {Description = description};
            var view = _viewRender.Render("/_Features/_Shared/Documents/", "WP1Template.cshtml", viewModel);

            var response = await _pandocService.ConvertDocumentAsync(view);
            Stream documentStream = await response.Content.ReadAsStreamAsync();
            
            return documentStream;
        }
    }
}