using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Resources
{
    public class DeleteResourceCommand : UserCommand
    {
        public Guid ResourceId { get; set; }

        public class Handler : ICommandHandler<DeleteResourceCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            public ICommandResult Handle(DeleteResourceCommand cmd)
            {
                var resourcePage = _documentSession.LoadOrThrow<Resource>(cmd.ResourceId);

                _documentSession.Events.Append(cmd.ResourceId, new ResourceDeletedEvent(
                    initiatedBy: cmd.Actor,
                    resourceId: cmd.ResourceId));
                _documentSession.Delete(resourcePage);
                _documentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}