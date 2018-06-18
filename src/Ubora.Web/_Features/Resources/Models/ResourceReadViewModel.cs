using System;
using System.Threading.Tasks;
using AngleSharp.Extensions;
using Ubora.Domain.Resources;

namespace Ubora.Web._Features.Resources.Models
{
    public class ResourceReadViewModel
    {
        public Guid ResourceId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }

        public class Factory
        {
            private readonly QuillDeltaToHtmlConverter _quillDeltaToHtmlConverter;

            public Factory(QuillDeltaToHtmlConverter quillDeltaToHtmlConverter)
            {
                _quillDeltaToHtmlConverter = quillDeltaToHtmlConverter;
            }
            
            protected Factory()
            {
            }
            
            public virtual async Task<ResourceReadViewModel> Create(ResourcePage resourcePage)
            {
                return new ResourceReadViewModel
                {
                    ResourceId = resourcePage.Id,
                    Body = (await _quillDeltaToHtmlConverter.ConvertQuillDeltaToHtml(resourcePage.Content.Body)).ToHtml(),
                    Title = resourcePage.Content.Title
                };
            }
        }
    }
}