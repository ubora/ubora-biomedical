using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Events
{
    public class ApplicableRegulationsQuestionnaireStartedEvent : UboraEvent
    {
        public ApplicableRegulationsQuestionnaireStartedEvent(UserInfo initiatedBy, Guid newQuestionnaireId, Guid projectId, QuestionnaireTree questionnaireTree, DateTime startedAt) : base(initiatedBy)
        {
            NewQuestionnaireId = newQuestionnaireId;
            ProjectId = projectId;
            QuestionnaireTree = questionnaireTree;
            StartedAt = startedAt;
        }

        public Guid NewQuestionnaireId { get; private set; }
        public Guid ProjectId { get; private set; }
        public QuestionnaireTree QuestionnaireTree { get; private set; }
        public DateTime StartedAt { get; private set; }

        public override string GetDescription() => "started questionnaire to find out project's applicable regulations (ISO standard)";
    }
}