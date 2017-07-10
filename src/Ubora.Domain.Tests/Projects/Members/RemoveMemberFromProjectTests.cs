using FluentAssertions;
using System;
using System.Linq;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class RemoveMemberFromProjectTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        private ICommandResult _lastCommandResult;

        [Fact]
        public void Project_Leader_Can_Remove_Member_User_From_Project()
        {
            this.Given(_ => There_Is_Project_And_User())
                .When(_ => Project_Add_Member_To_Project())
                .Then(_ => User_Is_Member_In_Project())
                .When(_ => Project_Leader_Removes_Member_User_From_Project())
                .Then(_ => User_Is_Not_Member_In_Project())
                .BDDfy();
        }

        [Fact]
        public void Project_Leader_Can_Remove_Member_User_in_Multiple_Roles_From_Project()
        {
            this.Given(_ => There_Is_Project_And_User())
                .When(_ => Project_Add_Member_To_Project())
                .When(_ => this.Assign_Project_Mentor(_projectId, _userId))
                .Then(_ => User_Is_Multiple_Roles_In_Project())
                .When(_ => Project_Leader_Removes_Member_User_From_Project())
                .Then(_ => User_Is_Not_Member_In_Project())
                .BDDfy();
        }

        private void There_Is_Project_And_User()
        {
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = _projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = _userId
            });
        }

        private void Project_Leader_Removes_Member_User_From_Project()
        {
            _lastCommandResult = Processor.Execute(new RemoveMemberFromProjectCommand
            {
                UserId = _userId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                ProjectId = _projectId
            });
        }

        private void User_Is_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            project.DoesSatisfy(new HasMember(_userId)).Should().BeTrue();

            var addedMember = project.Members.Last();
            addedMember.UserId.Should().Be(_userId);
            addedMember.Should().BeOfType<ProjectMember>();
        }

        private void User_Is_Multiple_Roles_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            project.DoesSatisfy(new HasMember(_userId)).Should().BeTrue();

            var members = project.Members.Where(x => x.UserId == _userId);
            members.Count().Should().BeGreaterThan(1);
        }

        private void Project_Add_Member_To_Project()
        {
            _lastCommandResult = Processor.Execute(new AddMemberToProjectCommand
            {
                UserId = _userId,
                ProjectId = _projectId,
                Actor = new UserInfo(_userId, "")
            });
        }

        private void User_Is_Not_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            project.DoesSatisfy(new HasMember(_userId)).Should().BeFalse();

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }
    }
}
