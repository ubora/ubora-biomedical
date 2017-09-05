using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class IndexViewModel
    {
        public QuestionnaireListItem Last { get; set; }
        public IEnumerable<QuestionnaireListItem> Previous { get; set; }

        public class QuestionnaireListItem
        {
            public Guid QuestionnaireId { get; set; }
            public DateTime StartedAt { get; set; }
        }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public IndexViewModel Create(Guid projectId)
            {
                var questionnaires = _queryProcessor.Find<ApplicableRegulationsQuestionnaireAggregate>()
                    .Where(x => x.ProjectId == projectId)
                    .OrderByDescending(x => x.StartedAt)
                    .Select(x => new QuestionnaireListItem
                    {
                        QuestionnaireId = x.Id,
                        StartedAt = x.StartedAt
                    })
                    .ToList();

                var lastQuestionnaire = questionnaires.FirstOrDefault();
                if (lastQuestionnaire != null)
                {
                    questionnaires.Remove(lastQuestionnaire);
                }

                return new IndexViewModel
                {
                    Last = lastQuestionnaire,
                    Previous = questionnaires
                };
            }
        }
    }
}
