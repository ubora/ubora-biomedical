using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages.Events
{
    public class WorkpackageSixOpenedEvent : ProjectEvent
    {
        public WorkpackageSixOpenedEvent(UserInfo initiatedBy, Guid projectId)
            : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription() => "opened work package 6: Operation.";
    }
}