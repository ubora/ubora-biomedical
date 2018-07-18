using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class EditWorkpackageFourStepCommand : UserProjectCommand
    {
        public string StepId { get; set; }
        public string NewValue { get; set; }

        internal class Handler : CommandHandler<EditWorkpackageFourStepCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditWorkpackageFourStepCommand cmd)
            {
                var workpackage = DocumentSession.LoadOrThrow<WorkpackageFour>(cmd.ProjectId);

                var step = workpackage.Steps.SingleOrDefault(x => x.Id == cmd.StepId);

                var @event = new WorkpackageFourStepEdited
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