using System;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.UboraMentors.Queries
{
    public class IsVerifiedUboraMentorQuery : IQuery<bool>
    {
        public IsVerifiedUboraMentorQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }

        public class Handler : IQueryHandler<IsVerifiedUboraMentorQuery, bool>
        {
            private readonly ApplicationUserManager _userManager;

            public Handler(ApplicationUserManager userManager)
            {
                _userManager = userManager;
            }

            public bool Handle(IsVerifiedUboraMentorQuery query)
            {
                return HandleAsync(query)
                    .GetAwaiter().GetResult();
            }

            private async Task<bool> HandleAsync(IsVerifiedUboraMentorQuery query)
            {
                var identityUser = await _userManager.FindByIdAsync(query.UserId);
                if (identityUser == null)
                {
                    throw new InvalidOperationException($"User not found with ID: {query.UserId}");
                }

                return await _userManager.IsInRoleAsync(identityUser, ApplicationRole.Mentor);
            }
        }
    }
}