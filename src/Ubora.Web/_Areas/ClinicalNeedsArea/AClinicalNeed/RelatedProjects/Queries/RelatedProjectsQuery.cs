using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.RelatedProjects.Models;
using Ubora.Web._Components;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.RelatedProjects.Queries
{
    public class RelatedProjectsQuery : IQuery<RelatedProjectsViewModel>
    {
        public Guid ClinicalNeedId { get; set; }

        public class Handler : IQueryHandler<RelatedProjectsQuery, RelatedProjectsViewModel>
        {
            private readonly IQuerySession _querySession;
            private readonly ProjectCardViewModel.Factory _projectCardFactory;

            public Handler(IQuerySession querySession, ProjectCardViewModel.Factory projectCardFactory)
            {
                _querySession = querySession;
                _projectCardFactory = projectCardFactory;
            }

            public RelatedProjectsViewModel Handle(RelatedProjectsQuery query)
            {
                var relatedProjects = 
                    _querySession
                        .Query<Project>()
                        .Where(p => p.RelatedClinicalNeeds.Contains(query.ClinicalNeedId))
                        .ToList();

                var relatedProjectCards = relatedProjects.Select(p => _projectCardFactory.Create(p)).ToList();

                return new RelatedProjectsViewModel
                {
                    RelatedProjects = relatedProjectCards
                };
            }
        }
    }
}
