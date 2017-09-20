using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Tasks.Events;

namespace Ubora.Domain.Projects.Tasks.Commands
{
    public class AddTaskCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        internal class Handler : CommandHandler<AddTaskCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddTaskCommand cmd)
            {
                var project = DocumentSession.Load<Project>(cmd.ProjectId);
                if (project == null)
                {
                    throw new InvalidOperationException();
                }

                var @event = new TaskAddedEvent(cmd.Actor)
                {
                    ProjectId = cmd.ProjectId,
                    Id = cmd.Id,
                    Title = cmd.Title,
                    Description = cmd.Description
                };

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
