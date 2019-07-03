using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.CommercialDossiers.Events
{
    public class UserManualChangedEvent : CommercialDossierEventBase
    {
        public UserManualChangedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, BlobLocation location, string fileName, long fileSize)
            : base(initiatedBy, projectId, aggregateId)
        {
            Location = location;
            FileName = fileName;
            FileSize = fileSize;
        }

        public BlobLocation Location { get; }
        public string FileName { get; }
        public long FileSize { get; }

        public override string GetDescription() => "uploaded new user manual.";
    }
}