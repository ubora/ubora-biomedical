using System.Threading.Tasks;

namespace Ubora.Domain.Resources
{
    public interface IResourceBlobDeleter
    {
        Task DeleteBlobContainerOfResourcePage(ResourcePage resourcePage);
        Task DeleteBlobOfResourceFile(ResourceFile resourceFile);
    }
}
