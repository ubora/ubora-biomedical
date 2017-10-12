using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
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
                    return CommandResult.Failed($"Email [{cmd.InvitedMemberEmail}] not found.");
                }

                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var isUserAlreadyMember = project.Members.Any(m => m.UserId == userProfile.UserId);
                if (isUserAlreadyMember)
                {
                    return CommandResult.Failed($"[{cmd.InvitedMemberEmail}] is already member of project [{project.Title}].");
                }

                var invite = new InvitationToProject(userProfile.UserId, cmd.ProjectId);

                _documentSession.Store(invite);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
