using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications
{
    public class MarkInvitationsAsViewedCommand : UserCommand
    {
        public Guid UserId { get; set; }
    }

    public class MarkInvitationsAsViewedCommandHandler : ICommandHandler<MarkInvitationsAsViewedCommand>
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IDocumentSession _documentSession;

        public MarkInvitationsAsViewedCommandHandler(IDocumentSession documentSession, ICommandQueryProcessor processor)
        {
            _processor = processor;
            _documentSession = documentSession;
        }

        public ICommandResult Handle(MarkInvitationsAsViewedCommand cmd)
        {
            var specification = new NonViewedInvitations(cmd.UserId);
            var query = _documentSession.Query<InvitationToProject>();
            var invitations = specification.SatisfyEntitiesFrom(query)
                .ToArray();

            foreach (var invitation in invitations)
            {
                invitation.HasBeenViewed = true;
            }

            _documentSession.Store(invitations);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
