using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
   internal class CandidateImageDeletedEvent : ProjectEvent
    {
        public CandidateImageDeletedEvent(UserInfo initiatedBy, Guid projectId, DateTime @when, Guid candidateId)
            : base(initiatedBy, projectId)
        {
            When = when;
            CandidateId = candidateId;
        }

        public DateTime When { get; private set; }
        public Guid CandidateId { get; private set; }


        public override string GetDescription() => "deleted candidate image.";
    
    }
}
