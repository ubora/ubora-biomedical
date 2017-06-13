using System;

namespace Ubora.Domain.Projects.Members
{
    public class ProjectMentor : ProjectMember
    {
        public ProjectMentor(Guid userId) : base(userId)
        {
        }
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
