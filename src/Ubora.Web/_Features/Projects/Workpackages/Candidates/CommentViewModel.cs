using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Users;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
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
        public bool CanBeEdited { get; set; }
        public EditCommentViewModel EditCommentViewModel { get; set; }

        public DateTime CommentEditedAt
        {
            get
            {
                return LastEditedAt > CommentedAt ? LastEditedAt : CommentedAt;
            }
        }

        public bool IsEdited
        {
            get
            {
                return LastEditedAt != null && LastEditedAt != DateTime.MinValue;
            }
        }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly ImageStorageProvider _imageStorageProvider;
            private readonly IAuthorizationService _authorizationService;

            public Factory(IQueryProcessor queryProcessor, ImageStorageProvider imageStorageProvider, IAuthorizationService authorizationService)
            {
                _queryProcessor = queryProcessor;
                _imageStorageProvider = imageStorageProvider;
                _authorizationService = authorizationService;
            }

            protected Factory()
            {
            }

            public virtual async Task<CommentViewModel> Create(ClaimsPrincipal user, Comment comment, Guid candidateId)
            {
                var candidate = _queryProcessor.FindById<Candidate>(candidateId);
                var isLeader = comment.RoleKeys.Any(x => x == "project-leader");
                var isMentor = comment.RoleKeys.Any(x => x == "project-mentor");
                var isEditable = (await _authorizationService.AuthorizeAsync(user, comment, Policies.CanEditComment)).Succeeded;

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
                    EditCommentViewModel = editCommentViewModel,
                    CanBeEdited = isEditable
                };

                return model;
            }
        }
    }
}
