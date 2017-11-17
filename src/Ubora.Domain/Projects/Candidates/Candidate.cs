using System;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates.Events;
using Ubora.Domain.Projects._Events;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Domain.Projects.Candidates
{
    public class Candidate
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public BlobLocation ImageLocation { get; private set; }

        [JsonIgnore]
        public bool HasImage => ImageLocation != null;

        [JsonProperty(nameof(Comments))]
        private readonly HashSet<Comment> _comments = new HashSet<Comment>();
        [JsonIgnore]
        // Virtual for testing.
        public virtual IReadOnlyCollection<Comment> Comments
        {
            get
            {
                return _comments;
            }
            private set { }
        }

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
        private void Apply(CandidateImageDeletedEvent e)
        {
            ImageLocation = null;
        }

        private void Apply(CandidateCommentAddedEvent e)
        {
            var comment = new Comment(e.InitiatedBy.UserId, e.CommentText);
            _comments.Add(comment);
        }
    }
}
