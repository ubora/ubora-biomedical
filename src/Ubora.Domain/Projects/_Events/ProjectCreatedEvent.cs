using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Users.Queries;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectCreatedEvent : ProjectEvent 
    {
        public ProjectCreatedEvent(UserInfo initiatedBy, Guid projectId, string title, string clinicalNeed, string areaOfUsage, string potentialTechnology, string gmdn) 
            : base(initiatedBy, projectId)
        {
            Title = title;
            ClinicalNeed = clinicalNeed;
            AreaOfUsage = areaOfUsage;
            PotentialTechnology = potentialTechnology;
            Gmdn = gmdn;
        }

        public string Title { get; private set; }
        public string ClinicalNeed { get; private set; }
        public string AreaOfUsage { get; private set; }
        public string PotentialTechnology { get; private set; }
        public string Gmdn { get; private set; }

        public override string GetDescription()
        {
            return $"created project \"{StringTokens.Project(ProjectId)}\".";
        }

        public class Notifier : UboraEventNotifier<ProjectCreatedEvent>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IQueryProcessor _queryProcessor;

            public Notifier(IDocumentSession documentSession, IQueryProcessor queryProcessor)
            {
                _documentSession = documentSession;
                _queryProcessor = queryProcessor;
            }
            
            protected override void HandleCore(ProjectCreatedEvent @event, IEvent eventWithMetadata)
            {
                var notifications = _queryProcessor
                    .ExecuteQuery(new FindUboraMentorProfilesQuery())
                    .Select(mentorUserProfile => EventNotification.Create(@event, eventWithMetadata.Id, mentorUserProfile.UserId));
                
                _documentSession.StoreUboraNotificationsIfAny(notifications);
                _documentSession.SaveChanges();
            }
        }
    }
}