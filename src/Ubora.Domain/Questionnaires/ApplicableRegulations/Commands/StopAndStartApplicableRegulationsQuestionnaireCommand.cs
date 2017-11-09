using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Commands
{
    public class StopAndStartApplicableRegulationsQuestionnaireCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }
        public Guid NewQuestionnaireId { get; set; }

        internal class Handler : ICommandHandler<StopAndStartApplicableRegulationsQuestionnaireCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(StopAndStartApplicableRegulationsQuestionnaireCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<ApplicableRegulationsQuestionnaireAggregate>(cmd.QuestionnaireId);
                if (aggregate.IsFinished)
                {
                    return CommandResult.Failed("Questionnaire is already stopped.");
                }

                var now = DateTime.UtcNow;

                var eventStop = new ApplicableRegulationsQuestionnaireStoppedEvent(cmd.Actor, 
                    projectId: aggregate.ProjectId,
                    stoppedAt: now);

                var eventStart = new ApplicableRegulationsQuestionnaireStartedEvent(cmd.Actor,
                    projectId: aggregate.ProjectId,
                    newQuestionnaireId: cmd.NewQuestionnaireId, 
                    questionnaireTree: ApplicableRegulationsQuestionnaireTreeFactory.Create(), 
                    startedAt: now);

                _documentSession.Events.Append(cmd.QuestionnaireId, eventStop);
                _documentSession.Events.StartStream<ApplicableRegulationsQuestionnaireAggregate>(cmd.NewQuestionnaireId, eventStart);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
