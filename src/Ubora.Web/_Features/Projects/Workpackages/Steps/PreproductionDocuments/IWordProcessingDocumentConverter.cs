using System.IO;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public interface IWordProcessingDocumentConverter
    {
        Task<Stream> GetDocumentStreamAsync(string html);
    }
}