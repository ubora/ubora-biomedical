using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates.Events;

namespace Ubora.Domain.Projects.Candidates
{
    public class Candidate
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public BlobLocation ImageLocation { get; private set; }

        private void Apply(CandidateAddedEvent e)
        {
            Id = e.Id;
            ProjectId = e.ProjectId;
            Title = e.Title;
            Description = e.Description;
            ImageLocation = e.ImageLocation;
        }

        private void Apply(CandidateEditedEvent e)
        {
            Title = e.Title;
            Description = e.Description;
        }

        private void Apply(CandidateImageEditedEvent e)
        {
            ImageLocation = e.ImageLocation;
        }
    }
}
