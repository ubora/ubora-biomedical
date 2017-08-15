using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications.Invitation
{
    public class DeclineInvitationToProjectCommand : UserCommand
    {
        public Guid InvitationId { get; set; }

        internal class DeclineInvitationToProjectCommandHandler : ICommandHandler<DeclineInvitationToProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public DeclineInvitationToProjectCommandHandler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeclineInvitationToProjectCommand command)
            {
                var invite = _documentSession.LoadOrThrow<InvitationToProject>(command.InvitationId);

                invite.Decline();

                _documentSession.Store(invite);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
