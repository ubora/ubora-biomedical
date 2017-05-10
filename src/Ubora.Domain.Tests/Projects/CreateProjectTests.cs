using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects;
using Xunit;
using Ubora.Domain.Projects.Members;

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
            var command = new CreateProjectCommand
            {
                Id = Guid.NewGuid(),
                Title = "ProjectName",
                AreaOfUsage = "expectedAreaOfUsage",
                ClinicalNeed = "expectedClinicalNeed",
                GmdnCode = "expectedGmdnCode",
                GmdnDefinition = "expectedGmdnDefinition",
                GmdnTerm = "expectedGmdnTerm",
                PotentialTechnology = "expectedPotentialTechnology",
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            };

            this.Given(x => Given_Command_Is_Handled(command))
                .Then(x => Then_Project_Should_Be_Created(command))
                .Then(x => Then_Creator_Should_Be_First_Member(command))
                .BDDfy();
        }

        private void Given_Command_Is_Handled(CreateProjectCommand command)
        {
            var commandProcessor = Container.Resolve<ICommandProcessor>();

            // Act
            commandProcessor.Execute(command);
        }

        private void Then_Project_Should_Be_Created(CreateProjectCommand command)
        {
            var project = Session.Load<Project>(command.Id);
            project.Should().NotBeNull();

            project.Id.Should().Be(command.Id);
            project.Title.Should().Be(command.Title);
            project.AreaOfUsageTags.Should().Be("expectedAreaOfUsage");
            project.ClinicalNeedTags.Should().Be("expectedClinicalNeed");
            project.GmdnCode.Should().Be("expectedGmdnCode");
            project.GmdnDefinition.Should().Be("expectedGmdnDefinition");
            project.GmdnTerm.Should().Be("expectedGmdnTerm");
            project.PotentialTechnologyTags.Should().Be("expectedPotentialTechnology");
        }

        private void Then_Creator_Should_Be_First_Member(CreateProjectCommand command)
        {
            var project = Session.Load<Project>(command.Id);
            var onlyMember = project.Members.Single();
            onlyMember.As<ProjectLeader>().UserId.Should().Be(command.UserInfo.UserId);
        }
    }
}
