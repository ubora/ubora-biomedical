﻿using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.WorkpackageOnes;
using Ubora.Domain.Projects.WorkpackageSpecifications;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    /// <summary>
    /// Submit workpackage for formal review. Workpackage will be locked for new edits while in review.
    /// </summary>
    public class SubmitWorkpackageOneForReviewCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<SubmitWorkpackageOneForReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(SubmitWorkpackageOneForReviewCommand cmd)
            {
                // lock if workpackage one

                var workpackageOne = DocumentSession.Load<WorkpackageOne>(cmd.ProjectId);
                if (workpackageOne == null)
                {
                    throw new InvalidOperationException($"{nameof(WorkpackageOne)} not found with id [{cmd.ProjectId}]");
                }

                var canHandle = workpackageOne.DoesSatisfy(new CanBeSubmittedForReview<WorkpackageOne>());
                if (!canHandle)
                {
                    return new CommandResult("Work package can not be submitted for review.");
                }

                var @event = new WorkpackageOneSubmittedForReviewEvent(cmd.Actor, cmd.ProjectId);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}