using System;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Assignments.Notifications
{
    public class AssignmentRemovedFromNotification : GeneralNotification
    {
        public AssignmentRemovedFromNotification(Guid notificationTo, Guid requesterId, Guid projectId, Guid taskId) : base(notificationTo)
        {
            RequesterId = requesterId;
            ProjectId = projectId;
            TaskId = taskId;
        }

        public Guid RequesterId { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid TaskId { get; private set; }

        public override string GetDescription()
        {
            return $"Assignment {StringTokens.Task(TaskId)} was removed from you.";
        }
    }
}
