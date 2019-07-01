using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.IntellectualProperties;
using Ubora.Domain.Projects.IntellectualProperties.Events;

public class EditLicenseCommand : UserProjectCommand
{
    public bool Attribution { get; set; }
    public bool ShareAlike { get; set; }
    public bool NonCommercial { get; set; }
    public bool NoDerivativeWorks { get; set; }   
    public bool UboraLicense { get; set; }

    public class Handler : ICommandHandler<EditLicenseCommand>
    {
        private readonly IDocumentSession _documentSession;

        public Handler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(EditLicenseCommand cmd)
        {
            var intellectualProperty = _documentSession.Load<IntellectualProperty>(cmd.ProjectId);

            object @event = CreateEvent();
            if (@event != null) 
            {
                _documentSession.Events.Append(intellectualProperty.Id, @event);
                _documentSession.SaveChanges();
            }

            return CommandResult.Success;

            LicenseTermsChangedEvent CreateEvent() 
            {
                if (cmd.UboraLicense) 
                {
                    return LicenseTermsChangedEvent.ForUbora(cmd.Actor, cmd.ProjectId, intellectualProperty.Id);
                }

                if (cmd.Attribution
                    || cmd.NoDerivativeWorks
                    || cmd.NonCommercial
                    || cmd.ShareAlike)
                {
                    var creativeCommons = new CreativeCommonsLicense(attribution: cmd.Attribution, noDerivativeWorks: cmd.NoDerivativeWorks, nonCommercial: cmd.NonCommercial, shareAlike: cmd.ShareAlike);
                    if (intellectualProperty.License is CreativeCommonsLicense existingCreativeCommonsLicense) 
                    {
                        if (existingCreativeCommonsLicense == creativeCommons) 
                        {
                            return null;
                        }
                    }
                    return LicenseTermsChangedEvent.ForCreativeCommons(cmd.Actor, cmd.ProjectId, intellectualProperty.Id, creativeCommons);
                }

                if (intellectualProperty.License != null) 
                {
                    return LicenseTermsChangedEvent.ForRemoval(cmd.Actor, cmd.ProjectId, intellectualProperty.Id);
                }

                return null;
            }
        }
    }
}