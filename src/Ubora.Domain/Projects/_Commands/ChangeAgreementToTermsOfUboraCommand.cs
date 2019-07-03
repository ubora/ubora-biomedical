using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects._Commands
{
    public class ChangeAgreementToTermsOfUboraCommand : UserProjectCommand
    {
        public bool IsAgreed { get; set; }

        internal class Handler : ICommandHandler<ChangeAgreementToTermsOfUboraCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(ChangeAgreementToTermsOfUboraCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var wp5 = _documentSession.Load<WorkpackageFive>(cmd.ProjectId);
                if (wp5 == null)
                {
                    return CommandResult.Failed("WP5 must be open to change the agreement.");
                }

                // No need to apply event again when already of correct agreement boolean.
                if (project.IsAgreedToTermsOfUbora != cmd.IsAgreed)
                {
                    _documentSession.Events.Append(project.Id, new AgreementWithTermsOfUboraChangedEvent(cmd.Actor, cmd.ProjectId, cmd.IsAgreed));
                    _documentSession.SaveChanges();
                }

                return CommandResult.Success;
            }
        }
    }
}
