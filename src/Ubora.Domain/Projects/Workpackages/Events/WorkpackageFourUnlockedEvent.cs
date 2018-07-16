using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageFourUnlockedEvent : ProjectEvent
    {
        public WorkpackageFourUnlockedEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
        {
            
        }

        public override string GetDescription() => $"unlocked WP4.";
    }
}