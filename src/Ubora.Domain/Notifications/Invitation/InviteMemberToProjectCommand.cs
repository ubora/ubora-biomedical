using Ubora.Domain.Infrastructure.Commands;
using Marten;
using System.Linq;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Domain.Notifications.Invitation
{
    public class InviteMemberToProjectCommand : UserProjectCommand
    {
        public string InvitedMemberEmail { get; set; }

        internal class Handler : ICommandHandler<InviteMemberToProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
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
                    return new CommandResult($"[{cmd.InvitedMemberEmail}] is already member of project [{project.Title}].");
                }

                var invite = new InvitationToProject(userProfile.UserId, cmd.ProjectId);

                _documentSession.Store(invite);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
