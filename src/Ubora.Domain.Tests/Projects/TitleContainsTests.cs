using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class TitleContainsTests : IntegrationFixture
    {
        [Fact]
        public void Is_Satisfied_By_Projects_Which_Name_Contains_Given_Title()
        {
            var title = "testTitle";

            // Insert dummy project
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Title = title,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Title = Guid.NewGuid().ToString(),
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Title = $"test{title}last",
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Title = $"testTestTiTlelast",
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var sut = new TitleContains(title);

            RefreshSession();

            var projectsQueryable = Session.Query<Project>();

            // Act
            var result = sut.SatisfyEntitiesFrom(projectsQueryable);

            // Assert
            result.Count().Should().Be(3);
        }
    }
}
