﻿using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Tasks
{
    public interface IAggregateMemberEvent
    {
        Guid Id { get; }
    }

    public interface ITaskEvent : IAggregateMemberEvent
    {
    }

    public class TaskAddedEvent : UboraEvent, ITaskEvent
    {
        public TaskAddedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string GetDescription()
        {
            return $"Task added: \"{Title}\"";
        }
    }
}