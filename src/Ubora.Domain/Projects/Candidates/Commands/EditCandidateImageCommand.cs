﻿using Marten;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class EditCandidateImageCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public BlobLocation ImageLocation { get; set; }

        internal class Handler : CommandHandler<EditCandidateImageCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditCandidateImageCommand cmd)
            {
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var @event = new CandidateImageEditedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    id: cmd.Id,
                    imageLocation: cmd.ImageLocation
                );

                DocumentSession.Events.Append(cmd.Id, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
