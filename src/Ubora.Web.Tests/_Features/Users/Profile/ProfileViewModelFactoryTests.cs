using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects._Specifications;
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
            var userId = Guid.NewGuid();
            var userProfile = new UserProfile(userId: userId)
                .Set(x => x.ProfilePictureBlobLocation, new BlobLocation("blobContainer", "blobPath"));

            var expectedModel = new ProfileViewModel();

            _autoMapperMock.Setup(m => m.Map<ProfileViewModel>(userProfile))
                .Returns(expectedModel);

            _imageStorageProviderMock.Setup(x => x.GetUrl(userProfile.ProfilePictureBlobLocation))
                .Returns("expectedProfilePictureBlobUrl");

            var project = new Mock<Project>();
            var projectId = Guid.NewGuid();
            project.Object.Set(x => x.Id, projectId);
            var projectTitle = "title";
            project.Object.Set(x => x.Title, projectTitle);
            //var members = new ProjectMember[]
            //{
            //    new ProjectMentor(userId),
            //    new ProjectLeader(userId)
            //};
            //project.Setup(x => x.Members)
            //    .Returns(members);

            _queryProcessorMock.Setup(x => x.Find(new HasMember(userId)))
                .Returns(new PagedListStub<Project> { project.Object });

            // Act
            var result = _factoryUnderTest.Create(userProfile);

            // Assert
            result.Should().BeSameAs(expectedModel);

            result.ProfilePictureLink.Should().Be("expectedProfilePictureBlobUrl");
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Create_Finds_Out_Whether_User_With_Role_Mentor_Is_Verified_Ubora_Mentor(
            bool isVerifiedUboraMentor, bool isUnverifiedUboraMentor)
        {
            var userId = Guid.NewGuid();
            var userProfile = new UserProfile(userId)
                .Set(x => x.Role, "Mentor");

            // Stub mapping
            _autoMapperMock.Setup(m => m.Map<ProfileViewModel>(userProfile))
                .Returns(new ProfileViewModel());

            _queryProcessorMock
                .Setup(x => x.ExecuteQuery(It.Is<IsVerifiedUboraMentorQuery>(q => q.UserId == userId)))
                .Returns(isVerifiedUboraMentor);

            _queryProcessorMock.Setup(x => x.Find(new HasMember(userId)))
                .Returns(new PagedListStub<Project>());

            // Act
            var result = _factoryUnderTest.Create(userProfile);

            // Assert
            result.IsVerifiedMentor.Should().Be(isVerifiedUboraMentor);
            result.IsUnverifedMentor.Should().Be(isUnverifiedUboraMentor);
        }
    }
}
