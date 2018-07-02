using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Domain.Projects.Candidates.Specifications;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features.Projects.Workpackages.Candidates;
using Ubora.Web._Features._Shared.Notices;
using Xunit;
using Ubora.Domain.Discussions;
using Ubora.Web._Components.Discussions.Models;
using Ubora.Domain.Discussions.Commands;
using Ubora.Domain.Projects;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Candidates
{
    public class CandidatesControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly Mock<CandidatesController> _controllerMock;
        private CandidatesController _controller => _controllerMock.Object;

        public CandidatesControllerTests()
        {
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _controllerMock = new Mock<CandidatesController>(_imageStorageProvider.Object)
            {
                CallBase = true
            };

            SetUpForTest(_controller);
        }

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var methodPolicies = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
            {
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(CandidatesController.AddCandidate),
                    Policies = new []{ nameof(Policies.CanAddProjectCandidate) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(CandidatesController.RemoveCandidate),
                    Policies = new []{ nameof(Policies.CanRemoveCandidate) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(CandidatesController.EditCandidate),
                    Policies = new []{ nameof(Policies.CanEditProjectCandidate) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(CandidatesController.EditCandidateImage),
                    Policies = new []{ nameof(Policies.CanChangeProjectCandidateImage) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(CandidatesController.RemoveCandidateImage),
                    Policies = new []{ nameof(Policies.CanRemoveProjectCandidateImage) }
                },
                new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                {
                    MethodName = nameof(CandidatesController.OpenWorkpackageThree),
                    Policies = new []{ nameof(Policies.CanOpenWorkpackageThree) }
                }
            };

            AssertHasAuthorizeAttributes(typeof(CandidatesController), methodPolicies);
        }


        [Fact]
        public async Task Voting_Returns_Voting_View_With_Candidates()
        {
            var candidateItemViewModelFactoryMock = new Mock<CandidateItemViewModel.Factory>(Mock.Of<ImageStorageProvider>(), Mock.Of<IMapper>());

            var candidate1 = new Candidate();
            var candidate2 = new Candidate();

            QueryProcessorMock.Setup(x => x.Find(new IsProjectCandidateSpec(ProjectId)))
                .Returns(new PagedListStub<Candidate>() { candidate1, candidate2 });

            var candidate1ItemViewModel = new CandidateItemViewModel();
            candidateItemViewModelFactoryMock.Setup(x => x.Create(candidate1))
                .Returns(candidate1ItemViewModel);

            var candidate2ItemViewModel = new CandidateItemViewModel();
            candidateItemViewModelFactoryMock.Setup(x => x.Create(candidate2))
                .Returns(candidate2ItemViewModel);

            AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, null, Policies.CanOpenWorkpackageThree))
                .ReturnsAsync(AuthorizationResult.Success);

            QueryProcessorMock.Setup(x => x.ExecuteQuery(It.Is<IsWorkpackageThreeOpenedQuery>(q => q.ProjectId == ProjectId)))
                .Returns(false);

            var expectedModel = new VotingViewModel
            {
                Candidates = new[] { candidate1ItemViewModel, candidate2ItemViewModel }.AsEnumerable(),
                CanOpenWorkpackageThree = true
            };

            // Act
            var result = (ViewResult)await _controller.Voting(candidateItemViewModelFactoryMock.Object);

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.Voting));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public void AddCandidate_Returns_View_With_Model()
        {
            var expectedModel = new AddCandidateViewModel();

            // Act
            var result = (ViewResult)_controller.AddCandidate();

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.AddCandidate));
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
            result.ViewName.Should().Be(nameof(CandidatesController.AddCandidate));
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
            result.ViewName.Should().Be(nameof(CandidatesController.AddCandidate));
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
            result.ActionName.Should().Be(nameof(CandidatesController.Voting));
            result.ControllerName.Should().Be("Candidates");
        }

        [Fact]
        public void RemoveCandidate_Redirects_To_Voting_When_Not_Executed_Successfully()
        {
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<RemoveCandidateCommand>()))
                .Returns(CommandResult.Failed("Error"));

            var model = new RemoveCandidateViewModel();

            // Act
            var result = (RedirectToActionResult)_controller.RemoveCandidate(model);

            // Assert
            result.ActionName.Should().Be(nameof(CandidatesController.Voting));
            result.ControllerName.Should().Be("Candidates");
        }

        [Fact]
        public void RemoveCandidate_Redirects_To_Voting_When_Command_Executed_Successfully()
        {
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<RemoveCandidateCommand>()))
                .Returns(CommandResult.Success);

            var model = new RemoveCandidateViewModel();

            // Act
            var result = (RedirectToActionResult)_controller.RemoveCandidate(model);

            // Assert
            result.ActionName.Should().Be(nameof(CandidatesController.Voting));
            result.ControllerName.Should().Be("Candidates");
        }

        [Fact]
        public async Task Candidate_Returns_Candidate_View_With_Expected_Model()
        {
            var candidateId = Guid.NewGuid();
            var candidate = new Candidate().Set(c => c.Id, candidateId);
            var discussion = new Discussion();

            _controller.CurrentCandidate = candidate;

            var candidateViewModelFactory = new Mock<CandidateViewModel.Factory>();
            var expectedModel = new CandidateViewModel();

            QueryProcessorMock
                .Setup(q => q.FindById<Discussion>(candidateId))
                .Returns(discussion);

            candidateViewModelFactory.Setup(x => x.Create(candidate, discussion, User))
                .ReturnsAsync(expectedModel);

            // Act
            var result = (ViewResult)await _controller.Candidate(candidateViewModelFactory.Object);

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
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

            _controller.CurrentCandidate = candidate;

            var expectedModel = new EditCandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<EditCandidateViewModel>(candidate))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_controller.EditCandidate(candidateId);

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.EditCandidate));
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
            result.ViewName.Should().Be(nameof(CandidatesController.EditCandidate));
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
            result.ViewName.Should().Be(nameof(CandidatesController.EditCandidate));
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
            result.ActionName.Should().Be(nameof(CandidatesController.Candidate));
        }

        [Fact]
        public void EditCandidateImage_Returns_EditCandidateImage_View_With_Expected_Model()
        {
            var candidateId = Guid.NewGuid();
            var imageLocation = new BlobLocation("containerName", "blobPath");
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            candidate.Set(x => x.ImageLocation, imageLocation);

            _controller.CurrentCandidate = candidate;

            var expectedModel = new EditCandidateImageViewModel();
            AutoMapperMock.Setup(x => x.Map<EditCandidateImageViewModel>(candidate))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_controller.EditCandidateImage();

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.EditCandidateImage));
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
            result.ViewName.Should().Be(nameof(CandidatesController.EditCandidateImage));
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
            result.ViewName.Should().Be(nameof(CandidatesController.EditCandidateImage));
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
            result.ActionName.Should().Be(nameof(CandidatesController.Candidate));
        }

        [Fact]
        public void RemoveCandidateImage_Returns_RemoveCandidateImage_View_With_Expected_Model()
        {
            var candidateId = Guid.NewGuid();
            var imageLocation = new BlobLocation("containerName", "blobPath");
            var candidate = new Candidate();
            candidate.Set(x => x.Id, candidateId);
            candidate.Set(x => x.ImageLocation, imageLocation);

            _controller.CurrentCandidate = candidate;

            var expectedModel = new RemoveCandidateImageViewModel();
            AutoMapperMock.Setup(x => x.Map<RemoveCandidateImageViewModel>(candidate))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_controller.RemoveCandidateImage();

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.RemoveCandidateImage));
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
            result.ViewName.Should().Be(nameof(CandidatesController.RemoveCandidateImage));
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
            result.ViewName.Should().Be(nameof(CandidatesController.RemoveCandidateImage));
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
            result.ActionName.Should().Be(nameof(CandidatesController.Candidate));
        }

        [Fact]
        public async Task AddComment_Returns_Candidate_View_With_ModelState_Errors_When_Invalid_Model()
        {
            var model = new AddCommentModel();
            var errorMessage = "errorMessage";
            _controller.ModelState.AddModelError("", errorMessage);

            var candidate = new Candidate();

            _controller.CurrentCandidate = candidate;

            QueryProcessorMock
                .Setup(q => q.FindById<Project>(ProjectId))
                .Returns(new Project());

            var candidateViewModel = new CandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(candidateViewModel);

            // Act
            var result = (ViewResult)await _controller.AddComment(model, Mock.Of<CandidateViewModel.Factory>());

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
            AssertModelStateContainsError(result, errorMessage);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task AddComment_Returns_Candidate_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
        {
            var model = new AddCommentModel();

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddCommentCommand>()))
                .Returns(commandResult);

            var candidate = new Candidate();
            _controller.CurrentCandidate = candidate;

            QueryProcessorMock
                .Setup(q => q.FindById<Project>(ProjectId))
                .Returns(new Project());

            var candidateViewModel = new CandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(candidateViewModel);

            // Act
            var result = (ViewResult)await _controller.AddComment(model, Mock.Of<CandidateViewModel.Factory>());

            // Assert
            result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public async Task AddComment_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        {
            var model = new AddCommentModel
            {
                CommentText = "comment"
            };

            var candidate = new Candidate().Set(c => c.Id, Guid.NewGuid());
            _controller.CurrentCandidate = candidate;

            QueryProcessorMock
                .Setup(q => q.FindById<Project>(ProjectId))
                .Returns(new Project());

            AddCommentCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddCommentCommand>()))
                .Callback<AddCommentCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)await _controller.AddComment(model, Mock.Of<CandidateViewModel.Factory>());

            // Assert
            result.ActionName.Should().Be(nameof(CandidatesController.Candidate));

            executedCommand.DiscussionId.Should().Be(candidate.Id);
            executedCommand.CommentText.Should().Be(model.CommentText);
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Actor.UserId.Should().Be(UserId);
        }

        [Fact]
        public async Task EditComment_Returns_Candidate_View_With_ModelState_Errors_When_Invalid_Model()
        {
            var model = new EditCommentModel();
            var errorMessage = "errorMessage";
            _controller.ModelState.AddModelError("", errorMessage);

            var candidateViewModelFactory = Mock.Of<CandidateViewModel.Factory>();
            var expectedResult = new ViewResult();
            _controllerMock.Setup(c => c.Candidate(candidateViewModelFactory)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.EditComment(model, candidateViewModelFactory);

            // Assert
            result.Should().BeSameAs(expectedResult);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task EditComment_Returns_Forbid_When_User_Not_Allowed_To_Edit_Comment()
        {
            var candidateId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var model = new EditCommentModel
            {
                CommentText = "commentText",
                CommentId = commentId
            };

            var comment = Comment.Create(commentId, UserId, "comment", DateTime.UtcNow, new Dictionary<string, object> { { "RoleKeys", "project-member" } }.ToImmutableDictionary());
            var candidate = new Candidate().Set(c => c.Id, candidateId);

            _controller.CurrentCandidate = candidate;

            QueryProcessorMock
                .Setup(x => x.FindById<Discussion>(candidateId))
                .Returns(new Discussion().Set(d => d.Comments, new [] { comment }.ToImmutableList()));

            AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, comment, Policies.CanEditComment))
                .ReturnsAsync(AuthorizationResult.Failed);

            var candidateViewModel = new CandidateViewModel();
            AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
                .Returns(candidateViewModel);

            // Act
            var result = await _controller.EditComment(model, Mock.Of<CandidateViewModel.Factory>());

            // Assert
            result.GetType().Should().Be(typeof(ForbidResult));

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        // TODO(Kaspar Kallas): Fix these tests...
        //[Fact]
        //public async Task EditComment_Returns_Candidate_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var commentId = Guid.NewGuid();
        //    var model = new EditCommentViewModel
        //    {
        //        CandidateId = candidateId,
        //        CommentText = "commentText",
        //        Id = commentId
        //    };

        //    var comment = new Comment(UserId, "comment", commentId, DateTime.UtcNow, new[] { "project-member" });
        //    var candidate = new Mock<Candidate>();
        //    candidate.Setup(x => x.Comments)
        //        .Returns(new[] { comment });
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate.Object);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, comment, Policies.CanEditComment))
        //        .ReturnsAsync(AuthorizationResult.Success);

        //    var commandResult = CommandResult.Failed("testError1", "testError2");
        //    CommandProcessorMock
        //        .Setup(p => p.Execute(It.IsAny<EditCandidateCommentCommand>()))
        //        .Returns(commandResult);

        //    var candidateViewModel = new CandidateViewModel();
        //    AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
        //        .Returns(candidateViewModel);

        //    // Act
        //    var result = (ViewResult)await _controller.EditComment(model, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
        //    AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        //}

        //[Fact]
        //public async Task EditComment_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var commentId = Guid.NewGuid();
        //    var model = new EditCommentViewModel
        //    {
        //        CandidateId = candidateId,
        //        CommentText = "commentText",
        //        Id = commentId
        //    };

        //    var comment = new Comment(UserId, "comment", commentId, DateTime.UtcNow, new[] { "project-member" });
        //    var candidate = new Mock<Candidate>();
        //    candidate.Setup(x => x.Comments)
        //        .Returns(new[] { comment });
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate.Object);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, comment, Policies.CanEditComment))
        //        .ReturnsAsync(AuthorizationResult.Success);

        //    EditCandidateCommentCommand executedCommand = null;
        //    CommandProcessorMock
        //        .Setup(p => p.Execute(It.IsAny<EditCandidateCommentCommand>()))
        //        .Callback<EditCandidateCommentCommand>(c => executedCommand = c)
        //        .Returns(CommandResult.Success);

        //    // Act
        //    var result = (RedirectToActionResult)await _controller.EditComment(model, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ActionName.Should().Be(nameof(CandidatesController.Candidate));

        //    executedCommand.CommentId.Should().Be(commentId);
        //    executedCommand.CandidateId.Should().Be(model.CandidateId);
        //    executedCommand.CommentText.Should().Be(model.CommentText);
        //    executedCommand.ProjectId.Should().Be(ProjectId);
        //    executedCommand.Actor.UserId.Should().Be(UserId);
        //}

        //[Fact]
        //public async Task RemoveComment_Returns_Candidate_View_With_ModelState_Errors_When_Invalid_Model()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var commentId = Guid.NewGuid();
        //    var errorMessage = "errorMessage";
        //    _controller.ModelState.AddModelError("", errorMessage);

        //    var candidate = new Candidate();
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate);

        //    var candidateViewModel = new CandidateViewModel();
        //    AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
        //        .Returns(candidateViewModel);

        //    // Act
        //    var result = (ViewResult)await _controller.RemoveComment(candidateId, commentId, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
        //    AssertModelStateContainsError(result, errorMessage);

        //    CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        //}

        //[Fact]
        //public async Task RemoveComment_Returns_Forbid_When_User_Not_Allowed_To_Edit_Comment()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var commentId = Guid.NewGuid();

        //    var comment = new Comment(UserId, "comment", commentId, DateTime.UtcNow, new[] { "project-member" });
        //    var candidate = new Mock<Candidate>();
        //    candidate.Setup(x => x.Comments)
        //        .Returns(new[] { comment });
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate.Object);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, comment, Policies.CanEditComment))
        //        .ReturnsAsync(AuthorizationResult.Failed);

        //    var candidateViewModel = new CandidateViewModel();
        //    AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
        //        .Returns(candidateViewModel);

        //    // Act
        //    var result = await _controller.RemoveComment(candidateId, commentId, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.GetType().Should().Be(typeof(ForbidResult));

        //    CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        //}

        //[Fact]
        //public async Task RemoveComment_Returns_Candidate_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var commentId = Guid.NewGuid();

        //    var comment = new Comment(UserId, "comment", commentId, DateTime.UtcNow, new[] { "project-member" });
        //    var candidate = new Mock<Candidate>();
        //    candidate.Setup(x => x.Comments)
        //        .Returns(new[] { comment });
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate.Object);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, comment, Policies.CanEditComment))
        //        .ReturnsAsync(AuthorizationResult.Success);

        //    var commandResult = CommandResult.Failed("testError1", "testError2");
        //    CommandProcessorMock
        //        .Setup(p => p.Execute(It.IsAny<RemoveCandidateCommentCommand>()))
        //        .Returns(commandResult);

        //    var candidateViewModel = new CandidateViewModel();
        //    AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
        //        .Returns(candidateViewModel);

        //    // Act
        //    var result = (ViewResult)await _controller.RemoveComment(candidateId, commentId, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
        //    AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        //}

        //[Fact]
        //public async Task RemoveComment_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var commentId = Guid.NewGuid();

        //    var comment = new Comment(UserId, "comment", commentId, DateTime.UtcNow, new[] { "project-member" });
        //    var candidate = new Mock<Candidate>();
        //    candidate.Setup(x => x.Comments)
        //        .Returns(new[] { comment });
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate.Object);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, comment, Policies.CanEditComment))
        //        .ReturnsAsync(AuthorizationResult.Success);

        //    RemoveCandidateCommentCommand executedCommand = null;
        //    CommandProcessorMock
        //        .Setup(p => p.Execute(It.IsAny<RemoveCandidateCommentCommand>()))
        //        .Callback<RemoveCandidateCommentCommand>(c => executedCommand = c)
        //        .Returns(CommandResult.Success);

        //    // Act
        //    var result = (RedirectToActionResult)await _controller.RemoveComment(candidateId, commentId, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ActionName.Should().Be(nameof(CandidatesController.Candidate));

        //    executedCommand.CommentId.Should().Be(commentId);
        //    executedCommand.CandidateId.Should().Be(candidateId);
        //    executedCommand.ProjectId.Should().Be(ProjectId);
        //    executedCommand.Actor.UserId.Should().Be(UserId);
        //}

        //[Fact]
        //public async Task AddVote_Returns_Candidate_View_With_ModelState_Errors_When_Invalid_Model()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var model = new AddVoteViewModel
        //    {
        //        CandidateId = candidateId,
        //        Safety = 2,
        //        Usability = 3,
        //        Functionality = 4,
        //        Performace = 5
        //    };
        //    var errorMessage = "errorMessage";
        //    _controller.ModelState.AddModelError("", errorMessage);

        //    var candidate = new Candidate();
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate);

        //    var candidateViewModel = new CandidateViewModel();
        //    AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
        //        .Returns(candidateViewModel);

        //    // Act
        //    var result = (ViewResult)await _controller.AddVote(model, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
        //    AssertModelStateContainsError(result, errorMessage);

        //    CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        //}

        //[Fact]
        //public async Task AddVote_Returns_Candidate_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var model = new AddVoteViewModel
        //    {
        //        CandidateId = candidateId,
        //        Safety = 2,
        //        Usability = 3,
        //        Functionality = 4,
        //        Performace = 5
        //    };

        //    var candidate = new Candidate();
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, candidate, Policies.CanVoteCandidate))
        //        .ReturnsAsync(AuthorizationResult.Success);

        //    var commandResult = CommandResult.Failed("testError1", "testError2");
        //    CommandProcessorMock
        //        .Setup(p => p.Execute(It.IsAny<AddCandidateVoteCommand>()))
        //        .Returns(commandResult);

        //    var candidateViewModel = new CandidateViewModel();
        //    AutoMapperMock.Setup(x => x.Map<CandidateViewModel>(candidate))
        //        .Returns(candidateViewModel);

        //    // Act
        //    var result = (ViewResult)await _controller.AddVote(model, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ViewName.Should().Be(nameof(CandidatesController.Candidate));
        //    AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        //}

        //[Fact]
        //public async Task AddVote_Redirects_To_Candidate_View_When_Command_Executed_Successfully()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var model = new AddVoteViewModel
        //    {
        //        CandidateId = candidateId,
        //        Safety = 2,
        //        Usability = 3,
        //        Functionality = 4,
        //        Performace = 5
        //    };

        //    var candidate = new Candidate();
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, candidate, Policies.CanVoteCandidate))
        //        .ReturnsAsync(AuthorizationResult.Success);

        //    AddCandidateVoteCommand executedCommand = null;
        //    CommandProcessorMock
        //        .Setup(p => p.Execute(It.IsAny<AddCandidateVoteCommand>()))
        //        .Callback<AddCandidateVoteCommand>(c => executedCommand = c)
        //        .Returns(CommandResult.Success);

        //    // Act
        //    var result = (RedirectToActionResult)await _controller.AddVote(model, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.ActionName.Should().Be(nameof(CandidatesController.Candidate));

        //    executedCommand.CandidateId.Should().Be(model.CandidateId);
        //    executedCommand.Safety.Should().Be(2);
        //    executedCommand.Usability.Should().Be(3);
        //    executedCommand.Functionality.Should().Be(4);
        //    executedCommand.Performance.Should().Be(5);
        //    executedCommand.ProjectId.Should().Be(ProjectId);
        //    executedCommand.Actor.UserId.Should().Be(UserId);
        //}

        //[Fact]
        //public async Task AddVote_Returns_Forbid_When_User_Not_Allowed_To_Vote()
        //{
        //    var candidateId = Guid.NewGuid();
        //    var addVoteViewModel = new AddVoteViewModel
        //    {
        //        CandidateId = candidateId,
        //        Safety = 1,
        //        Usability = 2,
        //        Functionality = 3,
        //        Performace = 4
        //    };

        //    var candidate = new Candidate();
        //    QueryProcessorMock.Setup(x => x.FindById<Candidate>(candidateId))
        //        .Returns(candidate);

        //    AuthorizationServiceMock.Setup(x => x.AuthorizeAsync(User, candidate, Policies.CanVoteCandidate))
        //        .ReturnsAsync(AuthorizationResult.Failed);

        //    // Act
        //    var result = await _controller.AddVote(addVoteViewModel, Mock.Of<CandidateViewModel.Factory>());

        //    // Assert
        //    result.GetType().Should().Be(typeof(ForbidResult));

        //    CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        //}

        //[Fact]
        //public async Task OpenWorkpackageThree_Executes_Command_And_Redirects_To_Voting_With_Success_Notice()
        //{
        //    OpenWorkpackageThreeCommand executedCommand = null;
        //    CommandProcessorMock
        //        .Setup(p => p.Execute(It.IsAny<OpenWorkpackageThreeCommand>()))
        //        .Callback<OpenWorkpackageThreeCommand>(c => executedCommand = c)
        //        .Returns(CommandResult.Success);

        //    var candidateItemViewModelFactoryMock = new Mock<CandidateItemViewModel.Factory>(Mock.Of<ImageStorageProvider>(), Mock.Of<IMapper>());

        //    // Act
        //    var result = (RedirectToActionResult)await _controller.OpenWorkpackageThree(candidateItemViewModelFactoryMock.Object);

        //    // Assert
        //    result.ActionName.Should().Be(nameof(CandidatesController.Voting));
        //    executedCommand.ProjectId.Should().Be(ProjectId);
        //    executedCommand.Actor.UserId.Should().Be(UserId);

        //    var successNotice = _controller.Notices.Dequeue();
        //    successNotice.Text.Should().Be(SuccessTexts.WP3Opened);
        //    successNotice.Type.Should().Be(NoticeType.Success);
        //}
    }
}
