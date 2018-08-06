using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates;
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
    public class CandidateViewModelFactoryTests
    {
        private readonly CandidateViewModel.Factory _factory;
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly Mock<CommentViewModelFactory> _commentFactory;
        private readonly Mock<IAuthorizationService> _authorizationService;

        public CandidateViewModelFactoryTests()
        {
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _commentFactory = new Mock<CommentViewModelFactory>();
            _authorizationService = new Mock<IAuthorizationService>();
            _factory = new CandidateViewModel.Factory(_imageStorageProvider.Object, _commentFactory.Object, _authorizationService.Object);
        }

        [Fact]
        public async Task Create_Returns_Expected_ViewModel()
        {
            var candidateMock = new Mock<Candidate>();
            var discussionMock = new Mock<Discussion>();

            var candidateId = Guid.NewGuid();
            candidateMock.Object.Set(x => x.Id, candidateId);
            var projectId = Guid.NewGuid();
            candidateMock.Object.Set(x => x.ProjectId, projectId);
            var imageLocation = new BlobLocation("containerName", "blobPath");
            candidateMock.Object.Set(x => x.ImageLocation, imageLocation);
            var candidateTitle = "title";
            candidateMock.Object.Set(x => x.Title, candidateTitle);
            var candidateDescription = "description";
            candidateMock.Object.Set(x => x.Description, candidateDescription);

            var comment1 = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), "comment1", DateTime.UtcNow, new Dictionary<string, object> { { "RoleKeys", "project-member" } }.ToImmutableDictionary());
            var comment2 = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), "comment2", DateTime.UtcNow, new Dictionary<string, object> { { "RoleKeys", "project-member" } }.ToImmutableDictionary());

            discussionMock.Setup(x => x.Comments).Returns(new [] { comment1, comment2 }.ToImmutableList());

            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId);

            var userVoteFunctionality = 1;
            var userVotePerformance = 2;
            var userVoteUsability = 3;
            var userVoteSafety = 5;
            var vote1 = new Vote(userId, userVoteFunctionality, userVotePerformance, userVoteUsability, userVoteSafety); // score = 11 , percentage = 20
            var vote2 = new Vote(Guid.NewGuid(), 5, 2, 5, 5); // score = 17, percentage = 20
            var vote3 = new Vote(Guid.NewGuid(), 2, 2, 1, 1); // score = 6, percentage = 20
            var vote4 = new Vote(Guid.NewGuid(), 1, 2, 1, 1); // score = 5, percentage = 40
            var vote5 = new Vote(Guid.NewGuid(), 1, 1, 1, 1); // score = 4, percentage = 40
            candidateMock.Setup(x => x.Votes).Returns(new[] { vote1, vote2, vote3, vote4, vote5 });

            var imageUrl = "imageUrl";
            _imageStorageProvider.Setup(x => x.GetUrl(candidateMock.Object.ImageLocation, ImageSize.Thumbnail400x300))
                .Returns(imageUrl);

            var comment1ViewModel = new CommentViewModel();
            _commentFactory.Setup(x => x.Create(user, comment1, candidateId))
                .ReturnsAsync(comment1ViewModel);

            var comment2ViewModel = new CommentViewModel();
            _commentFactory.Setup(x => x.Create(user, comment2, candidateId))
                .ReturnsAsync(comment2ViewModel);

            _authorizationService.Setup(x => x.AuthorizeAsync(user, candidateMock.Object, Policies.CanVoteCandidate))
                .ReturnsAsync(AuthorizationResult.Success);
            _authorizationService.Setup(x => x.AuthorizeAsync(user, candidateMock.Object, Policies.CanEditProjectCandidate))
                .ReturnsAsync(AuthorizationResult.Success);
            _authorizationService.Setup(x => x.AuthorizeAsync(user, candidateMock.Object, Policies.CanChangeProjectCandidateImage))
                .ReturnsAsync(AuthorizationResult.Success);
            _authorizationService.Setup(x => x.AuthorizeAsync(user, candidateMock.Object, Policies.CanRemoveProjectCandidateImage))
                .ReturnsAsync(AuthorizationResult.Success);
            _authorizationService.Setup(x => x.AuthorizeAsync(user, candidateMock.Object, Policies.CanRemoveCandidate))
                .ReturnsAsync(AuthorizationResult.Success);

            // Act
            var result = await _factory.Create(candidateMock.Object, discussionMock.Object, user);

            // Assert
            var expectedModel = new CandidateViewModel
            {
                Id = candidateId,
                ProjectId = projectId,
                Title = candidateTitle,
                Description = candidateDescription,
                ImageUrl = imageUrl,
                Comments = new[] {comment1ViewModel, comment2ViewModel},
                AddVoteViewModel = new AddVoteViewModel
                {
                    CandidateId = candidateId
                },
                HasImage = true,
                TotalScore = 8.6m,
                ScorePercentageVeryGood = 20,
                ScorePercentageGood = 20,
                ScorePercentageMediocre = 20,
                ScorePercentagePoor = 40,
                IsVotingAllowed = true,
                CanEditProjectCandidate = true,
                CanChangeProjectCandidateImage = true,
                CanRemoveProjectCandidateImage = true,
                CanRemoveCandidate = true,
                UserVotesViewModel = new UserVotesViewModel
                {
                    Functionality = userVoteFunctionality,
                    Performace = userVotePerformance,
                    Usability = userVoteUsability,
                    Safety = userVoteSafety
                },
                HasUserVoted = true
            };

            result.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
