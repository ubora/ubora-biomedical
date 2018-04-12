using System;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class IsVerifiedUboraMentorQuery : IQuery<bool>
    {
        public IsVerifiedUboraMentorQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}