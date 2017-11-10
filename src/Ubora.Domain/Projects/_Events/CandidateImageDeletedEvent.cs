using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class CandidateImageDeletedEvent : ProjectEvent
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
