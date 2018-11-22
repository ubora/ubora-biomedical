using System;
using System.Collections.Immutable;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Discussions.Events
{
    public class DiscussionOpenedEvent : UboraEvent
    {
        public Guid DiscussionId { get; }
        public AttachedToEntity AttachedToEntity { get; }
        public ImmutableDictionary<string, object> AdditionalDiscussionData { get; }

        public DiscussionOpenedEvent(UserInfo initiatedBy, Guid discussionId, AttachedToEntity attachedToEntity, ImmutableDictionary<string, object> additionalDiscussionData) : base(initiatedBy)
        {
            DiscussionId = discussionId;
            AttachedToEntity = attachedToEntity;
            AdditionalDiscussionData = additionalDiscussionData;
        }

        public override string GetDescription() => "opened discussion.";
    }
}