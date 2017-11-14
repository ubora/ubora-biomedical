using System;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Xunit;
using Ubora.Domain.Projects._Events;
using System.Linq;

namespace Ubora.Domain.Tests.Projects._Commands
{
    public class UpdateProjectTitleAndDescriptionTests : IntegrationFixture
    {
        [Fact]
        public void Updates_Project_Description_And_Title_When_Changed()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                Title = "title"
            });

            var newDescription = "newDescription";
            var newTitle = "newtitle";
            var command = new UpdateProjectTitleAndDescriptionCommand
            {
                ProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                Description = newDescription,
                Title = newTitle
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(projectId);

            project.Description.Should().Be(newDescription);
            project.Title.Should().Be(newTitle);

            var projectDescriptionEditedEvents = Session.Events.QueryRawEventDataOnly<EditProjectDescriptionEvent>();
            projectDescriptionEditedEvents.Count().Should().Be(1);
            projectDescriptionEditedEvents.First().Description.Should().Be(newDescription);
            projectDescriptionEditedEvents.First().ProjectId.Should().Be(projectId);

            var projectTitleEditedEvents = Session.Events.QueryRawEventDataOnly<ProjectTitleEditedEvent>();
            projectTitleEditedEvents.Count().Should().Be(1);
            projectTitleEditedEvents.First().Title.Should().Be(newTitle);
            projectDescriptionEditedEvents.First().ProjectId.Should().Be(projectId);
        }

        [Fact]
        public void Updates_Project_Description_When_Changed()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                Title = "title"
            });

            var newDescription = "newDescription";
            var command = new UpdateProjectTitleAndDescriptionCommand
            {
                ProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                Description = newDescription,
                Title = "title"
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(projectId);

            project.Description.Should().Be(newDescription);
            project.Title.Should().Be("title");

            var projectDescriptionEditedEvents = Session.Events.QueryRawEventDataOnly<EditProjectDescriptionEvent>();
            projectDescriptionEditedEvents.Count().Should().Be(1);
            projectDescriptionEditedEvents.First().Description.Should().Be(newDescription);
            projectDescriptionEditedEvents.First().ProjectId.Should().Be(projectId);

            var projectTitleEditedEvents = Session.Events.QueryRawEventDataOnly<ProjectTitleEditedEvent>();
            projectTitleEditedEvents.Count().Should().Be(0);
        }

        [Fact]
        public void Updates_Project_Title_When_Changed()
        {
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                Title = "title"
            });

            var newTitle = "newTitle";
            var command = new UpdateProjectTitleAndDescriptionCommand
            {
                ProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                Title = newTitle
            };

            // Act
            Processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(projectId);

            project.Description.Should().Be(null);
            project.Title.Should().Be(newTitle);

            var projectDescriptionEditedEvents = Session.Events.QueryRawEventDataOnly<EditProjectDescriptionEvent>();
            projectDescriptionEditedEvents.Count().Should().Be(0);

            var projectTitleEditedEvents = Session.Events.QueryRawEventDataOnly<ProjectTitleEditedEvent>();
            projectTitleEditedEvents.Count().Should().Be(1);
            projectTitleEditedEvents.First().Title.Should().Be(newTitle);
            projectTitleEditedEvents.First().ProjectId.Should().Be(projectId);
        }
    }
}
