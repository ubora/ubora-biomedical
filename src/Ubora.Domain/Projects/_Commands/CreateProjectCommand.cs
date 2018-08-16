using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class CreateProjectCommand : UserCommand, ITagsAndKeywords
    {
        public Guid NewProjectId { get; set; }
        public string Title { get; set; }
        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }

        internal class Handler : CommandHandler<CreateProjectCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(CreateProjectCommand cmd)
            {
                var @event = new ProjectCreatedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.NewProjectId,
                    title: cmd.Title,
                    clinicalNeed: cmd.ClinicalNeedTag,
                    areaOfUsage: cmd.AreaOfUsageTag,
                    potentialTechnology: cmd.PotentialTechnologyTag,
                    gmdn: cmd.Keywords);

                DocumentSession.Events.Append(cmd.NewProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}