﻿using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileHiddenEvent : UboraEvent, IFileEvent
    {
        public FileHiddenEvent(UserInfo initiatedBy, Guid id) : base(initiatedBy)
        {
            Id = id;
        }

        public Guid Id { get; private set;} 

        public override string GetDescription()
        {
            return $"Removed file [{Id}]";
        }
    }
}
