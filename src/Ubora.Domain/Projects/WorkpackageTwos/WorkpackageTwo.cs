using System;
using System.Collections.Generic;
using Marten.Schema;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    public class WorkpackageTwoOpenedEvent : UboraEvent
    {
        public WorkpackageTwoOpenedEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy)
        {
            ProjectId = projectId;
        }

        public Guid ProjectId { get; private set; }

        public override string GetDescription() => "Opened work package 2";
    }

    //public class WorkpackageTwoStep
    //{
    //    public Guid Id { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public IEnumerable<WorkpackageTwoStepPage> Pages { get; set; }
    //}

    //public class WorkpackageTwoStepPage
    //{
    //    public Guid Id { get; set; }
    //    public Guid StepId { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //}

    public class WorkpackageTwo : Workpackage<WorkpackageTwo>
    {
        private void Apply(WorkpackageTwoOpenedEvent e)
        {
            // validate

            ProjectId = e.ProjectId;
        }
    }
}
