using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members
{
    public class AssignProjectMentorCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }

        internal class Handler : CommandHandler<AssignProjectMentorCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AssignProjectMentorCommand cmd)
            {
                var project = DocumentSession.Load<Project>(cmd.ProjectId);
                if (project == null)
                {
                    throw new InvalidOperationException($"{nameof(Project)} not found with id [{cmd.ProjectId}]");
                }

                var user = DocumentSession.Load<UserProfile>(cmd.UserId);
                if (user == null)
                {
                    throw new InvalidOperationException($"{nameof(UserProfile)} not found with id [{cmd.UserId}]");
                }

                var @event = new ProjectMentorAssignedEvent(
                    initiatedBy: cmd.Actor, 
                    userId: cmd.UserId,
                    projectId: cmd.ProjectId);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }

    //public class WorkpackageTwoStep
    //{
    //    public Guid Id { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public IEnumerable<WorkpackageTwoStepPage> Pages { get; set; }
    //}

    //public class WorkpackageTwoStepPage
    //{
    //    public Guid Id { get; set; }
    //    public Guid StepId { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //}

    //public class WorkpackageTwo : Entity<WorkpackageTwo>
    //{
    //    [Identity]
    //    public Guid ProjectId { get; private set; }
    //    public IEnumerable<WorkpackageTwoStep> Steps { get; set; }
    //}
}
