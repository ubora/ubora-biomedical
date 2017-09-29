using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    internal class ProjectImageUpdatedEvent : ProjectEvent
    {
        public ProjectImageUpdatedEvent(UserInfo initiatedBy, Guid projectId, DateTime @when, BlobLocation blobLocation) : base(initiatedBy, projectId)
        {
            When = when;
            BlobLocation = blobLocation;
        }

        public DateTime When { get; private set; }
        public BlobLocation BlobLocation { get; private set; }

        public override string GetDescription() => "updated project image.";
    }
}
