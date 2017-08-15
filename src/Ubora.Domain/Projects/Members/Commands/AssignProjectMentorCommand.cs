using System;
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
                    return new CommandResult("User is already project mentor.");
                }

                // TODO(!!!)
                var invite = new ProjectMentorInvitation(userProfile.UserId, project.Id, Guid.Empty);

                DocumentSession.Store(invite);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
