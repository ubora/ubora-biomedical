using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    public class ProjectMentorAssignedEvent : UboraEvent
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }

        public ProjectMentorAssignedEvent(UserInfo initiatedBy, Guid userId, Guid projectId) : base(initiatedBy)
        {
            UserId = userId;
            ProjectId = projectId;
        }

        public override string GetDescription() => "Assigned project mentor.";
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

    //public class WorkpackageTwo : Entity<WorkpackageTwo>
    //{
    //    [Identity]
    //    public Guid ProjectId { get; private set; }
    //    public IEnumerable<WorkpackageTwoStep> Steps { get; set; }
    //}
}
