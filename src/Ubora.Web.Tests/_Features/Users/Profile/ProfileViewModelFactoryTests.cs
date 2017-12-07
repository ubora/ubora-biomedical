using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.UboraMentors.Queries;
using Ubora.Web._Features.Users.Profile;
using Xunit;

namespace Ubora.Web.Tests._Features.Users.Profile
{
    public class ProfileViewModelFactoryTests
    {
        private readonly ProfileViewModel.Factory _factoryUnderTest;
        private readonly Mock<IMapper> _autoMapperMock;
        private readonly Mock<ImageStorageProvider> _imageStorageProviderMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public ProfileViewModelFactoryTests()
        {
            _autoMapperMock = new Mock<IMapper>();
            _imageStorageProviderMock = new Mock<ImageStorageProvider>();
            _queryProcessorMock = new Mock<IQueryProcessor>();

            _factoryUnderTest = new ProfileViewModel.Factory(_autoMapperMock.Object, _imageStorageProviderMock.Object, _queryProcessorMock.Object);
        }

        [Fact]
        public void Create_Creates_ViewModel_For_UserProfile()
        {
            var userProfile = new UserProfile(userId: Guid.NewGuid())
                .Set(x => x.ProfilePictureBlobLocation, new BlobLocation("blobContainer", "blobPath"));

            var expectedModel = new ProfileViewModel();

            _autoMapperMock.Setup(m => m.Map<ProfileViewModel>(userProfile))
                .Returns(expectedModel);

            _imageStorageProviderMock.Setup(x => x.GetUrl(userProfile.ProfilePictureBlobLocation))
                .Returns("expectedProfilePictureBlobUrl");

            // Act
            var result = _factoryUnderTest.Create(userProfile);

            // Assert
            result.Should().BeSameAs(expectedModel);

            result.ProfilePictureLink.Should().Be("expectedProfilePictureBlobUrl");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Create_Finds_Out_Whether_User_With_Role_Mentor_Is_Verified_Ubora_Mentor(
            bool isUboraMentor)
        {
            var userId = Guid.NewGuid();
            var userProfile = new UserProfile(userId)
                .Set(x => x.Role, "mentor");

            // Stub mapping
            _autoMapperMock.Setup(m => m.Map<ProfileViewModel>(userProfile))
                .Returns(new ProfileViewModel());

            _queryProcessorMock
                .Setup(x => x.ExecuteQuery(It.Is<IsVerifiedUboraMentorQuery>(q => q.UserId == userId)))
                .Returns(isUboraMentor);

            // Act
            var result = _factoryUnderTest.Create(userProfile);

            // Assert
            result.IsVerifiedMentor.Should().Be(isUboraMentor);
        }
    }
}
