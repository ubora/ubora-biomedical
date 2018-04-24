using System;
using Newtonsoft.Json;
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

            [JsonConstructor]
            private NotificationToInviter(Guid id, Guid notificationTo, DateTime createdAt, Guid joinerId, Guid projectId) 
                : base(id, notificationTo, createdAt)
            {
                JoinerId = joinerId;
                ProjectId = projectId;
            }

            public Guid JoinerId { get; }
            public Guid ProjectId { get; }

            public override string GetDescription()
            {
                return $"{StringTokens.User(JoinerId)} accepted your invitation to join project {StringTokens.Project(ProjectId)} as mentor.";
            }
        }
    }
}
