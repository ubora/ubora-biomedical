using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class EditWorkpackageThreeStepCommand : UserProjectCommand
    {
        public string StepId { get; set; }
        public QuillDelta NewValue { get; set; }

        internal class Handler : CommandHandler<EditWorkpackageThreeStepCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditWorkpackageThreeStepCommand cmd)
            {
                var workpackage = DocumentSession.LoadOrThrow<WorkpackageThree>(cmd.ProjectId);

                var step = workpackage.Steps.SingleOrDefault(x => x.Id == cmd.StepId);
                if (step == null)
                {
                    throw new InvalidOperationException($"{nameof(WorkpackageStep)} not found with id [{cmd.StepId}]");
                }

                var @event = new WorkpackageThreeStepEditedEventV2
                (
                    initiatedBy: cmd.Actor,
                    stepId: cmd.StepId,
                    title: step.Title,
                    newValue: cmd.NewValue,
                    projectId: cmd.ProjectId
                );

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}