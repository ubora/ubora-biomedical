﻿using System;
using Newtonsoft.Json;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Assignments.Notifications
{
    public class AssignmentAssignedToNotification : GeneralNotification
    {
        public AssignmentAssignedToNotification(Guid notificationTo, Guid requesterId, Guid projectId, Guid taskId) : base(notificationTo)
        {
            RequesterId = requesterId;
            ProjectId = projectId;
            TaskId = taskId;
        }

        [JsonConstructor]
        private AssignmentAssignedToNotification(Guid id, Guid notificationTo, DateTime createdAt, Guid requesterId, Guid projectId, Guid taskId) 
            : base(id, notificationTo, createdAt)
        {
            RequesterId = requesterId;
            ProjectId = projectId;
            TaskId = taskId;
        }

        public Guid RequesterId { get; }
        public Guid ProjectId { get; }
        public Guid TaskId { get; }

        public override string GetDescription()
        {
            return $"Assignment {StringTokens.Task(TaskId)} was assigned to you.";
        }
    }
}
