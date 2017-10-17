using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.ApplicableRegulations.Events
{
    public class ApplicableRegulationsQuestionnaireStopedEvent : UboraEvent
    {
        
        public ApplicableRegulationsQuestionnaireStopedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
          
        }

        public override string GetDescription() => "stopped the questionnaire.";
    }
}
