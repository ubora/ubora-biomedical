using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Web._Features.Projects.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationIndexViewModel
    {
        public QuestionnaireListItem Last { get; set; }
        public IEnumerable<QuestionnaireListItem> Previous { get; set; }
        
        public class QuestionnaireListItem
        {
            public Guid QuestionnaireId { get; set; }
            public DateTime StartedAt { get; set; }
            public bool IsFinished { get; set; }
            public bool IsStopped { get; set; }
        }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            protected Factory()
            {
            }

            public virtual DeviceClassificationIndexViewModel Create(Guid projectId)
            {
                var isFromProject = new IsFromProjectSpec<DeviceClassificationAggregate> {ProjectId = projectId};
                var questionnaires = _queryProcessor.Find(isFromProject, new QuestionnaireListItemProjection(), null, Int32.MaxValue, 1)
                    .Where(x => !x.IsStopped)
                    .OrderByDescending(x => x.StartedAt)
                    .ToList();

                var latestStartedQuestionnaire = questionnaires.FirstOrDefault();
                if (latestStartedQuestionnaire != null)
                {
                    questionnaires.Remove(latestStartedQuestionnaire);
                }

                return new DeviceClassificationIndexViewModel
                {
                    Last = latestStartedQuestionnaire,
                    Previous = questionnaires
                };
            }
        }

        public class QuestionnaireListItemProjection : Projection<DeviceClassificationAggregate, QuestionnaireListItem>
        {
            protected override Expression<Func<DeviceClassificationAggregate, QuestionnaireListItem>>
                SelectExpression
                => x => new QuestionnaireListItem
                {
                    QuestionnaireId = x.Id,
                    StartedAt = x.StartedAt,
                    IsFinished = x.IsFinished,
                    IsStopped = x.IsStopped
                };
        }
    }
}
