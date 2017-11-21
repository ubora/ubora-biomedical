using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Assignments.Events;
using Ubora.Domain.Projects.Assignments.Notifications;

namespace Ubora.Domain.Projects.Assignments.Commands
{
    public class EditAssignmentCommand : UserProjectCommand
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

        internal class Handler : CommandHandler<EditAssignmentCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditAssignmentCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var task = DocumentSession.LoadOrThrow<Assignment>(cmd.Id);

                var previousAssigneeIds = task.Assignees.Select(x => x.UserId).ToList();

                var @event = new AssignmentEditedEvent(
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
