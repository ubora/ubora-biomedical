﻿using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class AcceptWorkpackageOneReviewCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Workpackage_Review_Can_Be_Accepted()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .When(_ => this.Accept_Workpackage_One_Review(_projectId))
                .Then(_ => this.Workpackage_One_Should_Have_Accepted_Review())
                    .And(_ => this.Workpackage_Two_Should_Be_Available())
                    .And(_ => this.Project_Should_Not_Be_In_Draft_Anymore())
                    .And(_ => this.Assert_Project_Device_Structured_Information_Aggregate_Was_Created())
                .BDDfy();
        }

        protected void Workpackage_One_Should_Have_Accepted_Review()
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(_projectId);

            var review = workpackageOne.Reviews.Single();

            review.Status.Should().Be(WorkpackageReviewStatus.Accepted);
            review.ConcludedAt.Should().BeCloseTo(DateTimeOffset.Now, 500);
        }

        protected void Workpackage_Two_Should_Be_Available()
        {
            var workpackageTwo = Processor.FindById<WorkpackageTwo>(_projectId);

            workpackageTwo.Should().NotBeNull();
        }

        protected void Project_Should_Not_Be_In_Draft_Anymore()
        {
            var project = Processor.FindById<Project>(_projectId);

            project.IsInDraft.Should().BeFalse();
        }

        private void Assert_Project_Device_Structured_Information_Aggregate_Was_Created()
        {
            var aggregate = Session.Query<DeviceStructuredInformation>().SingleOrDefault(x => x.ProjectId == _projectId);

            var expectedAggregate = new DeviceStructuredInformation()
                .Set(x => x.ProjectId, _projectId)
                .Set(x => x.WorkpackageType, DeviceStructuredInformationWorkpackageTypes.Two)
                .Set(x => x.UserAndEnvironment, UserAndEnvironmentInformation.CreateEmpty())
                .Set(x => x.HealthTechnologySpecification, new HealthTechnologySpecificationsInformation());

            aggregate.Should().NotBeNull();
            aggregate.ShouldBeEquivalentTo(expectedAggregate, opt => opt.Excluding(x => x.Id));
        }
    }
}
