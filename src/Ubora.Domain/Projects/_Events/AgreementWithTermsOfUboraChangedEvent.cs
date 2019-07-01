using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class AgreementWithTermsOfUboraChangedEvent : ProjectEvent
    {
        public AgreementWithTermsOfUboraChangedEvent(UserInfo initiatedBy, Guid projectId, bool isAgreed) 
            : base(initiatedBy, projectId)
        {
            IsAgreed = isAgreed;
        }

        public bool IsAgreed { get; }

        public override string GetDescription()
        {
            return IsAgreed
                ? "agreed to terms of UBORA."
                : "removed agreement to terms of UBORA.";
        }
    }
}
