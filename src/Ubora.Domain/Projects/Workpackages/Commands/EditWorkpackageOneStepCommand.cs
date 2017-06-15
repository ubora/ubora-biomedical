using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class EditWorkpackageOneStepCommand : UserProjectCommand
    {
        public Guid StepId { get; set; }
        public string NewValue { get; set; }

        internal class Handler : CommandHandler<EditWorkpackageOneStepCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditWorkpackageOneStepCommand cmd)
            {
                var workpackageOne = DocumentSession.Load<WorkpackageOne>(cmd.ProjectId);
                if (workpackageOne == null)
                {
                    throw new InvalidOperationException($"{nameof(WorkpackageOne)} not found with id [{cmd.ProjectId}]");
                }

                var step = workpackageOne.Steps.SingleOrDefault(x => x.Id == cmd.StepId);
                if (step == null)
                {
                    throw new InvalidOperationException($"{nameof(WorkpackageStep)} not found with id [{cmd.StepId}]");
                }

                var canHandle = workpackageOne.DoesSatisfy(new CanWorkpackageOneStepBeEdited(step.Id));
                if (!canHandle)
                {
                    return new CommandResult("Work package step can not be edited.");
                }

                var @event = new WorkpackageStepEditedEvent(cmd.Actor)
                {
                    StepId = cmd.StepId,
                    NewValue = cmd.NewValue,
                    Title = step.Title
                };

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}