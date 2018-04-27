using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Resources
{
    public class CreateResourcePageCommand : UserCommand
    {
        public Guid ResourceId { get; set; }
        public ResourceContent Content { get; set; }

        internal class Handler : ICommandHandler<CreateResourcePageCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            public ICommandResult Handle(CreateResourcePageCommand cmd)
            {
                if (_documentSession.Load<ResourcePage>(cmd.ResourceId) != null)
                {
                    throw new InvalidOperationException("Resource page with given ID already exists.");
                }

                _documentSession.Events.StartStream(
                    id: cmd.ResourceId, 
                    events: new ResourceCreatedEvent(
                        initiatedBy: cmd.Actor,
                        resourceId: cmd.ResourceId,
                        slug: Slug.Generate(cmd.Content.Title),
                        content: cmd.Content));
                
                _documentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}