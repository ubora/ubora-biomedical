using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.ClinicalNeeds.Events;
using Ubora.Domain.Infrastructure;
using Marten.Events.Projections;
using Marten.Schema;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.ClinicalNeeds
{
    public class ClinicalNeedQuickInfo
    {
        [Identity]
        public Guid ClinicalNeedId { get; private set; }
        public DateTimeOffset LastActivityAt { get; private set; }
        public int NumberOfRelatedProjects { get; set; }
        public int NumberOfComments { get; set; }

        public class ViewProjection : ViewProjection<ClinicalNeedQuickInfo, Guid>
        {
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

                ProjectEvent<CommentAddedEvent>(viewIdsSelector: (session, @event) =>
                {
                    var clinicalNeedIds = new List<Guid>();
                    var discussion = session.PendingChanges.UpdatesFor<Discussion>().First(); // hack
                    if (discussion.AttachedToEntity.EntityName == EntityName.ClinicalNeed)
                    {
                        clinicalNeedIds.Add(discussion.AttachedToEntity.EntityId);
                    }
                    return clinicalNeedIds;
                }, handler: (session, info, @event) =>
                {
                    info.NumberOfComments++;
                });

                ProjectEvent<CommentDeletedEvent>(viewIdsSelector: (session, @event) =>
                {
                    var clinicalNeedIds = new List<Guid>();
                    var discussion = session.PendingChanges.UpdatesFor<Discussion>().First(); // hack
                    if (discussion.AttachedToEntity.EntityName == EntityName.ClinicalNeed)
                    {
                        clinicalNeedIds.Add(discussion.AttachedToEntity.EntityId);
                    }
                    return clinicalNeedIds;
                }, handler: (session, info, @event) =>
                {
                    info.NumberOfComments--;
                });
            }
        }
    }

    public class ClinicalNeed : Entity<ClinicalNeed>, ITagsAndKeywords
    {
        public Guid Id { get; private set; }
        public Guid DiscussionId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ClinicalNeedTag { get; private set; }
        public string AreaOfUsageTag { get; private set; }
        public string PotentialTechnologyTag { get; private set; }
        public string Keywords { get; private set; }
        public DateTimeOffset IndicatedAt { get; private set; }
        public Guid IndicatorUserId { get; private set; }

        private void Apply(ClinicalNeedIndicatedEvent @event)
        {
            if (IndicatedAt != default(DateTimeOffset))
                throw new InvalidOperationException();

            Id = @event.ClinicalNeedId;
            DiscussionId = @event.ClinicalNeedId;
            Title = @event.Title;
            Description = @event.Description;
            ClinicalNeedTag = @event.ClinicalNeedTag;
            AreaOfUsageTag = @event.AreaOfUsageTag;
            PotentialTechnologyTag = @event.PotentialTechnologyTag;
            Keywords = @event.Keywords;
            IndicatedAt = @event.Timestamp;
            IndicatorUserId = @event.InitiatedBy.UserId;
        }
    }
}
