using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileAddedEvent : UboraEvent, IFileEvent
    {
        public FileAddedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }

        public override string GetDescription()
        {
            return $"Added file \"{FileName}\"";
        }
    }
}
