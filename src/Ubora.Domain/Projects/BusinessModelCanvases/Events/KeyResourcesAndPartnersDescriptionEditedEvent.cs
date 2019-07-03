using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.BusinessModelCanvases.Events
{
    public class KeyResourcesAndPartnersDescriptionEditedEvent : BusinessModelCanvasDescriptionEditedEventBase
    {
        public KeyResourcesAndPartnersDescriptionEditedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, QuillDelta value)
            : base(initiatedBy, projectId, aggregateId, value)
        {
        }

        public override string GetDescription() => "edited on business model canvas the description of key resources and partners.";
    }
}