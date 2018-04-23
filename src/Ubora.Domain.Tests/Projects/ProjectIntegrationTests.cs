using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class ProjectIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void GetMembers_Returns_Specified_Members()
        {
            var projectId = Guid.NewGuid();
            var leaderId = Guid.NewGuid();
            var mentorId1 = Guid.NewGuid();
            var mentorId2 = Guid.NewGuid();

            this.Create_UserProfiles(leaderId, mentorId1, mentorId2);
            this.Create_Project(projectId: projectId, userId: leaderId);
            this.Assign_Project_Mentor(projectId: projectId, userId: mentorId1);
            this.Assign_Project_Mentor(projectId: projectId, userId: mentorId2);

            var project = Session.Load<Project>(projectId);

            // Act
            var leaderResult = project.GetMembers(new IsLeaderSpec());
            var mentorResult = project.GetMembers(new IsMentorSpec());
            var everyMemberResult = project.GetMembers();

            // Assert
            leaderResult.Select(x => x.UserId).Should().OnlyContain(x => x == leaderId);
            mentorResult.Select(x => x.UserId).Should().BeEquivalentTo(new[] {mentorId1, mentorId2});
            everyMemberResult.Select(x => x.UserId).Should().BeEquivalentTo(new[] {leaderId, mentorId1, mentorId2});
        }
    }
}