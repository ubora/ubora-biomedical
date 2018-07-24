using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Queries;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class OpenWorkpackageFourCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<OpenWorkpackageFourCommand>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Handler(IDocumentSession documentSession, IQueryProcessor queryProcessor) : base(documentSession)
            {
                _queryProcessor = queryProcessor;
            }

            public override ICommandResult Handle(OpenWorkpackageFourCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);
                if (!workpackageOne.HasBeenAccepted)
                {
                    return CommandResult.Failed("Work package one hasn't been accepted.");
                }
                    
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var workPackageThree = DocumentSession.Load<WorkpackageThree>(cmd.ProjectId);
                if (!workPackageThree.HasBeenOpened)
                {
                    return CommandResult.Failed("Work package three hasn't been opened.");
                }
                
                var workPackageFour = DocumentSession.Load<WorkpackageFour>(cmd.ProjectId);
                if (workPackageFour != null)
                {
                    return CommandResult.Failed("Work package is already opened.");
                }

                var latestFinishedApplicableRegulationsQuestionnaire =
                    _queryProcessor
                        .ExecuteQuery(new FindLatestFinishedApplicableRegulationsQuestionnaireAggregateQuery(cmd.ProjectId))
                        ?.Questionnaire;

                var @event = new WorkpackageFourOpenedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    latestFinishedApplicableRegulationsQuestionnaire: latestFinishedApplicableRegulationsQuestionnaire);
                
                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}