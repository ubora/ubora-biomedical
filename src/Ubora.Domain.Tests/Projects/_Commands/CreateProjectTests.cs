using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects._Commands;
using Xunit;
using AutoFixture;
using Ubora.Domain.Tests.ClinicalNeeds;

namespace Ubora.Domain.Tests.Projects._Commands
{
    public class CreateProjectTests : IntegrationFixture
    {
        [Fact]
        public void Handle_Creates_New_Project_With_Creator_As_First_Member()
        {
            var command = new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Title = "ProjectName",
                AreaOfUsageTag = "expectedAreaOfUsage",
                ClinicalNeedTag = "expectedClinicalNeed",
                Keywords = "expectedGmdnTerm",
                PotentialTechnologyTag = "expectedPotentialTechnology",
                Actor = new UserInfo(Guid.NewGuid(), "")
            };

            this.Given(x => Given_Command_Is_Handled(command))
                .Then(x => Then_Project_Should_Be_Created(command))
                .Then(x => Then_Creator_Should_Be_First_Member(command))
                .Then(x => Then_Workpackage_One_Is_Opened(command))
                .BDDfy();
        }

        [Fact]
        public void Handle_Marks_Related_Clinical_Need()
        {
            var clinicalNeedId = Guid.NewGuid();
            new ClinicalNeedSeeder(this, clinicalNeedId)
                .IndicateTheClinicalNeed();

            var command = AutoFixture.Create<CreateProjectCommand>();
            command.RelatedClinicalNeedId = clinicalNeedId;

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var project = Processor.FindById<Project>(command.NewProjectId);

            project.RelatedClinicalNeeds.Should().HaveCount(1);
            project.RelatedClinicalNeeds.Should().Contain(clinicalNeedId);
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
            project.AreaOfUsageTag.Should().Be("expectedAreaOfUsage");
            project.ClinicalNeedTag.Should().Be("expectedClinicalNeed");
            project.Keywords.Should().Be("expectedGmdnTerm");
            project.PotentialTechnologyTag.Should().Be("expectedPotentialTechnology");
            project.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1000);
            project.RelatedClinicalNeeds.Should().BeEmpty();
        }

        private void Then_Creator_Should_Be_First_Member(CreateProjectCommand command)
        {
            var project = Session.Load<Project>(command.NewProjectId);

            var onlyMember = project.Members.Single();
            onlyMember.As<ProjectLeader>().UserId.Should().Be(command.Actor.UserId);
        }

        private void Then_Workpackage_One_Is_Opened(CreateProjectCommand command)
        {
            var workpackageOne = Session.Load<WorkpackageOne>(command.NewProjectId);

            workpackageOne.Should().NotBeNull();

            workpackageOne.Title.Should().Be("Medical need and product specification");

            var workpackageOneSteps = workpackageOne.Steps.ToArray();

            workpackageOneSteps[0].Title.Should().Be("Clinical needs");

            workpackageOneSteps[1].Title.Should().Be("Existing solutions");

            workpackageOneSteps[2].Title.Should().Be("Intended users");

            workpackageOneSteps[3].Title.Should().Be("Product requirements");
        }
    }
}
