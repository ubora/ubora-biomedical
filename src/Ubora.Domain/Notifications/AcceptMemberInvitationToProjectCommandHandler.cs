using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Notifications
{
    public class AcceptMemberInvitationToProjectCommandHandler : ICommandHandler<AcceptMemberInvitationToProjectCommand>
    {
        private readonly IDocumentSession _documentSession;
        private ICommandProcessor _commandProcessor;

        public AcceptMemberInvitationToProjectCommandHandler(
            IDocumentSession documentSession,
            ICommandProcessor commandProcessor)
        {
            _documentSession = documentSession;
            _commandProcessor = commandProcessor;
        }

        public ICommandResult Handle(AcceptMemberInvitationToProjectCommand command)
        {
            var invite = _documentSession.Load<InvitationToProject>(command.InvitationId);
            invite.IsAccepted = true;

            _documentSession.Store(invite);
            _documentSession.SaveChanges();

            _commandProcessor.Execute(new AddMemberToProjectCommand
            {
                Actor = command.Actor,
                ProjectId = invite.ProjectId,
                UserId = invite.InvitedMemberId
            });

            return new CommandResult();
        }
    }
}
