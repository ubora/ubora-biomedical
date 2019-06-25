using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Ubora.Domain;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.CommercialDossiers;
using Ubora.Domain.Projects.CommercialDossiers.Commands;
using Ubora.Domain.Projects.IntellectualProperties;
using Ubora.Domain.Tests;
using Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.Storage;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.CommericalDocumentations
{
    public class CommercialDocumentationsControllerTests : ProjectControllerTestsBase
    {
        private readonly CommercialDocumentationsController _controller;
        private readonly Mock<IUboraStorageProvider> _storageProviderMock;

        private CommercialDossier CommercialDossier { get; set; } = new CommercialDossier();
        private IntellectualProperty IntellectualProperty { get; set; } = new IntellectualProperty();

        public CommercialDocumentationsControllerTests()
        {
            _storageProviderMock = new Mock<IUboraStorageProvider>();
            var commercialDossierViewModelFactoryMock = 
                new Mock<CommercialDossierViewModel.Factory>(QuillDeltaTransformerMock.Object, _storageProviderMock.Object);
            var commercialDossierPostModelHelperMock = 
                new Mock<CommercialDossierPostModel.Helper>(_storageProviderMock.Object) { CallBase = true };
            var intellectualPropertyViewModelFactoryMock = new Mock<IntellectualPropertyViewModel.Factory>();

            _controller = new CommercialDocumentationsController(
                _storageProviderMock.Object,
                commercialDossierViewModelFactoryMock.Object,
                commercialDossierPostModelHelperMock.Object,
                intellectualPropertyViewModelFactoryMock.Object);

            SetUpForTest(_controller);

            QueryProcessorMock.Setup(q => q.FindById<CommercialDossier>(ProjectId)).Returns(CommercialDossier);
            QueryProcessorMock.Setup(q => q.FindById<IntellectualProperty>(ProjectId)).Returns(IntellectualProperty);
        }

        [Fact]
        public async Task EditIntellectualProperty_Executes_Command_For_Changing_Ubora_License()
        {
            _controller
                .Set(c => c.IntellectualProperty, this.IntellectualProperty);

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
            _controller
                .Set(c => c.IntellectualProperty, this.IntellectualProperty);

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

        [Fact]
        public async Task EditCommercialDossier_Executes_Command_For_CreativeCommons_License()
        {
            _controller
                .Set(c => c.CommercialDossier, this.CommercialDossier);

            this.CommercialDossier
                .Set(x => x.Logo, new BlobLocation("logoContainer", "logoPath"))
                .Set(x => x.UserManual, new UserManual(new BlobLocation("userManualContainer", "userManualPath"), "test", 0));

            EditCommercialDossierCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<EditCommercialDossierCommand>()))
                .Callback<EditCommercialDossierCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Execute cases
            await Case_With_All_Fresh_Data();
            await Case_With_Removed_UserManual_And_Logo();
            await Case_With_Unchanged_UserManual_And_logo();

            // Case descriptions
            async Task Case_With_All_Fresh_Data()
            {
                var postModel = new CommercialDossierPostModel
                {
                    CommercialName = TestStrings.CreateRandom(),
                    DescriptionQuillDelta = TestQuillDeltas.CreateRandom().Value,
                    Logo = TestFormFiles.CreateRandom(),
                    UserManual = TestFormFiles.CreateRandom(),
                    ProductName = TestStrings.CreateRandom(),
                    UserManualName = TestStrings.CreateRandom()
                };

                // Act
                await _controller.EditCommercialDossier(postModel);

                // Assert
                executedCommand.CommercialName.Should().Be(postModel.CommercialName);
                executedCommand.Description.Should().Be(new QuillDelta(postModel.DescriptionQuillDelta));
                executedCommand.LogoLocation.Should().Be(BlobLocations.GetCommercialDossierBlobLocation(ProjectId, postModel.Logo.GetFileName()));
                executedCommand.ProductName.Should().Be(postModel.ProductName);

                executedCommand.UserManualInfo.Location.Should()
                    .Be(BlobLocations.GetCommercialDossierBlobLocation(
                        ProjectId,
                        postModel.UserManual.GetFileName()));
            }

            async Task Case_With_Removed_UserManual_And_Logo()
            {
                var postModel = new CommercialDossierPostModel
                {
                    Logo = null,
                    HasOldLogoBeenDeleted = true,
                    UserManual = null,
                    HasOldUserManualBeenRemoved = true
                };

                // Act
                await _controller.EditCommercialDossier(postModel);

                // Assert
                executedCommand.LogoLocation.Should().BeNull();
                executedCommand.UserManualInfo.Location.Should().BeNull();
            }

            async Task Case_With_Unchanged_UserManual_And_logo()
            {
                var postModel = new CommercialDossierPostModel
                {
                    Logo = null,
                    HasOldLogoBeenDeleted = false,
                    UserManual = null,
                    HasOldUserManualBeenRemoved = false
                };

                // Assume
                this.CommercialDossier.UserManual.Should().NotBeNull();
                this.CommercialDossier.Logo.Should().NotBeNull();

                // Act
                await _controller.EditCommercialDossier(postModel);

                // Assert
                executedCommand.LogoLocation.Should().Be(CommercialDossier.Logo);
                executedCommand.UserManualInfo.Location.Should().Be(CommercialDossier.UserManual.Location);
            }
        }
    }
}
