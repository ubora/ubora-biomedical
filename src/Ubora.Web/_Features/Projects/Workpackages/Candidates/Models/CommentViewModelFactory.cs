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
    public class CommentViewModelFactory
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ImageStorageProvider _imageStorageProvider;
        private readonly IAuthorizationService _authorizationService;

        public CommentViewModelFactory(IQueryProcessor queryProcessor, ImageStorageProvider imageStorageProvider, IAuthorizationService authorizationService)
        {
            _queryProcessor = queryProcessor;
            _imageStorageProvider = imageStorageProvider;
            _authorizationService = authorizationService;
        }

        protected CommentViewModelFactory()
        {
        }

        public virtual async Task<Ubora.Web._Components.Discussions.Models.CommentViewModel> Create(ClaimsPrincipal user, Ubora.Domain.Discussions.Comment comment, Guid candidateId)
        {
            var roleKeys = (string[])comment.AdditionalData["RoleKeys"];
            var isLeader = roleKeys.Any(x => x == "project-leader");
            var isMentor = roleKeys.Any(x => x == "project-mentor");
            var isEditable = (await _authorizationService.AuthorizeAsync(user, comment, Policies.CanEditComment)).Succeeded;

            var userProfile = _queryProcessor.FindById<UserProfile>(comment.UserId);

            var model = new Ubora.Web._Components.Discussions.Models.CommentViewModel
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
                CanBeEdited = isEditable
            };
            return model;
        }
    }
}
