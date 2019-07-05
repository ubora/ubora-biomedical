using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Assignments.Events;

namespace Ubora.Domain.Projects.Assignments.Commands
{
    public class ToggleAssignmentDoneStatusCommand : UserProjectCommand
    {
        public Guid Id { get; set; }

        internal class Handler : CommandHandler<ToggleAssignmentDoneStatusCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(ToggleAssignmentDoneStatusCommand cmd)
            {
                var task = DocumentSession.LoadOrThrow<Assignment>(cmd.Id);

                if(task.IsDone)
                {
                    var @event = new AssignmentMarkedAsNotDoneEvent(cmd.Actor, cmd.ProjectId, cmd.Id);
                    DocumentSession.Events.Append(cmd.ProjectId, @event);
                }
                else
                {
                    var @event = new AssignmentMarkedAsDoneEvent(cmd.Actor, cmd.ProjectId, cmd.Id);
                    DocumentSession.Events.Append(cmd.ProjectId, @event);
                }

                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
