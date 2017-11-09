using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Steps
{
    public class ConceptualDesignControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly ConceptualDesignController _controller;

        public ConceptualDesignControllerTests()
        {
            //IStorageProvider ja IUboraStorageProvider
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _controller = new ConceptualDesignController(_imageStorageProvider.Object);

            SetUpForTest(_controller);
        }

        [Fact]
        public void AddCandidate_Returns_View_With_Model()
        {
            var expectedModel = new AddCandidateViewModel();

            // Act
            var result = (ViewResult)_controller.AddCandidate();

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.AddCandidate));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task AddCandidate_Returns_AddCandidate_View_With_ModelState_Error_When_Model_Is_Not_Valid()
        {
            var model = new AddCandidateViewModel();
            var errorMessage = "errorMessage";
            _controller.ModelState.AddModelError("", errorMessage);

            // Act
            var result = (ViewResult)await _controller.AddCandidate(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.AddCandidate));
            AssertModelStateContainsError(result, errorMessage);

            _imageStorageProvider.Verify(x => x.SaveImageAsync(It.IsAny<Stream>(), It.IsAny<BlobLocation>(), SizeOptions.AllDefaultSizes), Times.Never);
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task AddCandidate_Returns_AddCandidate_View_With_ModelState_Error_When_Command_Is_Not_Executed_Successfully()
        {
            var candidateId = Guid.NewGuid();
            var imageFile = new Mock<IFormFile>();
            var fileName = "fileName";
            imageFile.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var stream = Mock.Of<Stream>();
            imageFile.Setup(f => f.OpenReadStream())
                .Returns(stream);

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddCandidateCommand>()))
                .Returns(commandResult);

            var expectedBlobLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, candidateId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains(fileName) && b.BlobPath.Contains(ProjectId.ToString());

            var model = new AddCandidateViewModel
            {
                Description = "description",
                Name = "name",
                Image = imageFile.Object
            };

            // Act
            var result = (ViewResult)await _controller.AddCandidate(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.AddCandidate));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public async Task AddCandidate_Redirects_To_Voting_When_Command_Executed_Successfully()
        {
            var candidateId = Guid.NewGuid();
            var imageFile = new Mock<IFormFile>();
            var fileName = "fileName";
            imageFile.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var stream = Mock.Of<Stream>();
            imageFile.Setup(f => f.OpenReadStream())
                .Returns(stream);

            AddCandidateCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddCandidateCommand>()))
                .Callback<AddCandidateCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var expectedBlobLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, candidateId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains(fileName) && b.BlobPath.Contains(ProjectId.ToString());

            var model = new AddCandidateViewModel
            {
                Description = "description",
                Name = "name",
                Image = imageFile.Object
            };

            // Act
            var result = (RedirectToActionResult)await _controller.AddCandidate(model);

            // Assert
            result.ActionName.Should().Be(nameof(WorkpackageTwoController.Voting));
            result.ControllerName.Should().Be("WorkpackageTwo");
        }

        [Fact]
        public void Candidate_Returns_Candidate_View_With_Expected_Model()
        {
            var candidateId = Guid.NewGuid();
            var imageLocation = new BlobLocation("containerName", "blobPath");
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            candidate.Set(x => x.ImageLocation, imageLocation);

            QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
                .Returns(candidate);

            var candidateViewModel = new CandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(candidateViewModel);

            var imageUrl = "imageUrl";
            _imageStorageProvider.Setup(x => x.GetUrl(candidate.ImageLocation, ImageSize.Thumbnail400x300))
                .Returns(imageUrl);

            var expectedModel = candidateViewModel;
            expectedModel.ImageUrl = imageUrl;

            // Act
            var result = (ViewResult)_controller.Candidate(candidateId);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.Candidate));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
