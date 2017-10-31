using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
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
        public void Foo()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => this.Delete_Project())
                .Then(_ => this.Assert_No_Events_For_Project_Exist())
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
    }
}
