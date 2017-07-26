using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Specifications;

namespace Ubora.Domain.Projects.Queries
{
    public class GetAvailableProjectFilesQuery : IQuery<IEnumerable<ProjectFile>>
    {
        public Guid ProjectId { get; }

        public GetAvailableProjectFilesQuery(Guid projectId)
        {
            ProjectId = projectId;
        }

        public class Handler : QueryHandler<GetAvailableProjectFilesQuery, IEnumerable<ProjectFile>>
        {
            public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
            {
            }

            public override IEnumerable<ProjectFile> Handle(GetAvailableProjectFilesQuery query)
            {
                var projects = QueryProcessor.Find(new IsProjectFileSpec(query.ProjectId)
                    && !new IsHiddenFileSpec());

                return projects;
            }
        }
    }
}
