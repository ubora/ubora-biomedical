﻿using System;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Members.Specifications;
using Ubora.Domain.Projects.StructuredInformations.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageOneReviewAcceptedEvent : ProjectEvent, IDeviceStructuredInformationEvent
    {
        public WorkpackageOneReviewAcceptedEvent(
            UserInfo initiatedBy, 
            Guid projectId, 
            string concludingComment, 
            DateTimeOffset acceptedAt,
            Guid deviceStructuredInformationId) 
            : base(initiatedBy, projectId)
        {
            DeviceStructuredInformationId = (deviceStructuredInformationId == default(Guid)) ? projectId : deviceStructuredInformationId; // Warning: backwards-compatibility (ubora-kahawa)
            ConcludingComment = concludingComment;
            AcceptedAt = acceptedAt;
        }

        public string ConcludingComment { get; private set; }
        public DateTimeOffset AcceptedAt { get; private set; }
        public Guid DeviceStructuredInformationId { get; }

        public override string GetDescription() => $"accepted work package 1 by {StringTokens.WorkpackageOneReview(ProjectId)}.";

        public class Notifier : UboraEventNotifier<WorkpackageOneReviewAcceptedEvent>
        {
            private readonly IDocumentSession _documentSession;

            public Notifier(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            protected override void HandleCore(WorkpackageOneReviewAcceptedEvent @event, IEvent eventWithMetadata)
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