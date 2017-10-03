using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Tasks.Events;
using System.Collections.Generic;
using Ubora.Domain.Projects.Tasks.Notifications;

namespace Ubora.Domain.Projects.Tasks.Commands
{
    public class AddTaskCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> AssigneeIds { get; set; }

        internal class Handler : CommandHandler<AddTaskCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddTaskCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new TaskAddedEvent(cmd.Actor)
                {
                    ProjectId = cmd.ProjectId,
                    Id = cmd.Id,
                    Title = cmd.Title,
                    Description = cmd.Description,
                    AssigneeIds = cmd.AssigneeIds
                };

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                if(cmd.AssigneeIds == null)
                {
                    return CommandResult.Success;
                }

                foreach (var assigneeId in cmd.AssigneeIds)
                {
                    var notification = new AssignmentAssignedToNotification(assigneeId, cmd.Actor.UserId, cmd.ProjectId, cmd.Id);
                    DocumentSession.Store(notification);
                    DocumentSession.SaveChanges();
                }

                return CommandResult.Success;
            }
        }
    }
}
