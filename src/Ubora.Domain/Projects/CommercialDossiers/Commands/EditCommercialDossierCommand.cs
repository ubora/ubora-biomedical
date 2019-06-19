using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.CommercialDossiers.Events;

namespace Ubora.Domain.Projects.CommercialDossiers.Commands
{
    public class EditCommercialDossierCommand : UserProjectCommand
    {
        public string ProductName { get; set; }
        public string CommercialName { get; set; }
        public QuillDelta Description { get; set; }
        public BlobLocation LogoLocation { get; set; }
        public (BlobLocation Location, string FileName, long FileSize) UserManualInfo { get; set; }

        public class Handler : ICommandHandler<EditCommercialDossierCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(EditCommercialDossierCommand cmd)
            {
                var commercialDossier = _documentSession.LoadOrThrow<CommercialDossier>(cmd.ProjectId);

                var events = new List<object>();

                if (commercialDossier.ProductName != cmd.ProductName) 
                    events.Add(new ProductNameChangedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.ProductName));

                if (commercialDossier.CommercialName != cmd.CommercialName) 
                    events.Add(new CommercialNameChangedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.CommercialName));

                if (commercialDossier.Description != cmd.Description)
                    events.Add(new DescriptionEditedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.Description));
                    
                if (commercialDossier.Logo != cmd.LogoLocation)
                    events.Add(new LogoChangedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.LogoLocation));

                if (commercialDossier.UserManual?.Location != cmd.UserManualInfo.Location)
                    events.Add(new UserManualChangedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.UserManualInfo.Location, cmd.UserManualInfo.FileName, cmd.UserManualInfo.FileSize));

                if (events.Any()) 
                {
                    _documentSession.Events.Append(cmd.ProjectId, events.ToArray());
                    _documentSession.SaveChanges();
                }

                return CommandResult.Success;
            }
        }
    }
} 