using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Users;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Projects.Workpackages.Candidates;
using Xunit;
using Ubora.Domain.Discussions;
using Ubora.Web._Components.Discussions.Models;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Candidates
{
    public class CommentViewModelFactoryTests
    {
        private readonly CommentViewModelFactory _factory;
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly Mock<IQueryProcessor> _queryProcessor;
        private readonly Mock<IAuthorizationService> _authorizationService;

        public CommentViewModelFactoryTests()
        {
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _queryProcessor = new Mock<IQueryProcessor>();
            _authorizationService = new Mock<IAuthorizationService>();
            _factory = new CommentViewModelFactory(_queryProcessor.Object, _imageStorageProvider.Object, _authorizationService.Object);
        }

        [Fact]
        public async Task Create_Returns_Expected_ViewModel()
        {
            var userId = Guid.NewGuid();
            var commentText = "commentText";
            var commentedAt = DateTime.UtcNow;
            var commentId = Guid.NewGuid();
            var comment = Comment.Create(commentId, userId, commentText, commentedAt, new Dictionary<string, object> { { "RoleKeys", new [] { "project-mentor" } } }.ToImmutableDictionary());

            var projectId = Guid.NewGuid();
            var candidateId = Guid.NewGuid();
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            candidate.Set(x => x.ProjectId, projectId);
            _queryProcessor.Setup(x => x.FindById<Candidate>(candidateId))
                .Returns(candidate);

            var user = new UserProfile(userId);
            user.Set(x => x.FirstName, "FirstName");
            user.Set(x => x.LastName, "LastName");
            var profilePictureBlobLocation = new BlobLocation("containerName", "blobPath");
            user.Set(x => x.ProfilePictureBlobLocation, profilePictureBlobLocation);
            _queryProcessor.Setup(x => x.FindById<UserProfile>(userId))
                .Returns(user);

            var profilePictureUrl = "profilePictureUrl";
            _imageStorageProvider.Setup(x => x.GetUrl(profilePictureBlobLocation))
                .Returns(profilePictureUrl);

            var claimsPrincipal = FakeClaimsPrincipalFactory.CreateAuthenticatedUser();
            _authorizationService.Setup(x => x.AuthorizeAsync(claimsPrincipal, comment, Policies.CanEditCandidateComment))
                .ReturnsAsync(AuthorizationResult.Success);

            var expectedModel = new CommentViewModel
            {
                Id = commentId,
                CommentatorId = userId,
                CommentText = commentText,
                CommentatorName = "FirstName LastName",
                ProfilePictureUrl = profilePictureUrl,
                CommentedAt = commentedAt,
                CanBeEdited = true,
                IsLeader = false,
                IsMentor = true
            };

            // Act
            var result = await _factory.Create(claimsPrincipal, comment, candidateId);

            // Assert
            result.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
