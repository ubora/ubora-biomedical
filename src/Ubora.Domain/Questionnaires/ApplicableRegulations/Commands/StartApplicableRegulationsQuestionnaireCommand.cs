using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Events;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Queries;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Commands
{
    public class StartApplicableRegulationsQuestionnaireCommand : UserProjectCommand
    {
        public Guid NewQuestionnaireId { get; set; }

        public class Handler : ICommandHandler<StartApplicableRegulationsQuestionnaireCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IQueryProcessor _queryProcessor;

            public Handler(IDocumentSession documentSession, IQueryProcessor queryProcessor)
            {
                _documentSession = documentSession;
                _queryProcessor = queryProcessor;
            }

            public ICommandResult Handle(StartApplicableRegulationsQuestionnaireCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var existingActiveQuestionnaire = 
                    _queryProcessor.ExecuteQuery(new ActiveApplicableRegulationsQuestionnaireQuery { ProjectId = project.Id });

                if (existingActiveQuestionnaire != null)
                {
                    return CommandResult.Failed("Already questionnaire running...");
                }

                var @event = new ApplicableRegulationsQuestionnaireStartedEvent(cmd.Actor, cmd.NewQuestionnaireId, project.Id, QuestionnaireTreeFactory.Create(), DateTime.UtcNow);

                _documentSession.Events.StartStream<ApplicableRegulationsQuestionnaireAggregate>(cmd.NewQuestionnaireId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}