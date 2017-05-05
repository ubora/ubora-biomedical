﻿using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects;
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
            var command = new CreateProjectCommand
            {
                ProjectId = Guid.NewGuid(),
                Title = "ProjectName",
                AreaOfUsage = "expectedAreaOfUsage",
                ClinicalNeed = "expectedClinicalNeed",
                Description = "expectedDescription",
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
            var project = Session.Load<Project>(command.ProjectId);
            project.Should().NotBeNull();

            project.Title.Should().Be(command.Title);
            project.AreaOfUsage.Should().Be("expectedAreaOfUsage");
            project.ClinicalNeed.Should().Be("expectedClinicalNeed");
            project.Description.Should().Be("expectedDescription");
            project.GmdnCode.Should().Be("expectedGmdnCode");
            project.GmdnDefinition.Should().Be("expectedGmdnDefinition");
            project.GmdnTerm.Should().Be("expectedGmdnTerm");
            project.PotentialTechnology.Should().Be("expectedPotentialTechnology");
        }

        private void Then_Creator_Should_Be_First_Member(CreateProjectCommand command)
        {
            var project = Session.Load<Project>(command.ProjectId);
            var onlyMember = project.Members.Single();
            onlyMember.As<ProjectLeader>().UserId.Should().Be(command.UserInfo.UserId);
        }
    }
}
