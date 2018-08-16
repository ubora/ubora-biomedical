using System.Linq;
using Marten;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Queries
{
    public class ClinicalNeedsLandingPageQuery : IQuery<LandingPageViewModel>
    {
        public class Handler : IQueryHandler<ClinicalNeedsLandingPageQuery, LandingPageViewModel>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public LandingPageViewModel Handle(ClinicalNeedsLandingPageQuery query)
            {
                return new LandingPageViewModel
                {
                    ClinicalNeeds = _querySession.Query<ClinicalNeed>().Select(clinicalNeed => new ClinicalNeedViewModel
                    {
                        Id = clinicalNeed.Id,
                        Title = clinicalNeed.Title,
                        AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                        ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                        PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                        Keywords = clinicalNeed.Keywords,
                        IndicatedAt = clinicalNeed.IndicatedAt
                    }).ToList()
                };
            }
        }
    }
}