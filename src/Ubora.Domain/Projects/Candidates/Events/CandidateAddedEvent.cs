using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateAddedEvent : ProjectEvent
    {
        public CandidateAddedEvent(UserInfo initiatedBy, Guid projectId, Guid id, string title, string description, BlobLocation imageLocation) : base(initiatedBy, projectId)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageLocation = imageLocation;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public BlobLocation ImageLocation { get; private set; }

        public override string GetDescription()
        {
            return $"added project candidate \"{StringTokens.Candidate(Id)}\"";
        }
    }
}
