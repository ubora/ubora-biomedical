using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Members.Specifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageFourOpenedEvent : ProjectEvent
    {
        public WorkpackageFourOpenedEvent(Guid deviceStructuredInformationId, UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
        {
            DeviceStructuredInformationId = deviceStructuredInformationId;
        }

        public Guid DeviceStructuredInformationId { get; private set; }
        
        public override string GetDescription() => $"opened work package 4: Implementation.";
        
        public class Notifier : UboraEventNotifier<WorkpackageFourOpenedEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(WorkpackageFourOpenedEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications =
                    project
                        .GetMembers(!new HasUserIdSpec(@event.InitiatedBy.UserId))
                        .GroupBy(member => member.UserId)
                        .Select(memberGrouping => EventNotification.Create(eventWithMetadata.Data, eventWithMetadata.Id, memberGrouping.Key));
                
                _documentSession.StoreUboraNotificationsIfAny(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}