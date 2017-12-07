using System;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Projects.Workpackages.Queries
{
    public class GetStatusesOfProjectWorkpackagesQuery : IQuery<GetStatusesOfProjectWorkpackagesQuery.Result>
    {
        public GetStatusesOfProjectWorkpackagesQuery(Guid projectId)
        {
            ProjectId = projectId;
        }

        public Guid ProjectId { get; }

        public class Result
        {
            public Result(WorkpackageStatus wp1Status, WorkpackageStatus wp2Status, WorkpackageStatus wp3Status, WorkpackageStatus wp4Status, WorkpackageStatus wp5Status, WorkpackageStatus wp6Status)
            {
                Wp1Status = wp1Status;
                Wp2Status = wp2Status;
                Wp3Status = wp3Status;
                Wp4Status = wp4Status;
                Wp5Status = wp5Status;
                Wp6Status = wp6Status;
            }

            public WorkpackageStatus Wp1Status { get; }
            public WorkpackageStatus Wp2Status { get; }
            public WorkpackageStatus Wp3Status { get; }
            public WorkpackageStatus Wp4Status { get; }
            public WorkpackageStatus Wp5Status { get; }
            public WorkpackageStatus Wp6Status { get; }
        }

        public class Handler : IQueryHandler<GetStatusesOfProjectWorkpackagesQuery, GetStatusesOfProjectWorkpackagesQuery.Result>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public Result Handle(GetStatusesOfProjectWorkpackagesQuery query)
            {
                var batch = _documentSession.CreateBatchQuery();

                var wp1 = batch.Query<WorkpackageOne>()
                    .Where(wp => wp.ProjectId == query.ProjectId)
                    .Select(wp => new IntermediateResult { HasBeenAcceptedByReview = wp.HasBeenAccepted })
                    .FirstOrDefault();

                var wp2 = batch.Query<WorkpackageTwo>()
                    .Where(wp => wp.ProjectId == query.ProjectId)
                    .Select(wp => new IntermediateResult { HasBeenAcceptedByReview = wp.HasBeenAccepted })
                    .FirstOrDefault();

                var wp3 = batch.Query<WorkpackageThree>()
                    .Where(wp => wp.ProjectId == query.ProjectId)
                    .Select(wp => new IntermediateResult { HasBeenAcceptedByReview = wp.HasBeenAccepted })
                    .FirstOrDefault();

                batch.ExecuteSynchronously();

                return new Result
                (
                    wp1Status: GetStatus(wp1.Result),
                    wp2Status: GetStatus(wp2.Result),
                    wp3Status: GetStatus(wp3.Result),
                    wp4Status: WorkpackageStatus.Closed,
                    wp5Status: WorkpackageStatus.Closed,
                    wp6Status: WorkpackageStatus.Closed
                );
            }

            private WorkpackageStatus GetStatus(IntermediateResult workpackage)
            {
                if (workpackage == null)
                {
                    return WorkpackageStatus.Closed;
                }

                if (workpackage.HasBeenAcceptedByReview)
                {
                    return WorkpackageStatus.Accepted;
                }

                return WorkpackageStatus.Opened;
            }

            private class IntermediateResult
            {
                public bool HasBeenAcceptedByReview { get; set; }
            }
        }
    }
}
