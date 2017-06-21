using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;

namespace Ubora.Domain.Notifications.Join
{
    public class DeclineRequestToJoinProjectCommand : UserCommand
    {
        public Guid RequestId { get; set; }
    }

    internal class DeclineRequestToJoinProjectCommandHandler : ICommandHandler<DeclineRequestToJoinProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public DeclineRequestToJoinProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(DeclineRequestToJoinProjectCommand command)
        {
            var request = _documentSession.Load<RequestToJoinProject>(command.RequestId);
            if (request == null) throw new InvalidOperationException();

            request.Declined = DateTime.UtcNow;

            _documentSession.Store(request);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
