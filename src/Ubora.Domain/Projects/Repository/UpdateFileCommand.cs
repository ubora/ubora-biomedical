﻿using Marten;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Repository
{
    public class UpdateFileCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public BlobLocation BlobLocation { get; set; }

        internal class Handler : ICommandHandler<UpdateFileCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(UpdateFileCommand cmd)
            {
                var projectFile = _documentSession.Load<ProjectFile>(cmd.Id);
                if (projectFile == null)
                {
                    throw new InvalidOperationException();
                }

                var @event = new FileUpdatedEvent(
                    projectFile.Id,
                    projectFile.ProjectId,
                    cmd.BlobLocation,
                    cmd.Actor
                );

                _documentSession.Events.Append(projectFile.ProjectId, @event);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}