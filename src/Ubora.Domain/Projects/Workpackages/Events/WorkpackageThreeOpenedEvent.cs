using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageThreeOpenedEvent : ProjectEvent
    {
        public WorkpackageThreeOpenedEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription() => $"opened work package 3: Design and prototyping.";
        
        public class Notifier : EventNotifier<WorkpackageThreeOpenedEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(WorkpackageThreeOpenedEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications =
                    project.Members
                        .Where(x => x.UserId != @event.InitiatedBy.UserId)
                        .Select(projectMember => EventNotification.Create(eventWithMetadata, projectMember.UserId));
                
                _documentSession.StoreObjects(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}
