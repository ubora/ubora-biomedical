using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications
{
    public class DeclineMemberInvitationToProjectCommandHandler : ICommandHandler<DeclineMemberInvitationToProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public DeclineMemberInvitationToProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(DeclineMemberInvitationToProjectCommand command)
        {
            var invite = _documentSession.Load<InvitationToProject>(command.InvitationId);
            invite.State = InvitationToProjectState.Declined;

            _documentSession.Store(invite);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
