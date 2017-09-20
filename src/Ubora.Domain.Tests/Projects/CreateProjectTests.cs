using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Xunit;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects._Commands;

namespace Ubora.Domain.Tests.Projects
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
                AreaOfUsage = "expectedAreaOfUsage",
                ClinicalNeed = "expectedClinicalNeed",
                Gmdn = "expectedGmdnTerm",
                PotentialTechnology = "expectedPotentialTechnology",
                Actor = new UserInfo(Guid.NewGuid(), "")
            };

            this.Given(x => Given_Command_Is_Handled(command))
                .Then(x => Then_Project_Should_Be_Created(command))
                .Then(x => Then_Creator_Should_Be_First_Member(command))
                .Then(x => Then_Workpackage_One_Is_Opened(command))
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

        private void Then_Workpackage_One_Is_Opened(CreateProjectCommand command)
        {
            var workpackageOne = Session.Load<WorkpackageOne>(command.NewProjectId);

            workpackageOne.Should().NotBeNull();

            workpackageOne.Title.Should().Be("Medical need and product specification");

            var workpackageOneSteps = workpackageOne.Steps.ToArray();

            workpackageOneSteps[0].Title.Should().Be("Description of Needs");
            workpackageOneSteps[0].Description.Should().Be(Placeholders.DescriptionOfNeeds);

            workpackageOneSteps[1].Title.Should().Be("Description of Existing Solutions and Analysis");
            workpackageOneSteps[1].Description.Should().Be(Placeholders.DescriptionOfExistingSolutionsAndAnalysis);

            workpackageOneSteps[2].Title.Should().Be("Product Functionality");
            workpackageOneSteps[2].Description.Should().Be(Placeholders.ProductFunctionality);

            workpackageOneSteps[3].Title.Should().Be("Product Performance");
            workpackageOneSteps[3].Description.Should().Be(Placeholders.ProductPerformance);

            workpackageOneSteps[4].Title.Should().Be("Product Usability");
            workpackageOneSteps[4].Description.Should().Be(Placeholders.ProductUsability);

            workpackageOneSteps[5].Title.Should().Be("Product Safety");
            workpackageOneSteps[5].Description.Should().Be(Placeholders.ProductSafety);

            workpackageOneSteps[6].Title.Should().Be("Patient Population Study");
            workpackageOneSteps[6].Description.Should().Be(Placeholders.PatientPopulationStudy);

            workpackageOneSteps[7].Title.Should().Be("User Requirement Study");
            workpackageOneSteps[7].Description.Should().Be(Placeholders.UserRequirementStudy);

            workpackageOneSteps[8].Title.Should().Be("Additional Information");
            workpackageOneSteps[8].Description.Should().Be(Placeholders.AdditionalInformation);

        }
    }
}
