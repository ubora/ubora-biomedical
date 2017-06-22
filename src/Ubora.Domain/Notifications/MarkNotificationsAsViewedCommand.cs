using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications
{
    public class MarkNotificationsAsViewedCommand : UserCommand
    {
        public Guid UserId { get; set; }
    }

    internal class MarkNotificationsAsViewedCommandHandler : ICommandHandler<MarkNotificationsAsViewedCommand>
    {
        private readonly IDocumentSession _documentSession;

        public MarkNotificationsAsViewedCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(MarkNotificationsAsViewedCommand cmd)
        {
            var specification = new UnViewedNotifications(cmd.UserId);
            var query = _documentSession.Query<BaseNotification>();
            var invitations = specification.SatisfyEntitiesFrom(query).ToArray();

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
