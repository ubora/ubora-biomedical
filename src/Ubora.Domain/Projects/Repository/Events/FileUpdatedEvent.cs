using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository.Events
{
    /// <remarks>
    /// Note that this event did not have "file name" property before 4/24/2018 so there are some events in "ubora-kahawa" that have this property as NULL.
    /// </remarks>
    public class FileUpdatedEvent : UboraFileEvent, IFileEvent
    {
        public FileUpdatedEvent(UserInfo initiatedBy, Guid projectId, Guid id, string fileName, BlobLocation location, string comment, long fileSize, int revisionNumber) 
            : base(initiatedBy, projectId, id, fileName, location, comment, fileSize, revisionNumber)
        {
        }

        public override string GetDescription()
        {
            return $"updated file \"{StringTokens.File(Id)}\"";
        }
    }
}
