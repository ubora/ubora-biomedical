using FluentAssertions;
using System;
using System.Linq;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Commands;
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
                    .And(_ => Add_Member_To_Project())
                .When(_ => Project_Leader_Removes_Member_User_From_Project())
                .Then(_ => Assert_User_Is_Not_Member_In_Project())
                .BDDfy();
        }

        [Fact]
        public void Project_Leader_Can_Remove_Member_User_in_Multiple_Roles_From_Project()
        {
            this.Given(_ => There_Is_Project_And_User())
                    .And(_ => Add_Member_To_Project())
                    .And(_ => this.Assign_Project_Mentor(_projectId, _userId))
                .When(_ => Project_Leader_Removes_Member_User_From_Project())
                .Then(_ => Assert_User_Is_Not_Member_In_Project())
                .BDDfy();
        }

        private void There_Is_Project_And_User()
        {
            this.Create_Project(_projectId);
            this.Create_User(_userId);
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

        private void Add_Member_To_Project()
        {
            _lastCommandResult = Processor.Execute(new AddMemberToProjectCommand
            {
                UserId = _userId,
                ProjectId = _projectId,
                Actor = new UserInfo(_userId, "")
            });
        }

        private void Assert_User_Is_Not_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            project.DoesSatisfy(new HasMember(_userId)).Should().BeFalse();

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }
    }
}
