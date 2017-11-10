using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateImageEditedEvent : ProjectEvent
    {
        public CandidateImageEditedEvent(UserInfo initiatedBy, Guid projectId, Guid id, BlobLocation imageLocation) : base(initiatedBy, projectId)
        {
            Id = id;
            ImageLocation = imageLocation;
        }

        public Guid Id { get; internal set; }
        public BlobLocation ImageLocation { get; private set; }

        public override string GetDescription()
        {
            return $"edited project candidate \"{StringTokens.Candidate(Id)}\" image";
        }
    }
}
