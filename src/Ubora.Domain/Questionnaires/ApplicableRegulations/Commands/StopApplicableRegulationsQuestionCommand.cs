using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Commands
{
    public class StopApplicableRegulationsQuestionnaireCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }

        public class Handler : ICommandHandler<StopApplicableRegulationsQuestionnaireCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(StopApplicableRegulationsQuestionnaireCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<ApplicableRegulationsQuestionnaireAggregate>(cmd.QuestionnaireId);

                if (aggregate.IsFinished)
                {
                    return CommandResult.Failed("Questionnaire is already stopped");
                }

                var @event = new ApplicableRegulationsQuestionnaireStoppedEvent(cmd.Actor, aggregate.ProjectId);

                _documentSession.Events.Append(cmd.QuestionnaireId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}