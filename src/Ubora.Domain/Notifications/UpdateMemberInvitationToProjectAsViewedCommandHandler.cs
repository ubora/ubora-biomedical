using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications
{
    public class UpdateMemberInvitationToProjectAsViewedCommandHandler : ICommandHandler<UpdateMemberInvitationToProjectAsViewedCommand>
    {
        private readonly IDocumentSession _documentSession;

        public UpdateMemberInvitationToProjectAsViewedCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(UpdateMemberInvitationToProjectAsViewedCommand command)
        {
            var invite = _documentSession.Load<InvitationToProject>(command.InvitationId);
            invite.State = InvitationToProjectState.Viewed;

            _documentSession.Store(invite);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
