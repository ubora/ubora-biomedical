using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Events
{
    public class ApplicableRegulationsQuestionnaireStoppedEvent : ProjectEvent
    {
        public ApplicableRegulationsQuestionnaireStoppedEvent(UserInfo initiatedBy, Guid projectId, DateTime stoppedAt) : base(initiatedBy, projectId)
        {
            StoppedAt = stoppedAt;
        }

        public DateTime StoppedAt { get; private set; }

        public override string GetDescription() => "stopped the questionnaire.";
    }
}
