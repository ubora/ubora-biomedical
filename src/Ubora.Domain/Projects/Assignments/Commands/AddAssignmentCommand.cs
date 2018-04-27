using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Assignments.Events;
using Ubora.Domain.Projects.Assignments.Notifications;

namespace Ubora.Domain.Projects.Assignments.Commands
{
    public class AddAssignmentCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<Guid> _assigneeIds;
        public IEnumerable<Guid> AssigneeIds
        {
            get { return _assigneeIds ?? Enumerable.Empty<Guid>(); }
            set { _assigneeIds = value; }
        }

        internal class Handler : CommandHandler<AddAssignmentCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddAssignmentCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new AssignmentAddedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    createdByUserId: cmd.CreatedByUserId,
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
