using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.WorkpackageOnes
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
                    throw new InvalidOperationException($"{nameof(WorkpackageOneStep)} not found with id [{cmd.StepId}]");
                }

                var @event = new WorkpackageOneStepEditedEvent(cmd.Actor)
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