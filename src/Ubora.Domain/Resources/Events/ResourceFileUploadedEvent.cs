using Newtonsoft.Json;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourceFileUploadedEvent : UboraEvent, IResourceFileEvent
    {
        public ResourceFileUploadedEvent(UserInfo initiatedBy, Guid fileId, Guid resourcePageId, BlobLocation blobLocation, string fileName, long fileSize)
            : base(initiatedBy)
        {
            FileId = fileId;
            ResourcePageId = resourcePageId;
            BlobLocation = blobLocation;
            FileName = fileName;
            FileSize = fileSize;
        }

        public Guid FileId { get; }
        public Guid ResourcePageId { get; }
        public BlobLocation BlobLocation { get; }
        public string FileName { get; }
        public long FileSize { get; }

        [JsonIgnore]
        Guid IAggregateMemberEvent.Id => FileId;

        public override string GetDescription() => "File uploaded.";
    }
}