using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Events
{
    public class ApplicableRegulationsQuestionnaireStartedEvent : ProjectEvent
    {
        public ApplicableRegulationsQuestionnaireStartedEvent(UserInfo initiatedBy, Guid projectId, Guid newQuestionnaireId, ApplicableRegulationsQuestionnaireTree questionnaireTree, DateTime startedAt) : base(initiatedBy, projectId)
        {
            NewQuestionnaireId = newQuestionnaireId;
            QuestionnaireTree = questionnaireTree;
            StartedAt = startedAt;
        }

        public Guid NewQuestionnaireId { get; private set; }
        public ApplicableRegulationsQuestionnaireTree QuestionnaireTree { get; private set; }
        public DateTime StartedAt { get; private set; }

        public override string GetDescription() => "started questionnaire to find out project's applicable regulations (ISO standard)";
    }
}