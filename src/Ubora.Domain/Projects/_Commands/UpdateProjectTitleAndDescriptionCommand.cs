using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class UpdateProjectTitleAndDescriptionCommand : UserProjectCommand
    {
        public QuillDelta Description { get; set; }
        public string Title { get; set; }

        internal class Handler : CommandHandler<UpdateProjectTitleAndDescriptionCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(UpdateProjectTitleAndDescriptionCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                if (project.DescriptionV2 != cmd.Description)
                {
                    var editProjectDescriptionEvent = new ProjectDescriptionEditedEventV2(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    description: cmd.Description);

                    DocumentSession.Events.Append(project.Id, editProjectDescriptionEvent);
                }

                if (project.Title != cmd.Title)
                {
                    var projectTitleEditedEvent = new ProjectTitleEditedEvent(
                        initiatedBy: cmd.Actor,
                        projectId: cmd.ProjectId,
                        title: cmd.Title);

                    DocumentSession.Events.Append(project.Id, projectTitleEditedEvent);
                }

                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
