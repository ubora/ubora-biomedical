using System;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;
using Moq;
using AutoMapper;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Infrastructure;
using FluentAssertions;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Steps
{
    public class CandidateViewModelFactoryTests
    {
        private readonly CandidateViewModel.Factory _factory;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly Mock<CommentViewModel.Factory> _commentFactory;

        public CandidateViewModelFactoryTests()
        {
            _mapper = new Mock<IMapper>();
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _commentFactory = new Mock<CommentViewModel.Factory>();
            _factory = new CandidateViewModel.Factory(_mapper.Object, _imageStorageProvider.Object, _commentFactory.Object);
        }

        [Fact]
        public void Create_Returns_Expected_ViewModel()
        {
            var candidateId = Guid.NewGuid();
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            var projectId = Guid.NewGuid();
            candidate.Set(x => x.ProjectId, projectId);
            var imageLocation = new BlobLocation("containerName", "blobPath");
            candidate.Set(x => x.ImageLocation, imageLocation);

            var comment1 = new Comment(Guid.NewGuid(), "comment1");
            var comment2 = new Comment(Guid.NewGuid(), "comment2");
            candidate.Set(x => x.Comments, new [] { comment1, comment2 });

            var candidateViewModel = new CandidateViewModel();
            _mapper.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(candidateViewModel);

            var imageUrl = "imageUrl";
            _imageStorageProvider.Setup(x => x.GetUrl(candidate.ImageLocation, ImageSize.Thumbnail400x300))
                .Returns(imageUrl);

            var expectedModel = candidateViewModel;
            expectedModel.ImageUrl = imageUrl;

            var comment1ViewModel = new CommentViewModel();
            _commentFactory.Setup(x => x.Create(comment1, projectId))
                .Returns(comment1ViewModel);

            var comment2ViewModel = new CommentViewModel();
            _commentFactory.Setup(x => x.Create(comment2, projectId))
                .Returns(comment2ViewModel);

            expectedModel.Comments = new[] { comment1ViewModel, comment2ViewModel };

            expectedModel.AddCommentViewModel = new AddCommentViewModel
            {
                CandidateId = candidateId
            };

            // Act
            var result = _factory.Create(candidate);

            // Assert
            result.Should().Be(expectedModel);
        }
    }
}
