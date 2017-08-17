using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Projects.InviteMentors
{
    public class FindProjectMentorProfilesQuery : IQuery<IReadOnlyCollection<UserProfile>>
    {
        public Guid ProjectId { get; set; }

        public class Handler : IQueryHandler<FindProjectMentorProfilesQuery, IReadOnlyCollection<UserProfile>>
        {
            private readonly IQueryProcessor _queryProcessor;

            public Handler(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public IReadOnlyCollection<UserProfile> Handle(FindProjectMentorProfilesQuery query)
            {
                var project = _queryProcessor.FindById<Project>(query.ProjectId);

                var projectMentorUserIds = project.Members
                    // TODO(Kaspar Kallas): Use specification instead.
                    .Where(m => m is ProjectMentor)
                    .Select(x => x.UserId);

                var projectMentor = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery
                {
                    UserIds = projectMentorUserIds
                });

                return projectMentor;
            }
        }
    }
}