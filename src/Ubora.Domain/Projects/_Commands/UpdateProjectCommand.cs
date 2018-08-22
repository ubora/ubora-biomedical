using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class UpdateProjectCommand : UserProjectCommand, ITagsAndKeywords
    {
        public string Title { get; set; }
        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }

        internal class Handler : CommandHandler<UpdateProjectCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(UpdateProjectCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new ProjectUpdatedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    title: cmd.Title,
                    clinicalNeedTags: cmd.ClinicalNeedTag,
                    areaOfUsageTags: cmd.AreaOfUsageTag,
                    potentialTechnologyTags: cmd.PotentialTechnologyTag,
                    gmdn: cmd.Keywords);

                DocumentSession.Events.Append(project.Id, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
