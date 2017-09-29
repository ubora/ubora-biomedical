using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects._Events
{
    public class MentorJoinedProjectEvent : ProjectEvent
    {
        public MentorJoinedProjectEvent(UserInfo initiatedBy, Guid projectId, Guid userId) : base(initiatedBy, projectId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public override string GetDescription() => $"joined as mentor.";

        public class NotificationToInviter : GeneralNotification
        {
            public NotificationToInviter(Guid notificationTo, Guid joinerId, Guid projectId) : base(notificationTo)
            {
                JoinerId = joinerId;
                ProjectId = projectId;
            }

            public Guid JoinerId { get; set; }
            public Guid ProjectId { get; set; }

            public override string GetDescription()
            {
                return $"{StringTokens.User(JoinerId)} accepted your invitation to join project {StringTokens.Project(ProjectId)} as mentor.";
            }
        }
    }
}
