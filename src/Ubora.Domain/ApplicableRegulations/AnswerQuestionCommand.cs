using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.ApplicableRegulations
{
    public class AnswerQuestionCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }
        public Guid QuestionId { get; set; }
        public bool Answer { get; set; }

        public class Handler : ICommandHandler<AnswerQuestionCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AnswerQuestionCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<ProjectQuestionnaireAggregate>(cmd.QuestionnaireId);

                var question = aggregate.Questionnaire.FindQuestionOrThrow(cmd.QuestionId);
                if (question.Answer.HasValue)
                {
                    return CommandResult.Failed("Already answered. Refresh...");
                }

                var @event = new QuestionAnsweredEvent(cmd.Actor, cmd.QuestionId, cmd.Answer);
                _documentSession.Events.Append(cmd.QuestionnaireId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}