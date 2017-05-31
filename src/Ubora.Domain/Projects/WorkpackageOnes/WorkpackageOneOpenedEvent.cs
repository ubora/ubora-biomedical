using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class WorkpackageOneOpenedEvent : UboraEvent
    {
        public WorkpackageOneOpenedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; set; }

        public override string GetDescription()
        {
            return "Opened first work package.";
        }
    }
}