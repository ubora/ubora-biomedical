using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateRemovedEvent : ProjectEvent
    {
        public CandidateRemovedEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription()
        {
            return "deleted candidate.";
        }
    }
}
