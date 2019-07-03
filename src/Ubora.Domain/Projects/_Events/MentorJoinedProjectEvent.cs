using System;
using System.Linq;
using Marten;
using Marten.Events;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Members.Specifications;

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

        public class Notifier : UboraEventNotifier<MentorJoinedProjectEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            protected override void HandleCore(MentorJoinedProjectEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications = project
                        .GetMembers(new IsLeaderSpec())
                        .GroupBy(member => member.UserId)
                        .Select(memberGrouping => EventNotification.Create(eventWithMetadata.Data, eventWithMetadata.Id, memberGrouping.Key));

                _documentSession.StoreUboraNotificationsIfAny(notifications);
                _documentSession.SaveChanges();
            }
        }

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
