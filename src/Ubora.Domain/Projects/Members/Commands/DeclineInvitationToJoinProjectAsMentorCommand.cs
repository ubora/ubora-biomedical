using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class DeclineInvitationToJoinProjectAsMentorCommand : UserCommand
    {
        public Guid InvitationId { get; set; }

        internal class Handler : ICommandHandler<DeclineInvitationToJoinProjectAsMentorCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeclineInvitationToJoinProjectAsMentorCommand command)
            {
                var invite = _documentSession.LoadOrThrow<ProjectMentorInvitation>(command.InvitationId);

                invite.Decline();

                _documentSession.Store(invite);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}