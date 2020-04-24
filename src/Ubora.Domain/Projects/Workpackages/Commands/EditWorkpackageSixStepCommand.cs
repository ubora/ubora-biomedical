using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class EditWorkpackageSixStepCommand : UserProjectCommand
    {
        public string StepId { get; set; }
        public QuillDelta NewValue { get; set; }

        internal class Handler : CommandHandler<EditWorkpackageSixStepCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditWorkpackageSixStepCommand cmd)
            {
                var workpackage = DocumentSession.LoadOrThrow<WorkpackageSix>(cmd.ProjectId);

                var step = workpackage.Steps.SingleOrDefault(x => x.Id == cmd.StepId);

                var @event = new WorkpackageSixStepEditedEvent
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