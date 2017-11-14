using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web.Tests.Helper;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Steps
{
    public class ConceptualDesignControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly ConceptualDesignController _controller;

        public ConceptualDesignControllerTests()
        {
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _controller = new ConceptualDesignController(_imageStorageProvider.Object);

            SetUpForTest(_controller);
        }

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var methodPolicies = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
            {
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(ConceptualDesignController.AddCandidate),
                    Policies = new []{ nameof(Policies.CanAddProjectCandidate) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(ConceptualDesignController.EditCandidate),
                    Policies = new []{ nameof(Policies.CanEditProjectCandidate) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(ConceptualDesignController.EditCandidateImage),
                    Policies = new []{ nameof(Policies.CanChangeProjectCandidateImage) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(ConceptualDesignController.RemoveCandidateImage),
                    Policies = new []{ nameof(Policies.CanRemoveProjectCandidateImage) }
                }
            };

            AssertHasAuthorizeAttributes(typeof(ConceptualDesignController), methodPolicies);
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

            var comment1 = new Comment(UserId, "comment1");
            var comment2 = new Comment(Guid.NewGuid(), "comment2");
            candidate.Set(x => x.Comments, new List<Comment> { comment1, comment2 });

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

            var commentModelFactory = new Mock<CommentViewModel.Factory>();
            var comment1ViewModel = new CommentViewModel();
            commentModelFactory.Setup(x => x.Create(comment1))
                .Returns(comment1ViewModel);

            var comment2ViewModel = new CommentViewModel();
            commentModelFactory.Setup(x => x.Create(comment2))
                .Returns(comment2ViewModel);

            expectedModel.Comments = new[] { comment1ViewModel, comment2ViewModel };

            // Act
            var result = (ViewResult)_controller.Candidate(candidateId, commentModelFactory.Object);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.Candidate));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public void EditCandidate_Returns_EditCandidateView_With_Expected_Model()
        {
            var candidateId = Guid.NewGuid();
            var imageLocation = new BlobLocation("containerName", "blobPath");
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            candidate.Set(x => x.ImageLocation, imageLocation);

            QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
                .Returns(candidate);

            var expectedModel = new EditCandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<EditCandidateViewModel>(candidate))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_controller.EditCandidate(candidateId);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.EditCandidate));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public void EditCandidate_Returns_EditCandidate_View_With_ModelState_Errors_When_Model_Is_Invalid()
        {
            var model = new EditCandidateViewModel();
            var errorMessage = "errorMessage";
            _controller.ModelState.AddModelError("", errorMessage);

            // Act
            var result = (ViewResult)_controller.EditCandidate(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.EditCandidate));
            AssertModelStateContainsError(result, errorMessage);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public void EditCandidate_Returns_EditCandidate_View_With_ModelState_Errors_When_Command_Is_Not_Executed_Successfully()
        {
            var model = new EditCandidateViewModel
            {
                Description = "description",
                Title = "title"
            };

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditCandidateCommand>()))
                .Returns(commandResult);

            // Act
            var result = (ViewResult)_controller.EditCandidate(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.EditCandidate));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public void EditCandidate_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        {
            EditCandidateCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditCandidateCommand>()))
                .Callback<EditCandidateCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var model = new EditCandidateViewModel
            {
                Description = "description",
                Title = "title",
            };

            // Act
            var result = (RedirectToActionResult)_controller.EditCandidate(model);

            // Assert
            result.ActionName.Should().Be(nameof(ConceptualDesignController.Candidate));
        }

        [Fact]
        public void EditCandidateImage_Returns_EditCandidateImage_View_With_Expected_Model()
        {
            var candidateId = Guid.NewGuid();
            var imageLocation = new BlobLocation("containerName", "blobPath");
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            candidate.Set(x => x.ImageLocation, imageLocation);

            QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
                .Returns(candidate);

            var expectedModel = new EditCandidateImageViewModel();
            AutoMapperMock.Setup(x => x.Map<EditCandidateImageViewModel>(candidate))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_controller.EditCandidateImage(candidateId);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.EditCandidateImage));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task EditCandidateImage_Returns_EditCandidateImage_View_With_ModelState_Errors_When_Invalid_Model()
        {
            var model = new EditCandidateImageViewModel();
            var errorMessage = "errorMessage";
            _controller.ModelState.AddModelError("", errorMessage);

            // Act
            var result = (ViewResult)await _controller.EditCandidateImage(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.EditCandidateImage));
            AssertModelStateContainsError(result, errorMessage);

            _imageStorageProvider.Verify(x => x.SaveImageAsync(It.IsAny<Stream>(), It.IsAny<BlobLocation>(), SizeOptions.AllDefaultSizes), Times.Never);
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task EditCandidateImage_Returns_EditCandidateImage_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
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
                .Setup(p => p.Execute(It.IsAny<EditCandidateImageCommand>()))
                .Returns(commandResult);

            var expectedBlobLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, candidateId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains(fileName) && b.BlobPath.Contains(ProjectId.ToString());

            var model = new EditCandidateImageViewModel
            {
                Id = candidateId,
                Image = imageFile.Object
            };

            // Act
            var result = (ViewResult)await _controller.EditCandidateImage(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.EditCandidateImage));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public async Task EditCandidateImage_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        {
            var candidateId = Guid.NewGuid();
            var imageFile = new Mock<IFormFile>();
            var fileName = "fileName";
            imageFile.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var stream = Mock.Of<Stream>();
            imageFile.Setup(f => f.OpenReadStream())
                .Returns(stream);

            EditCandidateImageCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditCandidateImageCommand>()))
                .Callback<EditCandidateImageCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var expectedBlobLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, candidateId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains(fileName) && b.BlobPath.Contains(ProjectId.ToString());

            var model = new EditCandidateImageViewModel
            {
                Id = candidateId,
                Image = imageFile.Object
            };

            // Act
            var result = (RedirectToActionResult)await _controller.EditCandidateImage(model);

            // Assert
            result.ActionName.Should().Be(nameof(ConceptualDesignController.Candidate));
        }

        [Fact]
        public void RemoveCandidateImage_Returns_RemoveCandidateImage_View_With_Expected_Model()
        {
            var candidateId = Guid.NewGuid();
            var imageLocation = new BlobLocation("containerName", "blobPath");
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            candidate.Set(x => x.ImageLocation, imageLocation);

            QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
                .Returns(candidate);

            var expectedModel = new RemoveCandidateImageViewModel();
            AutoMapperMock.Setup(x => x.Map<RemoveCandidateImageViewModel>(candidate))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_controller.RemoveCandidateImage(candidateId);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.RemoveCandidateImage));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task Returns_RemoveCandidateImage_View_With_ModelState_Errors_When_Invalid_Model()
        {
            var model = new RemoveCandidateImageViewModel();
            var errorMessage = "errorMessage";
            _controller.ModelState.AddModelError("", errorMessage);

            // Act
            var result = (ViewResult)await _controller.RemoveCandidateImage(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.RemoveCandidateImage));
            AssertModelStateContainsError(result, errorMessage);

            _imageStorageProvider.Verify(x => x.DeleteImagesAsync(It.IsAny<BlobLocation>()), Times.Never);
            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task RemoveCandidateImage_Returns_RemoveCandidateImage_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
        {
            var candidateId = Guid.NewGuid();

            var expectedBlobLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, candidateId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains("fileName") && b.BlobPath.Contains(ProjectId.ToString());

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<DeleteCandidateImageCommand>()))
                .Returns(commandResult);

            var model = new RemoveCandidateImageViewModel
            {
                Id = candidateId,
            };

            // Act
            var result = (ViewResult)await _controller.RemoveCandidateImage(model);

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.RemoveCandidateImage));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public async Task RemoveCandidateImage_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        {
            var candidateId = Guid.NewGuid();

            DeleteCandidateImageCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<DeleteCandidateImageCommand>()))
                .Callback<DeleteCandidateImageCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var expectedBlobLocation = BlobLocations.GetProjectCandidateBlobLocation(ProjectId, candidateId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains("fileName") && b.BlobPath.Contains(ProjectId.ToString());

            var model = new RemoveCandidateImageViewModel
            {
                Id = candidateId,
            };

            // Act
            var result = (RedirectToActionResult)await _controller.RemoveCandidateImage(model);

            // Assert
            result.ActionName.Should().Be(nameof(ConceptualDesignController.Candidate));
        }

        [Fact]
        public void AddComment_Returns_Candidate_View_With_ModelState_Errors_When_Invalid_Model()
        {
            var candidateId = Guid.NewGuid();
            var model = new AddCommentViewModel
            {
                CandidateId = candidateId
            };
            var errorMessage = "errorMessage";
            _controller.ModelState.AddModelError("", errorMessage);

            var candidate = new Candidate();
            QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
                .Returns(candidate);

            var candidateViewModel = new CandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(candidateViewModel);

            // Act
            var result = (ViewResult)_controller.AddComment(model, Mock.Of<CommentViewModel.Factory>());

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.Candidate));
            AssertModelStateContainsError(result, errorMessage);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public void AddComment_Returns_Candidate_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
        {
            var candidateId = Guid.NewGuid();
            var model = new AddCommentViewModel
            {
                CandidateId = candidateId
            };

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddCandidateCommentCommand>()))
                .Returns(commandResult);

            var candidate = new Candidate();
            QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
                .Returns(candidate);

            var candidateViewModel = new CandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(candidateViewModel);

            // Act
            var result = (ViewResult)_controller.AddComment(model, Mock.Of<CommentViewModel.Factory>());

            // Assert
            result.ViewName.Should().Be(nameof(ConceptualDesignController.Candidate));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public void AddComment_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        {
            var model = new AddCommentViewModel();
            model.CandidateId = Guid.NewGuid();
            model.CommentText = "comment";

            AddCandidateCommentCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddCandidateCommentCommand>()))
                .Callback<AddCandidateCommentCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)_controller.AddComment(model, Mock.Of<CommentViewModel.Factory>());

            // Assert
            result.ActionName.Should().Be(nameof(ConceptualDesignController.Candidate));

            executedCommand.CandidateId.Should().Be(model.CandidateId);
            executedCommand.CommentText.Should().Be(model.CommentText);
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Actor.UserId.Should().Be(UserId);
        }
    }
}
