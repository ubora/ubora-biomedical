using System;
using System.Collections.Immutable;
using Marten;
using Ubora.Domain.ClinicalNeeds.Events;
using Ubora.Domain.Discussions;
using Ubora.Domain.Discussions.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.ClinicalNeeds.Commands
{
    public class IndicateClinicalNeedCommand : UserCommand, ITagsAndKeywords
    {
        public Guid ClinicalNeedId { get; set; }

        public string Title { get; set; }
        public QuillDelta Description { get; set; }

        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }

        public class Handler : ICommandHandler<IndicateClinicalNeedCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(IndicateClinicalNeedCommand cmd)
            {
                var clinicalNeedIndicatedEvent = new ClinicalNeedIndicatedEvent(
                    initiatedBy: cmd.Actor,
                    clinicalNeedId: cmd.ClinicalNeedId,
                    title: cmd.Title,
                    description: cmd.Description,
                    clinicalNeedTag: cmd.ClinicalNeedTag,
                    areaOfUsageTag: cmd.AreaOfUsageTag,
                    potentialTechnologyTag: cmd.PotentialTechnologyTag,
                    keywords: cmd.Keywords);

                var discussionOpenedEvent = new DiscussionOpenedEvent(
                    initiatedBy: cmd.Actor,
                    discussionId: cmd.ClinicalNeedId,
                    attachedToEntity: new AttachedToEntity(EntityName.ClinicalNeed, cmd.ClinicalNeedId),
                    additionalDiscussionData: ImmutableDictionary<string, object>.Empty);

                _documentSession.Events.Append(cmd.ClinicalNeedId, clinicalNeedIndicatedEvent, discussionOpenedEvent);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
