using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Domain.Notifications
{
    public class InviteMemberToProjectCommandHandler : ICommandHandler<InviteMemberToProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public InviteMemberToProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(InviteMemberToProjectCommand cmd)
        {
            var userProfile = _documentSession.Query<UserProfile>().SingleOrDefault(x => x.Email == cmd.InvitedMemberEmail);
            if (userProfile == null)
            {
                return new CommandResult($"Email [{cmd.InvitedMemberEmail}] not found.");
            }

            var project = _documentSession.Load<Project>(cmd.ProjectId);

            var isUserAlreadyMember = project.Members.Any(m => m.UserId == userProfile.UserId);
            if (isUserAlreadyMember)
            {
                return new CommandResult($"[{cmd.InvitedMemberEmail}] is already member of project [{cmd.ProjectId}].");
            }

            var invite = new InvitationToProject(Guid.NewGuid())
            {
                InvitedMemberId = userProfile.UserId,
                ProjectId = cmd.ProjectId,
                State = InvitationToProjectState.None
            };

            _documentSession.Store(invite);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
