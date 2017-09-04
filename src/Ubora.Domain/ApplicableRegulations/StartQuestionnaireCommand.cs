using System;
using System.Linq;
using Marten;
using Ubora.Domain.ApplicableRegulations.Queries;
using Ubora.Domain.ApplicableRegulations.Specifications;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Domain.ApplicableRegulations
{
    public class StartQuestionnaireCommand : UserProjectCommand
    {
        public Guid NewQuestionnaireId { get; set; }

        public class Handler : ICommandHandler<StartQuestionnaireCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IQueryProcessor _queryProcessor;

            public Handler(IDocumentSession documentSession, IQueryProcessor queryProcessor)
            {
                _documentSession = documentSession;
                _queryProcessor = queryProcessor;
            }

            public ICommandResult Handle(StartQuestionnaireCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var existingActiveQuestionnaire = 
                    _queryProcessor.ExecuteQuery(new ActiveApplicableRegulationsQuestionnaireQuery { ProjectId = project.Id });

                if (existingActiveQuestionnaire != null)
                {
                    return CommandResult.Failed("Already questionnaire running...");
                }

                var @event = new QuestionnaireStartedEvent(cmd.Actor, cmd.NewQuestionnaireId, project.Id);

                _documentSession.Events.StartStream<ProjectQuestionnaireAggregate>(cmd.NewQuestionnaireId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}