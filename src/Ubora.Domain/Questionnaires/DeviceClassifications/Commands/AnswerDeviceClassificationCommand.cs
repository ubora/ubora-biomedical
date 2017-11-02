using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Questionnaires.DeviceClassifications.Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Commands
{
    public class AnswerDeviceClassificationCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }

        internal class Handler : ICommandHandler<AnswerDeviceClassificationCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AnswerDeviceClassificationCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<DeviceClassificationAggregate>(cmd.QuestionnaireId);

                var @event = new DeviceClassificationAnsweredEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    questionnaireId: cmd.QuestionnaireId, 
                    questionId: cmd.QuestionId, 
                    answerId: cmd.AnswerId,
                    answeredAt: DateTime.UtcNow);

                _documentSession.Events.Append(aggregate.Id, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
