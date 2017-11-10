using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Commands
{
    public class AnswerApplicableRegulationsQuestionCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }

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
                if (question.IsAnswered)
                {
                    return CommandResult.Failed("Question has already been answered. Please reload the questionnaire.");
                }

                var @event = new ApplicableRegulationsQuestionAnsweredEvent(cmd.Actor, 
                    projectId: cmd.ProjectId, 
                    questionnaireId: cmd.QuestionnaireId,
                    questionId: cmd.QuestionId,
                    answerId: cmd.AnswerId, 
                    answeredAt: DateTime.UtcNow);

                _documentSession.Events.Append(cmd.QuestionnaireId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}