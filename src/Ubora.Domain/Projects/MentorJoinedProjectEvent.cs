using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects
{
    public class MentorJoinedProjectEvent : UboraEvent
    {
        public MentorJoinedProjectEvent(Guid projectId, Guid userId, UserInfo initiatedBy) : base(initiatedBy)
        {
            ProjectId = projectId;
            UserId = userId;
        }

        public Guid ProjectId { get; private set; }
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
