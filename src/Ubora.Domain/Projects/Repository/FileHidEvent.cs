using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileHidEvent : UboraEvent, IFileEvent
    {
        public FileHidEvent(UserInfo initiatedBy, Guid id, string fileName) : base(initiatedBy)
        {
            Id = id;
            FileName = fileName;
        }

        public Guid Id { get; private set; }
        public string FileName { get; private set; }

        public override string GetDescription()
        {
            return $"Soft removed file \"{FileName}\"";
        }
    }
}
