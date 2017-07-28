using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileHidEvent : UboraEvent, IFileEvent
    {
        public FileHidEvent(UserInfo initiatedBy, Guid id) : base(initiatedBy)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public override string GetDescription()
        {
            return $"Removed file [{Id}]";
        }
    }
}
