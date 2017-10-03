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
        public IEnumerable<Guid> AssigneeIds { get; set; }

        internal class Handler : CommandHandler<EditTaskCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditTaskCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var task = DocumentSession.LoadOrThrow<ProjectTask>(cmd.Id);
                var previousAssignees = task.Assignees?.Select(x => x.UserId).ToList();

                var @event = new TaskEditedEvent(cmd.Actor)
                {
                    Description = cmd.Description,
                    ProjectId = project.Id,
                    Id = cmd.Id,
                    Title = cmd.Title,
                    AssigneeIds = cmd.AssigneeIds
                };

                DocumentSession.Events.Append(project.Id, @event);
                DocumentSession.SaveChanges();

                if (cmd.AssigneeIds == null)
                {
                    return CommandResult.Success;
                }

                var addedAssignees = cmd.AssigneeIds?.Except(previousAssignees).ToList();
                foreach (var assigneeId in addedAssignees)
                {
                    var notification = new AssignmentAssignedToNotification(assigneeId, cmd.Actor.UserId, cmd.ProjectId, cmd.Id);
                    DocumentSession.Store(notification);
                }

                var removedAssignees = previousAssignees?.Except(cmd.AssigneeIds).ToList();
                foreach (var assigneeId in removedAssignees)
                {
                    var notification = new AssignmentRemovedFromNotification(assigneeId, cmd.Actor.UserId, cmd.ProjectId, cmd.Id);
                    DocumentSession.Store(notification);
                }

                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
