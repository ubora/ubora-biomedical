using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Questionnaires.DeviceClassifications.Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Commands
{
    public class StopAndStartDeviceClassificationCommand : UserProjectCommand
    {
        public Guid StopQuestionnaireId { get; set; }
        public Guid StartQuestionnaireId { get; set; }

        internal class Handler : ICommandHandler<StopAndStartDeviceClassificationCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(StopAndStartDeviceClassificationCommand cmd)
            {
                var deviceClassificationAggregate = _documentSession.LoadOrThrow<DeviceClassificationAggregate>(cmd.StopQuestionnaireId);
                if (deviceClassificationAggregate.IsFinished)
                {
                    return CommandResult.Failed("Previous device classification has already been stopped.");
                }

                var now = DateTime.UtcNow;

                var stopEvent = new DeviceClassificationStoppedEvent(cmd.Actor,
                    projectId: cmd.ProjectId,
                    stoppedAt: now);

                var startEvent = new DeviceClassificationStartedEvent(cmd.Actor,
                    projectId: cmd.ProjectId,
                    id: cmd.StartQuestionnaireId,
                    questionnaireTree: DeviceClassificationQuestionnaireTreeFactory.CreateDeviceClassification(),
                    startedAt: now);

                _documentSession.Events.Append(cmd.StopQuestionnaireId, stopEvent);
                _documentSession.Events.StartStream<DeviceClassificationAggregate>(cmd.StartQuestionnaireId, startEvent);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
