using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class BySearchPhraseTests : IntegrationFixture
    {
        [Fact]
        public void Is_Satisfied_By_Projects_Which_Name_Contains_Given_Word()
        {
            var searchPhrase = "testTitle";

            // Insert dummy project
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Title = searchPhrase,
                AreaOfUsage = searchPhrase,
                PotentialTechnology = searchPhrase,
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
                Title = $"test{searchPhrase}last",
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Title = $"testTestTiTlelast",
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                ClinicalNeed = $"testTestTiTlelast",
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Gmdn = $"test{searchPhrase}last",
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var sut = new BySearchPhrase(searchPhrase);

            RefreshSession();

            var projectsQueryable = Session.Query<Project>();

            // Act
            var result = sut.SatisfyEntitiesFrom(projectsQueryable);

            // Assert
            result.Count().Should().Be(5);
        }

        [Fact]
        public void Is_Satisfied_By_Projects_When_Not_Found_Word()
        {
            var searchPhrase = "testTitle";

            var sut = new BySearchPhrase(searchPhrase);

            RefreshSession();

            var projectsQueryable = Session.Query<Project>();

            // Act
            var result = sut.SatisfyEntitiesFrom(projectsQueryable);

            // Assert
            result.Count().Should().Be(0);
        }
    }
}
