using System;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;
using Moq;
using AutoMapper;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Infrastructure;
using FluentAssertions;
using Ubora.Web.Tests.Fakes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Authorization;
using Ubora.Web.Services;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Steps
{
    public class CandidateViewModelFactoryTests
    {
        private readonly CandidateViewModel.Factory _factory;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly Mock<CommentViewModel.Factory> _commentFactory;
        private readonly Mock<IAuthorizationService> _authorizationService;

        public CandidateViewModelFactoryTests()
        {
            _mapper = new Mock<IMapper>();
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _commentFactory = new Mock<CommentViewModel.Factory>();
            _authorizationService = new Mock<IAuthorizationService>();
            _factory = new CandidateViewModel.Factory(_mapper.Object, _imageStorageProvider.Object, _commentFactory.Object, _authorizationService.Object);
        }

        [Fact]
        public async Task Create_Returns_Expected_ViewModel()
        {
            var candidateId = Guid.NewGuid();
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            var projectId = Guid.NewGuid();
            candidate.Set(x => x.ProjectId, projectId);
            var imageLocation = new BlobLocation("containerName", "blobPath");
            candidate.Set(x => x.ImageLocation, imageLocation);

            var comment1 = new Comment(Guid.NewGuid(), "comment1", Guid.NewGuid(), DateTime.UtcNow, new[] { "project-member" });
            var comment2 = new Comment(Guid.NewGuid(), "comment2", Guid.NewGuid(), DateTime.UtcNow, new[] { "project-member" });
            candidate.Set(x => x.Comments, new [] { comment1, comment2 });

            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId);

            var userVoteFunctionality = 1;
            var userVotePerformance = 2;
            var userVoteUsability = 3;
            var userVoteSafety = 5;
            var vote1 = new Vote(userId, userVoteFunctionality, userVotePerformance, userVoteUsability, userVoteSafety); // score = 11 , percentage = 20
            var vote2 = new Vote(Guid.NewGuid(), 5, 2, 3, 5); // score = 16, percentage = 20
            var vote3 = new Vote(Guid.NewGuid(), 2, 2, 1, 1); // score = 6, percentage = 20
            var vote4 = new Vote(Guid.NewGuid(), 1, 2, 1, 1); // score = 5, percentage = 40
            var vote5 = new Vote(Guid.NewGuid(), 1, 1, 1, 1); // score = 4, percentage = 40
            candidate.Set(x => x.Votes, new[] { vote1, vote2, vote3, vote4, vote5 });

            var model = new CandidateViewModel();
            _mapper.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(model);

            var imageUrl = "imageUrl";
            _imageStorageProvider.Setup(x => x.GetUrl(candidate.ImageLocation, ImageSize.Thumbnail400x300))
                .Returns(imageUrl);


            var comment1ViewModel = new CommentViewModel();
            _commentFactory.Setup(x => x.Create(user, comment1, candidateId))
                .ReturnsAsync(comment1ViewModel);

            var comment2ViewModel = new CommentViewModel();
            _commentFactory.Setup(x => x.Create(user, comment2, candidateId))
                .ReturnsAsync(comment2ViewModel);

            _authorizationService.Setup(x => x.AuthorizeAsync(user, candidate, Policies.CanVoteCandidate))
                .ReturnsAsync(AuthorizationResult.Success);

            // Act
            var result = await _factory.Create(candidate, user);

            // Assert
            var expectedModel = model;
            expectedModel.ImageUrl = imageUrl;
            expectedModel.Comments = new[] { comment1ViewModel, comment2ViewModel };
            expectedModel.AddCommentViewModel = new AddCommentViewModel
            {
                CandidateId = candidateId
            };
            expectedModel.AddVoteViewModel = new AddVoteViewModel
            {
                CandidateId = candidateId
            };
            expectedModel.ScorePercentageVeryGood = 40;
            expectedModel.ScorePercentageGood = 20;
            expectedModel.ScorePercentageMediocre = 20;
            expectedModel.ScorePercentagePoor = 20;
            expectedModel.IsVotingAllowed = true;

            expectedModel.UserVotesViewModel = new UserVotesViewModel
            {
                Functionality = userVoteFunctionality,
                Performace = userVotePerformance,
                Usability = userVotePerformance,
                Safety = userVoteSafety
            };

            result.Should().Be(expectedModel);
        }
    }
}
