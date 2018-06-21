using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources.Commands
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
                var resourcePage = _documentSession.LoadOrThrow<ResourcePage>(cmd.ResourceId);

                if (resourcePage.ContentVersion != cmd.PreviousContentVersion)
                {
                    return CommandResult.Failed("The content has been changed while you were editing. Please refresh the page.");
                }

                _documentSession.Events.Append(
                    stream: cmd.ResourceId, 
                    events: new ResourcePageContentEditedEvent(
                        initiatedBy: cmd.Actor,
                        content: cmd.Content,
                        previousContentVersion: cmd.PreviousContentVersion));
                
                _documentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}