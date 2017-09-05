using System;
using Marten;
using Ubora.Domain.ApplicableRegulations.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.ApplicableRegulations.Commands
{
    public class AnswerApplicableRegulationsQuestionCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }
        public Guid QuestionId { get; set; }
        public bool Answer { get; set; }

        public class Handler : ICommandHandler<AnswerApplicableRegulationsQuestionCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AnswerApplicableRegulationsQuestionCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<ApplicableRegulationsQuestionnaireAggregate>(cmd.QuestionnaireId);

                var question = aggregate.Questionnaire.FindQuestionOrThrow(cmd.QuestionId);
                if (question.Answer.HasValue)
                {
                    return CommandResult.Failed("Question has already been answered. Please reload the questionnaire.");
                }

                var @event = new ApplicableRegulationsQuestionAnsweredEvent(cmd.Actor, cmd.QuestionId, cmd.Answer);
                _documentSession.Events.Append(cmd.QuestionnaireId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}