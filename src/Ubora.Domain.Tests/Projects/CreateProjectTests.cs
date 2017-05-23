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
using Ubora.Domain.Projects.WorkpackageOnes;

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
                NewProjectId = Guid.NewGuid(),
                Title = "ProjectName",
                AreaOfUsage = "expectedAreaOfUsage",
                ClinicalNeed = "expectedClinicalNeed",
                Gmdn = "expectedGmdnTerm",
                PotentialTechnology = "expectedPotentialTechnology",
                Actor = new UserInfo(Guid.NewGuid(), "")
            };

            this.Given(x => Given_Command_Is_Handled(command))
                .Then(x => Then_Project_Should_Be_Created(command))
                .Then(x => Then_Creator_Should_Be_First_Member(command))
                .Then(x => Then_Workpackage_One_Is_Created(command))
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
            var project = Session.Load<Project>(command.NewProjectId);
            project.Should().NotBeNull();

            project.Id.Should().Be(command.NewProjectId);
            project.Title.Should().Be(command.Title);
            project.AreaOfUsageTags.Should().Be("expectedAreaOfUsage");
            project.ClinicalNeedTags.Should().Be("expectedClinicalNeed");
            project.Gmdn.Should().Be("expectedGmdnTerm");
            project.PotentialTechnologyTags.Should().Be("expectedPotentialTechnology");
        }

        private void Then_Creator_Should_Be_First_Member(CreateProjectCommand command)
        {
            RefreshSession();

            var project = Session.Load<Project>(command.NewProjectId);

            var onlyMember = project.Members.Single();
            onlyMember.As<ProjectLeader>().UserId.Should().Be(command.Actor.UserId);
        }

        private void Then_Workpackage_One_Is_Created(CreateProjectCommand command)
        {
            var workpackageOne = Session.Load<WorkpackageOne>(command.NewProjectId);

            workpackageOne.Should().NotBeNull();
        }
    }
}
