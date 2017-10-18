using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.ApplicableRegulations.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.ApplicableRegulations.Commands
{
    public class StopAndStartApplicableRegulationsQuestionCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }
        public Guid NewQuestionnaireId { get; set; }

        public class Handler : ICommandHandler<StopAndStartApplicableRegulationsQuestionCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(StopAndStartApplicableRegulationsQuestionCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<ApplicableRegulationsQuestionnaireAggregate>(cmd.QuestionnaireId);

                if (aggregate.IsFinished)
                {
                    return CommandResult.Failed("Questionnaire is already stopped");
                }

                var eventStop = new ApplicableRegulationsQuestionnaireStoppedEvent(cmd.Actor, aggregate.ProjectId);
                var eventStart = new ApplicableRegulationsQuestionnaireStartedEvent(cmd.Actor, cmd.NewQuestionnaireId, aggregate.ProjectId);
                

                _documentSession.Events.Append(cmd.QuestionnaireId, eventStop);
                _documentSession.Events.StartStream(cmd.NewQuestionnaireId, eventStart);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
