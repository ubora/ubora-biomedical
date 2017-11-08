using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Questionnaires.DeviceClassifications.Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Commands
{
    public class StartClassifyingDeviceCommand : UserProjectCommand
    {
        public Guid Id { get; set; }

        internal class Handler : ICommandHandler<StartClassifyingDeviceCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(StartClassifyingDeviceCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var isQuestionnaireAlreadyRunning = _documentSession.Query<DeviceClassificationAggregate>()
                    .Any(x => x.ProjectId == project.Id && x.FinishedAt == null);
                if (isQuestionnaireAlreadyRunning)
                {
                    return CommandResult.Failed("Already questionnaire running...");
                }

                var @event = new DeviceClassificationStartedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    id: cmd.Id,
                    startedAt: DateTime.UtcNow,
                    questionnaireTree: DeviceClassificationQuestionnaireTreeFactory.CreateDeviceClassification());

                _documentSession.Events.StartStream<DeviceClassificationAggregate>(cmd.Id, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
