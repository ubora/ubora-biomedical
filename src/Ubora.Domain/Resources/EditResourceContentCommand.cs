using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Resources
{
    public class EditResourceContentCommand : UserCommand
    {
        public Guid ResourceId { get; set; }
        public ResourceContent Content { get; set; }
        public Guid PreviousContentVersion { get; set; }

        public class Handler : ICommandHandler<EditResourceContentCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            public ICommandResult Handle(EditResourceContentCommand cmd)
            {
                _documentSession.LoadOrThrow<ResourcePage>(cmd.ResourceId);

                _documentSession.Events.Append(
                    stream: cmd.ResourceId, 
                    events: new ResourceContentEditedEvent(
                        initiatedBy: cmd.Actor,
                        content: cmd.Content,
                        previousContentVersion: cmd.PreviousContentVersion));
                
                _documentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}