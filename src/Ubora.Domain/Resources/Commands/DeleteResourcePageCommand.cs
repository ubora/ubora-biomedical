using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Resources.Commands
{
    public class DeleteResourcePageCommand : UserCommand
    {
        public Guid ResourcePageId { get; set; }

        public class Handler : ICommandHandler<DeleteResourcePageCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IResourceBlobDeleter _resourceBlobDeleter;

            public Handler(IDocumentSession documentSession, IResourceBlobDeleter resourceBlobDeleter)
            {
                _documentSession = documentSession;
                _resourceBlobDeleter = resourceBlobDeleter;
            }
            
            public ICommandResult Handle(DeleteResourcePageCommand cmd)
            {
                var resourcePage = _documentSession.LoadOrThrow<ResourcePage>(cmd.ResourcePageId);

                _documentSession.DeleteWhere<ResourceFile>(resourceFile => resourceFile.ResourcePageId == resourcePage.Id);
                _documentSession.Delete(resourcePage);

                _documentSession.DocumentStore.Advanced.Clean.DeleteSingleEventStream(cmd.ResourcePageId);

                _documentSession.SaveChanges();

                _resourceBlobDeleter.DeleteBlobContainerOfResourcePage(resourcePage)
                    .GetAwaiter().GetResult();

                return CommandResult.Success;
            }
        }
    }
}