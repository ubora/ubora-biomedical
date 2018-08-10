using System;
using Marten;
using Ubora.Domain.ClinicalNeeds.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.ClinicalNeeds.Commands
{
    public class IndicateClinicalNeedCommand : UserCommand, ITagsAndKeywords
    {
        public Guid ClinicalNeedId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

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
                var @event = new ClinicalNeedIndicatedEvent(
                    initiatedBy: cmd.Actor,
                    clinicalNeedId: cmd.ClinicalNeedId,
                    title: cmd.Title,
                    description: cmd.Description,
                    clinicalNeedTag: cmd.ClinicalNeedTag,
                    areaOfUsageTag: cmd.AreaOfUsageTag,
                    potentialTechnologyTag: cmd.PotentialTechnologyTag, 
                    keywords: cmd.Keywords);

                _documentSession.Events.Append(cmd.ClinicalNeedId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
