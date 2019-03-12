using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.CommercialDossiers;
using Ubora.Domain.Projects.CommercialDossiers.Commands;
using Ubora.Domain.Projects.CommercialDossiers.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.CommercialDossiers
{
    public class EditCommercialDossierCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Edits_Commercial_Dossier_Values_When_Changed() 
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp5Unlocked()
                    .Seed(this);

            var actor = new DummyUserInfo();
            var description = new QuillDelta("{Description}");
            var logo = new BlobLocation("logo", "logo");
            var userManual = new BlobLocation("userManual", "userManual");

            var command = new EditCommercialDossierCommand
            {
                Actor = actor,
                ProjectId = project.Id,
                ProductName = "productName",
                CommercialName = "commercialName",
                Description = description,
                Logo = logo,
                UserManual = userManual
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var events = Session.Events.FetchStream(project.Id).Select(x => x.Data).TakeLast(5);

            events.Should().Contain(x => x is ProductNameChangedEvent);
            events.Should().Contain(x => x is CommercialNameChangedEvent);
            events.Should().Contain(x => x is DescriptionEditedEvent);
            events.Should().Contain(x => x is LogoChangedEvent);
            events.Should().Contain(x => x is UserManualChangedEvent);
            events.Should().OnlyContain(x => ((UboraEvent)x).InitiatedBy == actor);

            var commercialDossier = Session.Load<CommercialDossier>(project.Id);
            commercialDossier.ProductName.Should().Be("productName");
            commercialDossier.CommercialName.Should().Be("commercialName");
            commercialDossier.Description.Should().Be(description);
            commercialDossier.Logo.Should().Be(logo);
            commercialDossier.UserManual.Should().Be(userManual);
        }

        [Fact]
        public void Does_Not_Persists_Events_When_Not_Changed() 
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .WithWp4Unlocked()
                    .WithWp5Unlocked()
                    .Seed(this);
            var commercialDossier = Session.Load<CommercialDossier>(project.Id);

            var command = new EditCommercialDossierCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = project.Id,
                CommercialName = commercialDossier.CommercialName,
                Description = commercialDossier.Description,
                Logo = commercialDossier.Logo,
                ProductName = commercialDossier.ProductName,
                UserManual = commercialDossier.UserManual
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var events = Session.Events.FetchStream(project.Id).Select(x => x.Data);

            events.Should().NotContain(x => x is ProductNameChangedEvent);
            events.Should().NotContain(x => x is CommercialNameChangedEvent);
            events.Should().NotContain(x => x is DescriptionEditedEvent);
            events.Should().NotContain(x => x is LogoChangedEvent);
            events.Should().NotContain(x => x is UserManualChangedEvent);
        }
    }
}