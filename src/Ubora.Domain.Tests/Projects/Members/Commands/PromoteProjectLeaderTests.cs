using FluentAssertions;
using System;
using System.Linq;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Domain.Projects.Members.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members.Commands
{
    public class PromoteProjectLeaderTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _secoundUserId = Guid.NewGuid();

        private ICommandResult _lastCommandResult;

        [Fact]
        public void Management_Group_Can_Promote_Project_Leader_From_Project()
        {
            this.Given(_ => There_Is_Project_And_User())
                    .And(_ => Add_Member_To_Project(_userId))
                    .And(_ => Add_Member_To_Project(_secoundUserId))
                .When(_ => Management_Group_Promotes_Project_Leader_From_Project())
                .Then(_ => Assert_User_Is_Project_Leader_In_Project())
                .Then(_ => Assert_ProjectLeaderPromoted_Is_Added_In_Events())
                .BDDfy();
        }

        private void There_Is_Project_And_User()
        {
            this.Create_Project(_projectId);
            this.Create_User(_userId, "email", "firstName", "lastName");
            this.Create_User(_secoundUserId, "secoundEmail", "seocundFirstName", "secoundLastName");
        }

        private void Add_Member_To_Project(Guid userId)
        {
            _lastCommandResult = Processor.Execute(new AddMemberToProjectCommand
            {
                UserId = userId,
                ProjectId = _projectId,
                Actor = new UserInfo(_userId, "")
            });
        }

        private void Management_Group_Promotes_Project_Leader_From_Project()
        {
            _lastCommandResult = Processor.Execute(new PromoteProjectLeaderCommand
            {
                UserId = _secoundUserId,
                Actor = new UserInfo(_userId, ""),
                ProjectId = _projectId
            });
        }

        private void Assert_User_Is_Project_Leader_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            project.DoesSatisfy(new HasLeader(_secoundUserId)).Should().BeTrue();

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void Assert_ProjectLeaderPromoted_Is_Added_In_Events()
        {
            var projectLeaderPromotedEvents = Session.Events.QueryRawEventDataOnly<ProjectLeaderPromotedEvent>();

            projectLeaderPromotedEvents.Count().Should().Be(1);
            projectLeaderPromotedEvents.First().InitiatedBy.UserId.Should().Be(_userId);
            projectLeaderPromotedEvents.First().ProjectId.Should().Be(_projectId);
            projectLeaderPromotedEvents.First().UserId.Should().Be(_secoundUserId);
        }
    }
}
