using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain;

namespace Ubora.Web.Infrastructure
{
    public class QuillDeltaTransformer
    {
        private readonly INodeServices _nodeServices;

        public QuillDeltaTransformer(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }

        public QuillDeltaTransformer()
        {
        }

        public virtual async Task<string> SanitizeQuillDeltaForEditing(QuillDelta quillDelta)
        {
            return await _nodeServices.InvokeAsync<string>("./Scripts/backend/SanitizeQuillDelta.js", quillDelta?.Value ?? new QuillDelta().Value);
        }

        public virtual async Task<string> ConvertQuillDeltaToHtml(QuillDelta quillDelta)
        {
            return await _nodeServices.InvokeAsync<string>("./Scripts/backend/ConvertQuillDeltaToHtml.js", quillDelta?.Value);
        }
    }
}
