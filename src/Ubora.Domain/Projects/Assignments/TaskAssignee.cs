using System;

namespace Ubora.Domain.Projects.Assignments
{
    public class TaskAssignee
    {
        public TaskAssignee(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
