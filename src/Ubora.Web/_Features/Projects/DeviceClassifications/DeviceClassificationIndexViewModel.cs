using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.DeviceClassifications;

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
            private readonly IProjection<DeviceClassificationAggregate, QuestionnaireListItem> _questionnaireListItemProjection;

            public Factory(IQueryProcessor queryProcessor, IProjection<DeviceClassificationAggregate, QuestionnaireListItem> questionnaireListItemProjection)
            {
                _queryProcessor = queryProcessor;
                _questionnaireListItemProjection = questionnaireListItemProjection;
            }

            protected Factory()
            {
            }

            public virtual DeviceClassificationIndexViewModel Create(Guid projectId)
            {
                var isFromProject = new IsFromProjectSpec<DeviceClassificationAggregate> { ProjectId = projectId };
                // Can't apply this projection in db now, because of computed fields (can be refactored later, if needed)
                var questionnaires = _questionnaireListItemProjection.Apply(
                                            _queryProcessor.Find(isFromProject, null, Int32.MaxValue, 1))
                    .Where(x => !x.IsStopped)
                    .OrderByDescending(x => x.StartedAt)
                    .ToList();

                var latestStartedQuestionnaire = questionnaires.FirstOrDefault();
                if (latestStartedQuestionnaire != null && !latestStartedQuestionnaire.IsFinished)
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
            protected override Expression<Func<DeviceClassificationAggregate, QuestionnaireListItem>> ToSelector()
            {
                return x => new QuestionnaireListItem
                {
                    QuestionnaireId = x.Id,
                    StartedAt = x.StartedAt,
                    IsFinished = x.IsFinished,
                    IsStopped = x.IsStopped
                };
            }
        }
    }
}
