using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.CommercialDossiers;
using Ubora.Domain.Projects.IntellectualProperties;
using Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations;
using Ubora.Web.Infrastructure.Storage;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.CommericalDocumentations
{
    public class CommercialDocumentationsControllerTests : ProjectControllerTestsBase
    {
        private readonly CommercialDocumentationsController _controller;

        private CommercialDossier CommercialDossier { get; set; } = new CommercialDossier();
        private IntellectualProperty IntellectualProperty { get; set; } = new IntellectualProperty();

        public CommercialDocumentationsControllerTests()
        {
            var storageProviderMock = new Mock<IUboraStorageProvider>();
            var commercialDossierViewModelFactoryMock = new Mock<CommercialDossierViewModel.Factory>();
            var commercialDossierViewModelHelperMock = new Mock<CommercialDossierViewModel.Helper>() { CallBase = true };
            var intellectualPropertyViewModelFactoryMock = new Mock<IntellectualPropertyViewModel.Factory>();

            _controller = new CommercialDocumentationsController(
                storageProviderMock.Object,
                commercialDossierViewModelFactoryMock.Object,
                commercialDossierViewModelHelperMock.Object,
                intellectualPropertyViewModelFactoryMock.Object);

            SetUpForTest(_controller);

            QueryProcessorMock.Setup(q => q.FindById<CommercialDossier>(ProjectId)).Returns(CommercialDossier);
            QueryProcessorMock.Setup(q => q.FindById<IntellectualProperty>(ProjectId)).Returns(IntellectualProperty);
        }

        [Fact]
        public async Task EditIntellectualProperty_Executes_Command_For_Changing_Ubora_License()
        {
            var postModel = new IntellectualPropertyViewModel
            {
                License = LicenseType.Ubora,
                Attribution = true,
                NonCommercial = true,
                NoDerivativeWorks = true,
                ShareAlike = true,
            };

            EditLicenseCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditLicenseCommand>()))
                .Callback<EditLicenseCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            await _controller.EditIntellectualProperty(postModel);

            // Assert
            executedCommand.UboraLicense.Should().BeTrue();
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Actor.UserId.Should().Be(UserId);

            executedCommand.NoDerivativeWorks.Should().BeFalse();
            executedCommand.NonCommercial.Should().BeFalse();
            executedCommand.ShareAlike.Should().BeFalse();
            executedCommand.Attribution.Should().BeFalse();
        }

        [Fact]
        public async Task EditIntellectualProperty_Executes_Command_For_CreativeCommons_License()
        {
            var postModel = new IntellectualPropertyViewModel
            {
                License = LicenseType.CreativeCommons,
                Attribution = true,
                NonCommercial = true,
                NoDerivativeWorks = true,
                ShareAlike = true,
            };

            EditLicenseCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditLicenseCommand>()))
                .Callback<EditLicenseCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            await _controller.EditIntellectualProperty(postModel);

            // Assert
            executedCommand.NoDerivativeWorks.Should().BeTrue();
            executedCommand.NonCommercial.Should().BeTrue();
            executedCommand.ShareAlike.Should().BeTrue();
            executedCommand.Attribution.Should().BeTrue();
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Actor.UserId.Should().Be(UserId);

            executedCommand.UboraLicense.Should().BeFalse();
        }
    }
}
