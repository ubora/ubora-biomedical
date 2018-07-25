using System.IO;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public interface IWordProcessingCreationConverter
    {
        Task<Stream> GetDocumentAsync(string html);
    }
}