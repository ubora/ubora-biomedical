using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class DeclineRequestToJoinProjectCommand : UserCommand
    {
        public Guid RequestId { get; set; }

        internal class Handler : ICommandHandler<DeclineRequestToJoinProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeclineRequestToJoinProjectCommand command)
            {
                var request = _documentSession.LoadOrThrow<RequestToJoinProject>(command.RequestId);

                request.Decline();

                _documentSession.Store(request);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
