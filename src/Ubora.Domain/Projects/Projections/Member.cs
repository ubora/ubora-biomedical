using System;

namespace Ubora.Domain.Projects.Projections
{
    public abstract class Member
    {
        protected Member(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }

    public class Leader : Member
    {
        public Leader(Guid userId) : base(userId)
        {
        }
    }
}
