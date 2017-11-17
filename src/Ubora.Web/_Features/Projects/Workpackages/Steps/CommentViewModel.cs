using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid CommentatorId { get; set; }
        public string CommentatorName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentedAt { get; set; }
        public DateTime LastEditedAt { get; set; }
        public bool IsLeader { get; set; }
        public bool IsMentor { get; set; }
        public EditCommentViewModel EditCommentViewModel { get; set; }

        public DateTime CommentEditedAt
        {
            get
            {
                return LastEditedAt == null ? CommentedAt : LastEditedAt;
            }
        }

        public bool IsEdited
        {
            get
            {
                return LastEditedAt != null;
            }
        }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly ImageStorageProvider _imageStorageProvider;

            public Factory(IQueryProcessor queryProcessor, ImageStorageProvider imageStorageProvider)
            {
                _queryProcessor = queryProcessor;
                _imageStorageProvider = imageStorageProvider;
            }

            protected Factory()
            {
            }

            public virtual CommentViewModel Create(Comment comment, Guid candidateId)
            {
                var candidate = _queryProcessor.FindById<Candidate>(candidateId);
                var project = _queryProcessor.FindById<Project>(candidate.ProjectId);
                var isLeader = project.Members.Any(x => x.UserId == comment.UserId && x.IsLeader);
                var isMentor = project.Members.Any(x => x.UserId == comment.UserId && x.IsMentor);

                var userProfile = _queryProcessor.FindById<UserProfile>(comment.UserId);

                var editCommentViewModel = new EditCommentViewModel
                {
                    Id = comment.Id,
                    CandidateId = candidate.Id,
                    CommentText = comment.Text
                };

                var model = new CommentViewModel
                {
                    Id = comment.Id,
                    CommentatorId = comment.UserId,
                    CommentText = comment.Text,
                    CommentatorName = userProfile.FullName,
                    ProfilePictureUrl = _imageStorageProvider.GetDefaultOrBlobUrl(userProfile),
                    IsLeader = isLeader,
                    IsMentor = isMentor,
                    CommentedAt = comment.CommentedAt,
                    LastEditedAt = comment.LastEditedAt,
                    EditCommentViewModel = editCommentViewModel
                };

                return model;
            }
        }
    }
}
