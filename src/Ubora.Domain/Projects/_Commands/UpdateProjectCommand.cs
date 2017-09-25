﻿using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class UpdateProjectCommand : UserProjectCommand
    {
        public string Title { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string Gmdn { get; set; }

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
                    id: cmd.ProjectId,
                    title: cmd.Title,
                    clinicalNeedTags: cmd.ClinicalNeedTags,
                    areaOfUsageTags: cmd.AreaOfUsageTags,
                    potentialTechnologyTags: cmd.PotentialTechnologyTags,
                    gmdn: cmd.Gmdn);

                DocumentSession.Events.Append(project.Id, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
