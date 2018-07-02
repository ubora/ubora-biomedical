using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources.Commands
{
    public class EditResourcePageContentCommand : UserCommand
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }
        public QuillDelta Body { get; set; }
        public int PreviousContentVersion { get; set; }

        public class Handler : ICommandHandler<EditResourcePageContentCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            public ICommandResult Handle(EditResourcePageContentCommand cmd)
            {
                var resourcePage = _documentSession.LoadOrThrow<ResourcePage>(cmd.ResourceId);

                if (resourcePage.BodyVersion != cmd.PreviousContentVersion)
                {
                    return CommandResult.Failed("The content has been changed while you were editing. Please refresh the page.");
                }

                var events = new object[]
                {
                    new ResourcePageBodyEditedEvent(
                        initiatedBy: cmd.Actor,
                        body: cmd.Body,
                        previousBodyVersion: cmd.PreviousContentVersion),
                    new ResourcePageTitleChangedEvent(
                        initiatedBy: cmd.Actor,
                        title: cmd.Title,
                        slug: Slug.Generate(cmd.Title))
                };

                _documentSession.Events.Append(cmd.ResourceId, events);
                
                _documentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}