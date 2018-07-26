using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public interface IMarkdownConverter
    {
        Task<string> GetHtmlAsync(string markdown);
    }
}