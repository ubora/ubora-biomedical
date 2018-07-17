using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    public class ResourceFile
    {
        public Guid Id { get; private set; }
        public Guid ResourcePageId { get; private set; } // TODO: marten duplicate field
        public string FileName { get; private set; }
        public BlobLocation BlobLocation { get; private set; }
        public long FileSize { get; private set; } // TODO: value object?

        private void Apply(ResourceFileUploadedEvent e)
        {
            Id = e.FileId;
            ResourcePageId = e.ResourcePageId;
            FileName = e.FileName;
            FileSize = e.FileSize;
            BlobLocation = e.BlobLocation;
        }
    }
}
