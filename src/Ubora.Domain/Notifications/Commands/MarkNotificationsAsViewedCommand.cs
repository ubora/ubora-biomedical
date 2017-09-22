using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications.Specifications;

namespace Ubora.Domain.Notifications.Commands
{
    public class MarkNotificationsAsViewedCommand : UserCommand
    {
        internal class Handler : ICommandHandler<MarkNotificationsAsViewedCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(MarkNotificationsAsViewedCommand cmd)
            {
                var specification = new HasUnViewedNotifications(cmd.Actor.UserId);
                var query = _documentSession.Query<INotification>();
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
}