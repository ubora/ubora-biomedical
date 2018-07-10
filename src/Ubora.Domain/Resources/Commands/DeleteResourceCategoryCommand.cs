using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources.Commands
{
    public class DeleteResourceCategoryCommand : UserCommand
    {
        public Guid ResourceCategoryId { get; set; }

        public class Handler : ICommandHandler<DeleteResourceCategoryCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeleteResourceCategoryCommand cmd)
            {
                var category = _documentSession.LoadOrThrow<ResourceCategory>(cmd.ResourceCategoryId);

                _documentSession.Delete<ResourceCategory>(category.Id);
                _documentSession.Events.Append(category.Id, new ResourceCategoryDeletedEvent(cmd.Actor, category.Id));
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
