﻿using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources.Commands
{
    public class DeleteResourcePageCommand : UserCommand
    {
        public Guid ResourceId { get; set; }

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
                var resourcePage = _documentSession.LoadOrThrow<ResourcePage>(cmd.ResourceId);

                _documentSession.Events.Append(cmd.ResourceId, new ResourcePageDeletedEvent(
                    initiatedBy: cmd.Actor,
                    resourceId: cmd.ResourceId));
                _documentSession.Delete(resourcePage);
                _documentSession.SaveChanges();

                _resourceBlobDeleter.DeleteBlobContainerOfResourcePage(resourcePage)
                    .GetAwaiter().GetResult();

                return CommandResult.Success;
            }
        }
    }
}