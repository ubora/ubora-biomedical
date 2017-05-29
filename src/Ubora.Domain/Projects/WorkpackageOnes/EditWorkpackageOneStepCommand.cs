using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class EditWorkpackageOneStepCommand : UserProjectCommand
    {
        public Guid StepId { get; set; }
        public string Title { get; set; }
        public string NewValue { get; set; }

        internal class Handler : CommandHandler<EditWorkpackageOneStepCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditWorkpackageOneStepCommand cmd)
            {
                var @event = new WorkpackageOneStepEditedEvent(cmd.Actor)
                {
                    StepId = cmd.StepId,
                    Title = cmd.Title,
                    NewValue = cmd.NewValue
                };

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}