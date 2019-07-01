using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.IntellectualProperties;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IntellectualProperties.Commands
{
    public class EditLicenseCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void License_Editing_HappyPath_TestCases() 
        {
            var project = new ProjectSeeder()
                .WithWp1Accepted()
                .WithWp3Unlocked()
                .WithWp4Unlocked()
                .WithWp5Unlocked()
                .Seed(this);

            var actor = new UserInfo(Guid.NewGuid(), "test");

            // Act
            var result = Processor.Execute(new EditLicenseCommand() 
            {
                Actor = actor,
                ProjectId = project.Id,
                NoDerivativeWorks = false,
                Attribution = false,
                ShareAlike = false,
                NonCommercial = false,
                UboraLicense = false
            });

            // Assert
            Session.Events.FetchStream(project.Id).Select(x => (UboraEvent)x.Data).ToList().Should().NotContain(x => x.InitiatedBy.UserId == actor.UserId);
            var intellectualProperty = Session.Load<IntellectualProperty>(project.Id);
            intellectualProperty.License.Should().BeNull();

            // Act
            var result2 = Processor.Execute(new EditLicenseCommand() 
            {
                Actor = actor,
                ProjectId = project.Id,
                NoDerivativeWorks = true,
                Attribution = true,
                ShareAlike = true,
                NonCommercial = true,
                UboraLicense = false
            });

            // Assert
            Session.Events.FetchStream(project.Id).Select(x => (UboraEvent)x.Data).Last().InitiatedBy.Should().Be(actor);
            intellectualProperty = Session.Load<IntellectualProperty>(project.Id);
            intellectualProperty.License.Should().BeOfType<CreativeCommonsLicense>();
            var creativeCommonsLicense = (CreativeCommonsLicense)intellectualProperty.License;
            creativeCommonsLicense.NoDerivativeWorks.Should().BeTrue();
            creativeCommonsLicense.Attribution.Should().BeTrue();
            creativeCommonsLicense.ShareAlike.Should().BeTrue();
            creativeCommonsLicense.NonCommercial.Should().BeTrue();

            // Act
            var result3 = Processor.Execute(new EditLicenseCommand() 
            {
                Actor = actor,
                ProjectId = project.Id,
                NoDerivativeWorks = true,
                Attribution = true,
                ShareAlike = true,
                NonCommercial = true,
                UboraLicense = true
            });

            // Assert
            Session.Events.FetchStream(project.Id).Select(x => (UboraEvent)x.Data).Last().InitiatedBy.Should().Be(actor);
            intellectualProperty = Session.Load<IntellectualProperty>(project.Id);
            intellectualProperty.License.Should().BeOfType<UboraLicense>();

            // Act
            var result4 = Processor.Execute(new EditLicenseCommand() 
            {
                Actor = actor,
                ProjectId = project.Id,
                NoDerivativeWorks = false,
                Attribution = true,
                ShareAlike = false,
                NonCommercial = true
            });

            // Assert
            intellectualProperty = Session.Load<IntellectualProperty>(project.Id);
            creativeCommonsLicense = (CreativeCommonsLicense)intellectualProperty.License;
            creativeCommonsLicense.NoDerivativeWorks.Should().BeFalse();
            creativeCommonsLicense.Attribution.Should().BeTrue();
            creativeCommonsLicense.ShareAlike.Should().BeFalse();
            creativeCommonsLicense.NonCommercial.Should().BeTrue();

            // Act
            var result5 = Processor.Execute(new EditLicenseCommand() 
            {
                Actor = actor,
                ProjectId = project.Id,
                NoDerivativeWorks = true,
                Attribution = false,
                ShareAlike = true,
                NonCommercial = false
            });

            // Assert
            intellectualProperty = Session.Load<IntellectualProperty>(project.Id);
            creativeCommonsLicense = (CreativeCommonsLicense)intellectualProperty.License;
            creativeCommonsLicense.NoDerivativeWorks.Should().BeTrue();
            creativeCommonsLicense.Attribution.Should().BeFalse();
            creativeCommonsLicense.ShareAlike.Should().BeTrue();
            creativeCommonsLicense.NonCommercial.Should().BeFalse();

            // Act
            var result6 = Processor.Execute(new EditLicenseCommand() 
            {
                Actor = actor,
                ProjectId = project.Id,
                NoDerivativeWorks = false,
                Attribution = false,
                ShareAlike = false,
                NonCommercial = false,
                UboraLicense = false
            });

            // Assert
            intellectualProperty = Session.Load<IntellectualProperty>(project.Id);
            intellectualProperty.License.Should().BeNull();
        }
    }
}