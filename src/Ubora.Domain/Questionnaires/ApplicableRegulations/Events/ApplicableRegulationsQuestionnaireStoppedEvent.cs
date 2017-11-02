using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Events
{
    public class ApplicableRegulationsQuestionnaireStoppedEvent : ProjectEvent
    {
        public ApplicableRegulationsQuestionnaireStoppedEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription() => "stopped the questionnaire.";
    }
}
