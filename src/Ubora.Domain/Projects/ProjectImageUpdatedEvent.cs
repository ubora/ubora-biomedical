using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    internal class ProjectImageUpdatedEvent : UboraEvent
    {
        public DateTime When { get; private set; }
        public BlobLocation BlobLocation { get; private set; }

        public ProjectImageUpdatedEvent(BlobLocation blobLocation, DateTime when, UserInfo initiatedBy) : base(initiatedBy)
        {
            When = when;
            BlobLocation = blobLocation;
        }

        public override string GetDescription() => "Updated project image.";
    }
}
