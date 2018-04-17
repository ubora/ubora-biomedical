using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectDeletedEvent : ProjectEvent
    {
        public ProjectDeletedEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription() => "deleted project";
        
        public class Notifier : UboraEventNotifier<ProjectDeletedEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(ProjectDeletedEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications =
                    project.Members
                        .Where(x => x.UserId != @event.InitiatedBy.UserId)
                        .Select(projectMember => EventNotification.Create(@event, eventWithMetadata.Id, projectMember.UserId));
                
                _documentSession.StoreObjects(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}
