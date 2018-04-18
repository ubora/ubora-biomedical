using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.Projects.Workpackages.Candidates;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Candidates
{
    public class CandidateItemViewModelFactoryTests
    {
        private readonly CandidateItemViewModel.Factory _candidateItemViewModelFactory;
        private readonly Mock<ImageStorageProvider> _imageStorageProviderMock;
        private readonly Mock<IMapper> _automapper;

        public CandidateItemViewModelFactoryTests()
        {
            _imageStorageProviderMock = new Mock<ImageStorageProvider>();
            _automapper = new Mock<IMapper>();
            _candidateItemViewModelFactory = new CandidateItemViewModel.Factory(_imageStorageProviderMock.Object, _automapper.Object);
        }

        [Fact]
        public void Create_Returns_Expected_CandidateItemViewModel()
        {
            var candidate = new Candidate();
            candidate.Set(x => x.ImageLocation, new BlobLocation("containerName", "blobPath"));
            var candidateItemViewModel = new CandidateItemViewModel()
            {
                Description = "Description",
                Id = Guid.NewGuid(),
                Title = "Title"
            };
            _automapper.Setup(x => x.Map<CandidateItemViewModel>(candidate))
                .Returns(candidateItemViewModel);

            var imageUrl = "imageUrl";
            _imageStorageProviderMock.Setup(x => x.GetUrl(candidate.ImageLocation, ImageSize.Thumbnail400x300))
                .Returns(imageUrl);

            // Act
            var result = _candidateItemViewModelFactory.Create(candidate);

            // Assert
            candidateItemViewModel.ImageUrl = imageUrl;
            result.ShouldBeEquivalentTo(candidateItemViewModel);
        }
    }
}
