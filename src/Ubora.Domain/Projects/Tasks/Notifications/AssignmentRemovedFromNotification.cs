using System;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Tasks.Notifications
{
    public class AssignmentRemovedFromNotification : GeneralNotification
    {
        public AssignmentRemovedFromNotification(Guid notificationTo, Guid requesterId, Guid projectId, Guid taskId) : base(notificationTo)
        {
            RequesterId = requesterId;
            ProjectId = projectId;
            TaskId = taskId;
        }

        public Guid RequesterId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }

        public override string GetDescription()
        {
            return $"An assignment {StringTokens.Task(TaskId)} was removed from you!";
        }
    }
}
