using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Tasks.Events;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.Tasks.Notifications;

namespace Ubora.Domain.Projects.Tasks.Commands
{
    public class EditTaskCommand : UserProjectCommand
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

        internal class Handler : CommandHandler<EditTaskCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditTaskCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var task = DocumentSession.LoadOrThrow<ProjectTask>(cmd.Id);

                var previousAssigneeIds = task.Assignees.Select(x => x.UserId).ToList();

                var @event = new TaskEditedEvent(
                    initiatedBy: cmd.Actor,
                    description: cmd.Description,
                    projectId: cmd.ProjectId,
                    id: cmd.Id,
                    title: cmd.Title,
                    assigneeIds: cmd.AssigneeIds);

                DocumentSession.Events.Append(project.Id, @event);

                var addedAssigneeIds = cmd.AssigneeIds.Except(previousAssigneeIds);
                foreach (var assigneeId in addedAssigneeIds)
                {
                    var notification = new AssignmentAssignedToNotification(notificationTo: assigneeId, requesterId: cmd.Actor.UserId, projectId: cmd.ProjectId, taskId: cmd.Id);
                    DocumentSession.Store(notification);
                }

                var removedAssigneeIds = previousAssigneeIds.Except(cmd.AssigneeIds);
                foreach (var assigneeId in removedAssigneeIds)
                {
                    var notification = new AssignmentRemovedFromNotification(notificationTo: assigneeId, requesterId: cmd.Actor.UserId, projectId: cmd.ProjectId, taskId: cmd.Id);
                    DocumentSession.Store(notification);
                }

                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
