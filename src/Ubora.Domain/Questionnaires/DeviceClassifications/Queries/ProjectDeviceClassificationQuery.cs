using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Queries
{
    public class LatestFinishedProjectDeviceClassificationQuery : IQuery<DeviceClassificationAggregate>
    {
        public Guid ProjectId { get; private set; }

        public LatestFinishedProjectDeviceClassificationQuery(Guid projectId)
        {
            ProjectId = projectId;
        }

        internal class Handler : IQueryHandler<LatestFinishedProjectDeviceClassificationQuery, DeviceClassificationAggregate>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public DeviceClassificationAggregate Handle(LatestFinishedProjectDeviceClassificationQuery query)
            {
                var lastFinishedDeviceClassification = _querySession.Query<DeviceClassificationAggregate>()
                    .Where(x => x.ProjectId == query.ProjectId && x.FinishedAt != null)
                    .OrderByDescending(x => x.FinishedAt)
                    .FirstOrDefault();

                return lastFinishedDeviceClassification;
            }
        }
    }
}
