using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class InviteProjectMentorCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }

        internal class Handler : CommandHandler<InviteProjectMentorCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(InviteProjectMentorCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var userProfile = DocumentSession.LoadOrThrow<UserProfile>(cmd.UserId);

                var isAlreadyMentor = project.DoesSatisfy(new HasMember<ProjectMentor>(cmd.UserId));
                if (isAlreadyMentor)
                {
                    return new CommandResult("User is already a mentor of this project.");
                }

                var isAlreadyInvited = DocumentSession.Query<ProjectMentorInvitation>()
                    .Any(x => x.ProjectId == project.Id && x.InviteeUserId == cmd.UserId && x.IsPending);
                if (isAlreadyInvited)
                {
                    return new CommandResult($"{userProfile.FullName} already has a pending mentor invitation to this project.");
                }

                var invite = new ProjectMentorInvitation(userProfile.UserId, project.Id, cmd.Actor.UserId);

                DocumentSession.Store(invite);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
