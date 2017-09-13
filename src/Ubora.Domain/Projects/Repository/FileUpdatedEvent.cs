﻿using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileUpdatedEvent : UboraFileEvent, IFileEvent
    {
        public FileUpdatedEvent(Guid id, Guid projectId, BlobLocation location, string comment, long fileSize, UserInfo initiatedBy) 
            : base(id, projectId, location, comment, fileSize, initiatedBy)
        {
        }

        public override string GetDescription()
        {
            return $"Updated file [{Id}]";
        }
    }
}
