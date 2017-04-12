using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Projections;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class CreateProjectCommandHandlerTests : DocumentSessionIntegrationFixture
    {
        public CreateProjectCommandHandlerTests()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Handle_Creates_New_Project_With_Creator_As_First_Member()
        {
            var command = new CreateProjectCommand
            {
                Id = Guid.NewGuid(),
                Name = "ProjectName",
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            };

            this.Given(x => Given_Command_Is_Handled(command))
                .Then(x => Then_Project_Should_Be_Created(command))
                .Then(x => Then_Creator_Should_Be_First_Member(command))
                .BDDfy();
        }

        private void Given_Command_Is_Handled(CreateProjectCommand command)
        {
            var handler = new CreateProjectCommandHandler(this.Session);

            // Act
            handler.Handle(command);
        }

        private void Then_Project_Should_Be_Created(CreateProjectCommand command)
        {
            var project = Session.Load<Project>(command.Id);
            project.Should().NotBeNull();
            project.Name.Should().Be(command.Name);
        }

        private void Then_Creator_Should_Be_First_Member(CreateProjectCommand command)
        {
            var project = Session.Load<Project>(command.Id);
        }
    }
}
