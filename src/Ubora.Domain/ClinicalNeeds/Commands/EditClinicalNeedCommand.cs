using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.ClinicalNeeds.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.ClinicalNeeds.Commands
{
    public class EditClinicalNeedCommand : UserCommand, ITagsAndKeywords
    {
        public Guid ClinicalNeedId { get; set; }

        public string Title { get; set; }
        public QuillDelta Description { get; set; }

        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }

        public class Handler : ICommandHandler<EditClinicalNeedCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(EditClinicalNeedCommand cmd)
            {
                var clinicalNeed = _documentSession.LoadOrThrow<ClinicalNeed>(cmd.ClinicalNeedId);

                var events = new List<object>();

                if (cmd.Title != clinicalNeed.Title)
                {
                    events.Add(new ClinicalNeedTitleEditedEvent(cmd.Actor, cmd.Title));
                }

                if (cmd.Description != clinicalNeed.Description)
                {
                    events.Add(new ClinicalNeedDescriptionEditedEvent(cmd.Actor, cmd.Description));
                }

                if (clinicalNeed.AreaOfUsageTag != cmd.AreaOfUsageTag
                    || clinicalNeed.ClinicalNeedTag != cmd.ClinicalNeedTag
                    || clinicalNeed.PotentialTechnologyTag != cmd.PotentialTechnologyTag
                    || clinicalNeed.Keywords != cmd.Keywords)
                {
                    events.Add(new ClinicalNeedDesignTagsEditedEvent(cmd.Actor, cmd.ClinicalNeedTag, cmd.AreaOfUsageTag, cmd.PotentialTechnologyTag, cmd.Keywords));
                }

                if (events.Any())
                {
                    _documentSession.Events.Append(clinicalNeed.Id, events.ToArray());
                    _documentSession.SaveChanges();
                }

                return CommandResult.Success;
            }
        }
    }
}