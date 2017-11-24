using System;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;
using Moq;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Infrastructure;
using FluentAssertions;
using Ubora.Web.Tests.Fakes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Authorization;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Steps
{
    public class CandidateViewModelFactoryTests
    {
        private readonly CandidateViewModel.Factory _factory;
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly Mock<CommentViewModel.Factory> _commentFactory;
        private readonly Mock<IAuthorizationService> _authorizationService;

        public CandidateViewModelFactoryTests()
        {
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _commentFactory = new Mock<CommentViewModel.Factory>();
            _authorizationService = new Mock<IAuthorizationService>();
            _factory = new CandidateViewModel.Factory( _imageStorageProvider.Object, _commentFactory.Object, _authorizationService.Object);
        }

        [Fact]
        public async Task Create_Returns_Expected_ViewModel()
        {
            var candidateMock = new Mock<Candidate>();

            var candidateId = Guid.NewGuid();
            candidateMock.Object.Set(x => x.Id, candidateId);
            var projectId = Guid.NewGuid();
            candidateMock.Object.Set(x => x.ProjectId, projectId);
            var imageLocation = new BlobLocation("containerName", "blobPath");
            candidateMock.Object.Set(x => x.ImageLocation, imageLocation);
            var candidateTitle = "title";
            candidateMock.Object.Set(x => x.Title, candidateTitle);
            var candidateDescription = "description";
            candidateMock.Object.Set(x => x.Description,candidateDescription);

            var comment1 = new Comment(Guid.NewGuid(), "comment1", Guid.NewGuid(), DateTime.UtcNow, new[] { "project-member" });
            var comment2 = new Comment(Guid.NewGuid(), "comment2", Guid.NewGuid(), DateTime.UtcNow, new[] { "project-member" });
            candidateMock.Setup(x => x.Comments).Returns(new [] { comment1, comment2 });

            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId);

            var userVoteFunctionality = 1;
            var userVotePerformance = 2;
            var userVoteUsability = 3;
            var userVoteSafety = 5;
            var vote1 = new Vote(userId, userVoteFunctionality, userVotePerformance, userVoteUsability, userVoteSafety); // score = 11 , percentage = 20
            var vote2 = new Vote(Guid.NewGuid(), 5, 2, 5, 5); // score = 16, percentage = 20
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

            // Act
            var result = await _factory.Create(candidateMock.Object, user);

            // Assert
            var expectedModel = new CandidateViewModel
            {
                Id = candidateId,
                ProjectId = projectId,
                Title = candidateTitle,
                Description = candidateDescription,
                ImageUrl = imageUrl,
                Comments = new[] {comment1ViewModel, comment2ViewModel},
                AddCommentViewModel = new AddCommentViewModel
                {
                    CandidateId = candidateId
                },
                AddVoteViewModel = new AddVoteViewModel
                {
                    CandidateId = candidateId
                },
                ScorePercentageVeryGood = 20,
                ScorePercentageGood = 20,
                ScorePercentageMediocre = 20,
                ScorePercentagePoor = 40,
                IsVotingAllowed = true,
                UserVotesViewModel = new UserVotesViewModel
                {
                    Functionality = userVoteFunctionality,
                    Performace = userVotePerformance,
                    Usability = userVoteUsability,
                    Safety = userVoteSafety
                }
            };

            result.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
