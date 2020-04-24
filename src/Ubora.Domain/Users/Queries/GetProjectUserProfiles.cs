using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members.Queries;

namespace Ubora.Domain.Users.Queries
{
    public class GetProjectUserProfiles : IQuery<IReadOnlyCollection<UserProfile>>
    {
        public Guid ProjectId { get; set; }
        
        internal class Handler : IQueryHandler<GetProjectUserProfiles, IReadOnlyCollection<UserProfile>>
        {
            private readonly IQuerySession _querySession;
            private readonly IQueryProcessor _queryProcessor;
            
            public Handler(IQuerySession querySession, IQueryProcessor queryProcessor)
            {
                _querySession = querySession;
                _queryProcessor = queryProcessor;
            }
            
            public IReadOnlyCollection<UserProfile> Handle(GetProjectUserProfiles query)
            {
                var projectMemberGroups = _querySession.Load<Project>(query.ProjectId).Members.GroupBy(m => m.UserId);
                var userIds = projectMemberGroups.Select(m => m.Key);
                var projectMemberUserProfiles = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery {UserIds = userIds});

                return projectMemberUserProfiles;
            }
        } 
    }
}