using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
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
                var quickInfoes = new Dictionary<Guid, ClinicalNeedQuickInfo>();
                var userProfiles = new Dictionary<Guid, UserProfile>();

                var clinicalNeeds = 
                    _querySession
                        .Query<ClinicalNeed>()
                        .Include(cn => cn.Id, quickInfoes)
                        .Include(cn => cn.IndicatorUserId, userProfiles) // Would actually prefer a "select" of full name (to optimize)
                        .Select(clinicalNeed => new ClinicalNeedViewModel
                        {
                            Id = clinicalNeed.Id,
                            Title = clinicalNeed.Title,
                            AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                            ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                            PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                            Keywords = clinicalNeed.Keywords,
                            IndicatedAt = clinicalNeed.IndicatedAt,
                            IndicatorUserId = clinicalNeed.IndicatorUserId
                        }).ToList();

                clinicalNeeds = clinicalNeeds.Select(cn =>
                {
                    cn.NumberOfComments = quickInfoes[cn.Id].NumberOfComments;
                    cn.NumberOfRelatedProjects = quickInfoes[cn.Id].NumberOfRelatedProjects;
                    cn.IndicatorFullName = userProfiles[cn.IndicatorUserId].FullName;
                    return cn;
                }).ToList();

                return new LandingPageViewModel
                {
                    ClinicalNeeds = clinicalNeeds
                };
            }
        }
    }
}