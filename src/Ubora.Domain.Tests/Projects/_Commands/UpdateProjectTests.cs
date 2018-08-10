using System;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Commands
{
    public class UpdateProjectTests : IntegrationFixture
    {
        [Fact]
        public void Updates_Project()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var command = new UpdateProjectCommand
            {
                ProjectId = projectId,
                Title = "title",
                ClinicalNeedTags = "clinicalNeedTags",
                AreaOfUsageTags = "areaOfUsageTags",
                PotentialTechnologyTags = "potentialTechnologyTags",
                Gmdn = "gmdn",
                Actor = new UserInfo(Guid.NewGuid(), "")
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(projectId);

            project.Id.Should().Be(projectId);
            project.Title.Should().Be("title");
            project.ClinicalNeedTag.Should().Be("clinicalNeedTags");
            project.AreaOfUsageTag.Should().Be("areaOfUsageTags");
            project.PotentialTechnologyTag.Should().Be("potentialTechnologyTags");
            project.Keywords.Should().Be("gmdn");
        }
    }
}
