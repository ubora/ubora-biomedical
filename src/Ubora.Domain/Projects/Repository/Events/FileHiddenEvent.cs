using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Repository.Events
{
    public class FileHiddenEvent : ProjectEvent, IFileEvent
    {
        public FileHiddenEvent(UserInfo initiatedBy, Guid projectId, Guid id) : base(initiatedBy, projectId)
        {
            Id = id;
        }

        public Guid Id { get; private set;} 

        public override string GetDescription()
        {
            return $"removed file [{StringTokens.File(Id)}]";
        }
    }
}
