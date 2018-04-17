using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Members.Specifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateAddedEvent : ProjectEvent
    {
        public CandidateAddedEvent(UserInfo initiatedBy, Guid projectId, Guid id, string title, string description, BlobLocation imageLocation) : base(initiatedBy, projectId)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageLocation = imageLocation;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public BlobLocation ImageLocation { get; private set; }

        public override string GetDescription()
        {
            return $"added project candidate \"{StringTokens.Candidate(Id)}\"";
        }

        public class Notifier : UboraEventNotifier<CandidateAddedEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            protected override void HandleCore(CandidateAddedEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var notifications =
                    project
                        .GetMembers(!new HasUserIdSpec(@event.InitiatedBy.UserId))
                        .GroupBy(member => member.UserId)
                        .Select(memberGrouping => EventNotification.Create(@event, eventWithMetadata.Id, memberGrouping.Key));

                _documentSession.StoreUboraNotificationsIfAny(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}