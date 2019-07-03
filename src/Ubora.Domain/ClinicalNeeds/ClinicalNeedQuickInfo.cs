using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Events.Projections;
using Marten.Schema;
using Ubora.Domain.ClinicalNeeds.Events;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.ClinicalNeeds
{
    public class ClinicalNeedQuickInfo
    {
        [Identity]
        public Guid ClinicalNeedId { get; private set; }
        public DateTimeOffset LastActivityAt { get; private set; }
        public int NumberOfRelatedProjects { get; private set; }
        public int NumberOfComments { get; private set; }

        public class ViewProjection : ViewProjection<ClinicalNeedQuickInfo, Guid>
        {
            // Note that there is boilerplate code of initializing a list and then adding a single id to make it possible to do these projections conditionally.
            // TODO: Instead of `.UpdatesFor<Discussion>().Single();` migrate the aggregate ID to events?
            public ViewProjection()
            {
                ProjectEvent<ClinicalNeedIndicatedEvent>(viewIdSelector: @event => @event.ClinicalNeedId, handler: (session, info, @event) =>
                {
                    info.ClinicalNeedId = @event.ClinicalNeedId;
                    info.LastActivityAt = @event.Timestamp;
                    info.NumberOfComments = 0;
                    info.NumberOfRelatedProjects = 0;
                });

                ProjectEvent<ProjectCreatedEvent>(viewIdsSelector: @event =>
                {
                    var clinicalNeedIds = new List<Guid>();
                    if (@event.RelatedClinicalNeedId.HasValue)
                    {
                        clinicalNeedIds.Add(@event.RelatedClinicalNeedId.Value);
                    }
                    return clinicalNeedIds;
                }, handler: (session, info, @event) =>
                {
                    info.NumberOfRelatedProjects++;
                });

                ProjectEvent<ProjectDeletedEvent>(viewIdsSelector: (session, @event) =>
                {
                    var clinicalNeedIds = new List<Guid>();
                    var project = session.Load<Project>(@event.ProjectId);
                    foreach (var clinicalNeedId in project.RelatedClinicalNeeds)
                    {
                        clinicalNeedIds.Add(clinicalNeedId);
                    }
                    return clinicalNeedIds;
                }, handler: (session, info, @event) =>
                {
                    info.NumberOfRelatedProjects--;
                });

                ProjectEvent<CommentAddedEvent>(viewIdsSelector: (session, @event) =>
                {
                    var clinicalNeedIds = new List<Guid>();
                    var discussion = session.PendingChanges.UpdatesFor<Discussion>().Single();
                    if (discussion.AttachedToEntity.EntityName == EntityName.ClinicalNeed)
                    {
                        clinicalNeedIds.Add(discussion.AttachedToEntity.EntityId);
                    }
                    return clinicalNeedIds;
                }, handler: (session, info, @event) =>
                {
                    info.NumberOfComments++;
                    info.LastActivityAt = @event.Timestamp;
                });

                ProjectEvent<CommentDeletedEvent>(viewIdsSelector: (session, @event) =>
                {
                    var clinicalNeedIds = new List<Guid>();
                    var discussion = session.PendingChanges.UpdatesFor<Discussion>().Single();
                    if (discussion.AttachedToEntity.EntityName == EntityName.ClinicalNeed)
                    {
                        clinicalNeedIds.Add(discussion.AttachedToEntity.EntityId);
                    }
                    return clinicalNeedIds;
                }, handler: (session, info, @event) =>
                {
                    info.NumberOfComments--;
                    info.LastActivityAt = @event.Timestamp;
                });

                ProjectEvent<CommentEditedEvent>(viewIdsSelector: (session, @event) =>
                {
                    var clinicalNeedIds = new List<Guid>();
                    var discussion = session.PendingChanges.UpdatesFor<Discussion>().Single();
                    if (discussion.AttachedToEntity.EntityName == EntityName.ClinicalNeed)
                    {
                        clinicalNeedIds.Add(discussion.AttachedToEntity.EntityId);
                    }
                    return clinicalNeedIds;
                }, handler: (session, info, @event) =>
                {
                    info.LastActivityAt = @event.Timestamp;
                });
            }
        }
    }
}