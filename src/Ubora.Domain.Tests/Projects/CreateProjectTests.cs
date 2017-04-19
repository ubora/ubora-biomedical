using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Projections;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class CreateProjectTests : IntegrationFixture
    {
        public CreateProjectTests()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Handle_Creates_New_Project_With_Creator_As_First_Member()
        {
            var command = new CreateProject
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

        private void Given_Command_Is_Handled(CreateProject command)
        {
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            // Act
            commandProcessor.Execute(command);
        }

        private void Then_Project_Should_Be_Created(CreateProject command)
        {
            var project = Session.Load<Project>(command.Id);
            project.Should().NotBeNull();
            project.Name.Should().Be(command.Name);
        }

        private void Then_Creator_Should_Be_First_Member(CreateProject command)
        {
            var project = Session.Load<Project>(command.Id);
            var onlyMember = project.Members.Single();
            onlyMember.As<Leader>().UserId.Should().Be(command.UserInfo.UserId);
        }
    }
}
