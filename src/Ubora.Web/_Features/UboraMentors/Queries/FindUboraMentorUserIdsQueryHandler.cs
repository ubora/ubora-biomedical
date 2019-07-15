using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.UboraMentors.Queries
{
    public class FindUboraMentorUserIdsQueryHandler : IQueryHandler<FindUboraMentorUserIdsQuery, IReadOnlyCollection<Guid>>
    {
        private IApplicationUserManager _applicationUserManager;
        public FindUboraMentorUserIdsQueryHandler(IApplicationUserManager applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        public IReadOnlyCollection<Guid> Handle(FindUboraMentorUserIdsQuery query)
        {
            var mentorUserIds = _applicationUserManager.GetUsersInRoleAsync(ApplicationRole.Mentor)
                .GetAwaiter().GetResult()
                .Select(x => x.Id)
                .ToList();
            return mentorUserIds;
        }
    }
}
