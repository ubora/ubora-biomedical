using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class AssignProjectMentorCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void Mentor_Can_Be_Assigned_To_Project()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Create_User(_userId))
                .When(_ => this.Assign_Project_Mentor(_projectId, _userId))
                .Then(_ => this.AssertUserIsProjectMentor())
                .BDDfy();
        }

        [Fact]
        public void Duplicate_Mentor_Can_Not_Be_Assigned_To_Project()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Create_User(_userId))
                    .And(_ => this.Assign_Project_Mentor(_projectId, _userId))
                .When(_ => this.Assign_Project_Mentor(_projectId, _userId))
                .Then(_ => this.Then_User_Is_Not_Duplicate_Project_Mentor())
                .BDDfy();
        }

        private void AssertUserIsProjectMentor()
        {
            var project = Session.Load<Project>(_projectId);

            var isUserProjectMentor = project.DoesSatisfy(new HasMember<ProjectMentor>(_userId));

            isUserProjectMentor.Should().BeTrue();
        }

        private void Then_User_Is_Not_Duplicate_Project_Mentor()
        {
            var project = Session.Load<Project>(_projectId);

            project.Members.OfType<ProjectMentor>()
                .Count(m => m.UserId == _userId)
                .Should().Be(1);
        }
    }
}
