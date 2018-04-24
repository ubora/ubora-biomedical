using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources
{
    public class ResourceFileUploadedEvent : UboraEvent
    {
        public ResourceFileUploadedEvent(UserInfo initiatedBy, Guid fileId, BlobLocation blobLocation, string fileName, long fileSize) 
            : base(initiatedBy)
        {
            FileId = fileId;
            BlobLocation = blobLocation;
            FileName = fileName;
            FileSize = fileSize;
        }
        
        public Guid FileId { get; }
        public BlobLocation BlobLocation { get; }
        public string FileName { get; }
        public long FileSize { get; }
        
        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}