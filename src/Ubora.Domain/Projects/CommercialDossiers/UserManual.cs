using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.CommercialDossiers
{
    public class UserManual 
    {
        public UserManual(BlobLocation location, string fileName, long fileSize)
        {
            Location = location;
            FileName = fileName;
            FileSize = fileSize;
        }

        public BlobLocation Location { get; }
        public string FileName { get; }
        public long FileSize { get; }
    }
}