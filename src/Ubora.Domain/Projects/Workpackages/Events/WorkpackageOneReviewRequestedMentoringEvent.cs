using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users.Queries;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneReviewRequestedMentoringEvent : ProjectEvent
    {
        public WorkpackageOneReviewRequestedMentoringEvent(UserInfo initiatedBy, Guid projectId)
            : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription() => $"requested mentoring.";

        public class Notifier : UboraEventNotifier<WorkpackageOneReviewRequestedMentoringEvent>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IQueryProcessor _queryProcessor;

            public Notifier(IDocumentSession documentSession, IQueryProcessor queryProcessor)
            {
                _documentSession = documentSession;
                _queryProcessor = queryProcessor;
            }

            protected override void HandleCore(WorkpackageOneReviewRequestedMentoringEvent @event, IEvent eventWithMetadata)
            {
                var project = _documentSession.Load<Project>(@event.ProjectId);

                var managementGroupUsers = _queryProcessor.ExecuteQuery(new FindUboraManagementGroupQuery());
                var notificationsTomanagementGroupUsers = managementGroupUsers
                    .Select(managementGroupUser => EventNotification.Create(eventWithMetadata.Data, eventWithMetadata.Id, managementGroupUser.UserId));
                _documentSession.StoreUboraNotificationsIfAny(notificationsTomanagementGroupUsers);

                var admins = _queryProcessor
                    .ExecuteQuery(new FindUboraAdministratorsQuery());
                var notificationsToAdmins = admins
                        .Select(admin => EventNotification.Create(eventWithMetadata.Data, eventWithMetadata.Id, admin.UserId));

                _documentSession.StoreUboraNotificationsIfAny(notificationsToAdmins);
                _documentSession.SaveChanges();
            }
        }
    }
}
