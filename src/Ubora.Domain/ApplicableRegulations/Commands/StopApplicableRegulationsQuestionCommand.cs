using System;
using Marten;
using Ubora.Domain.ApplicableRegulations.Events;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;


namespace Ubora.Domain.ApplicableRegulations.Commands
{
    public class StopApplicableRegulationsQuestionnaireCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get;  set; }
       

        public class Handler : ICommandHandler<StopApplicableRegulationsQuestionnaireCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IQueryProcessor _queryProcessor;

            public Handler(IDocumentSession documentSession, IQueryProcessor queryProcessor)
            {
                _documentSession = documentSession;
                _queryProcessor = queryProcessor;
            }

            public ICommandResult Handle(StopApplicableRegulationsQuestionnaireCommand cmd)
            {
               var aggregate = _documentSession.LoadOrThrow<ApplicableRegulationsQuestionnaireAggregate>(cmd.QuestionnaireId);
                
                if (aggregate.IsFinished)
                {
                    return CommandResult.Failed("Questionnaire is already stopped");
                }
                
                var @event =
                    new ApplicableRegulationsQuestionnaireStopedEvent(cmd.Actor);
                
                _documentSession.Events.Append(cmd.QuestionnaireId,@event);
               _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}



