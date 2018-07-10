using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources.Commands
{
    public class CreateResourceCategoryCommand : UserCommand
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int MenuPriority { get; set; }

        public class Handler : ICommandHandler<CreateResourceCategoryCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(CreateResourceCategoryCommand cmd)
            {
                _documentSession.Events.StartStream(
                    cmd.CategoryId, 
                    new ResourceCategoryEditedEvent(
                        initiatedBy: cmd.Actor,
                        categoryId: cmd.CategoryId,
                        title: cmd.Title,
                        description: cmd.Description,
                        parentCategoryId: cmd.ParentCategoryId,
                        menuPriority: cmd.MenuPriority));

                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
