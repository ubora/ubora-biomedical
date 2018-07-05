using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources.Commands
{
    public class CreateResourcePageCommand : UserCommand
    {
        public Guid ResourcePageId { get; set; }
        public string Title { get; set; }
        public QuillDelta Body { get; set; }
        public int MenuPriority { get; set; }
        public Guid ParentCategoryId { get; set; }

        internal class Handler : ICommandHandler<CreateResourcePageCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            public ICommandResult Handle(CreateResourcePageCommand cmd)
            {
                if (_documentSession.Load<ResourcePage>(cmd.ResourcePageId) != null)
                {
                    throw new InvalidOperationException("Resource page with given ID already exists.");
                }

                _documentSession.Events.StartStream(
                    id: cmd.ResourcePageId, 
                    events: new ResourcePageCreatedEvent(
                        initiatedBy: cmd.Actor,
                        resourcePageId: cmd.ResourcePageId,
                        slug: Slug.Generate(cmd.Title),
                        title: cmd.Title,
                        body: cmd.Body,
                        menuPriority: cmd.MenuPriority,
                        parentCategoryId: cmd.ParentCategoryId));
                
                _documentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}