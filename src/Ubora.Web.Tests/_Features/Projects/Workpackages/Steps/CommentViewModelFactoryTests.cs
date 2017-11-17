using FluentAssertions;
using Moq;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Users;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web.Infrastructure.ImageServices;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.Steps
{
    public class CommentViewModelFactoryTests
    {
        private readonly CommentViewModel.Factory _factory;
        private readonly Mock<ImageStorageProvider> _imageStorageProvider;
        private readonly Mock<IQueryProcessor> _queryProcessor;

        public CommentViewModelFactoryTests()
        {
            _imageStorageProvider = new Mock<ImageStorageProvider>();
            _queryProcessor = new Mock<IQueryProcessor>();
            _factory = new CommentViewModel.Factory(_queryProcessor.Object, _imageStorageProvider.Object);
        }

        [Fact]
        public void Create_Returns_Expected_ViewModel()
        {
            var userId = Guid.NewGuid();
            var commentText = "commentText";
            var comment = new Comment(userId, commentText);

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

            var expectedModel = new CommentViewModel
            {
                CommentatorId = userId,
                CommentText = commentText,
                CommentatorName = "FirstName LastName",
                ProfilePictureUrl = profilePictureUrl
            };

            // Act
            var result = _factory.Create(comment);

            // Assert
            result.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
