using System;
using System.Linq;
using FluentAssertions;
using Marten.Linq.SoftDeletes;
using TestStack.BDDfy;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Commands
{
    public class DeleteProjectCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Project_Can_Be_Deleted()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => this.Delete_Project())
                //.Then(_ => this.Assert_No_Events_For_Project_Exist())
                .Then(_ => this.Assert_Project_Is_Marked_As_Deleted())
                .BDDfy();
        }

        private void Delete_Project()
        {
            this.Processor.Execute(new DeleteProjectCommand
            {
                ProjectId = _projectId,
                Actor = new DummyUserInfo()
            });
        }

        private void Assert_No_Events_For_Project_Exist()
        {
            var project = this.Session.Load<Project>(_projectId);
            project.Should().BeNull();

            var projectEvents = this.Session.Events
                .QueryAllRawEvents()
                .Where(e => e.StreamId == _projectId);

            projectEvents.Should().BeEmpty();
        }

        private void Assert_Project_Is_Marked_As_Deleted()
        {
            var project = Session.Load<Project>(_projectId);
            project.IsDeleted.Should().BeTrue();

            var softDeletedProject = Session.Query<Project>().Single(p => p.IsDeleted());
            softDeletedProject.Id.Should().Be(project.Id);
        }
    }
}
