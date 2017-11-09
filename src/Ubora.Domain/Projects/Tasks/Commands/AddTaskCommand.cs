using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Tasks.Events;
using System.Collections.Generic;
using Ubora.Domain.Projects.Tasks.Notifications;
using System.Linq;

namespace Ubora.Domain.Projects.Tasks.Commands
{
    public class AddTaskCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<Guid> _assigneeIds;
        public IEnumerable<Guid> AssigneeIds
        {
            get { return _assigneeIds ?? Enumerable.Empty<Guid>(); }
            set { _assigneeIds = value; }
        }

        internal class Handler : CommandHandler<AddTaskCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddTaskCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new TaskAddedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    id: cmd.Id,
                    title: cmd.Title,
                    description: cmd.Description,
                    assigneeIds: cmd.AssigneeIds
                );

                DocumentSession.Events.Append(cmd.ProjectId, @event);

                foreach (var assigneeId in cmd.AssigneeIds)
                {
                    var notification = new AssignmentAssignedToNotification(notificationTo: assigneeId, requesterId: cmd.Actor.UserId, projectId: cmd.ProjectId, taskId: cmd.Id);
                    DocumentSession.Store(notification);
                }

                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
