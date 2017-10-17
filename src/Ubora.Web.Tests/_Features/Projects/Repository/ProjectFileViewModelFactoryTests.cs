using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository;
using Ubora.Web._Features.Projects.Repository;
using Ubora.Web.Infrastructure.Storage;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Repository
{
    public class ProjectFileViewModelFactoryTests
    {
        private readonly Mock<IUboraStorageProvider> _uboraStorageProvider;
        private readonly Mock<IMapper> _mapper;
        private readonly ProjectFileViewModel.Factory _projectFileViewModelFactory;

        public ProjectFileViewModelFactoryTests()
        {
            _uboraStorageProvider = new Mock<IUboraStorageProvider>();
            _mapper = new Mock<IMapper>();
            _projectFileViewModelFactory = new ProjectFileViewModel.Factory(_uboraStorageProvider.Object, _mapper.Object);
        }

        [Fact]
        public void Create_Returns_Expected_ProjectFileViewModel()
        {
            var fileId = Guid.NewGuid();
            var blobLocation = new BlobLocation("containerName", "blobPath");
            var projectFile = new ProjectFile();
            projectFile.Set(x => x.Id, fileId);
            projectFile.Set(x => x.FileName, "fileName");
            projectFile.Set(x => x.Comment, "comment");
            projectFile.Set(x => x.FileSize, 12345);
            projectFile.Set(x => x.RevisionNumber, 2);
            projectFile.Set(x => x.Location, blobLocation);

            var downloadUrl = "downloadUrl";
            _uboraStorageProvider.Setup(x => x.GetReadUrl(blobLocation, It.IsAny<DateTime>()))
                .Returns(downloadUrl);

            var expectedModel = new ProjectFileViewModel
            {
                Id = fileId,
                FileName = "fileName",
                Comment = "comment",
                FileSize = 12345,
                RevisionNumber = 2,
                DownloadUrl = downloadUrl
            };

            _mapper.Setup(x => x.Map<ProjectFileViewModel>(projectFile))
                .Returns(expectedModel);

            // Act
            var result = _projectFileViewModelFactory.Create(projectFile);

            // Assert
            result.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
