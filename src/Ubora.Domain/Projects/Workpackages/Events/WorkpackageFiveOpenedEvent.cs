using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageFiveOpenedEvent : ProjectEvent
    {
        public WorkpackageFiveOpenedEvent(UserInfo initiatedBy, Guid projectId)
            : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription() => "opened work package 5: Operation.";
    }
}
