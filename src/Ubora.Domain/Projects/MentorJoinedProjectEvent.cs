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

        public override string GetDescription() => $"{InitiatedBy.Name} joined as mentor.";

        public class NotificationToInviter : GeneralNotification
        {
            public NotificationToInviter(Guid notificationTo, Guid declinerUserId, Guid projectId) : base(notificationTo)
            {
                DeclinerUserId = declinerUserId;
                ProjectId = projectId;
            }

            public Guid DeclinerUserId { get; set; }
            public Guid ProjectId { get; set; }

            public override string GetDescription()
            {
                return $"{Template.User(DeclinerUserId)} accepted your invitation to join project {Template.Project(ProjectId)} as mentor.";
            }
        }
    }
}
