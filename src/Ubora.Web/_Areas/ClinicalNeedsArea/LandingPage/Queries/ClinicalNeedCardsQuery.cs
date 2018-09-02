using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Marten.Pagination;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Queries
{
    public class ClinicalNeedCardsQuery : IQuery<IPagedList<ClinicalNeedCardViewModel>>
    {
        public Paging Paging { get; set; }

        public class Handler : IQueryHandler<ClinicalNeedCardsQuery, IPagedList<ClinicalNeedCardViewModel>>
        {
            private readonly IQuerySession _querySession;

            public Handler(IQuerySession querySession)
            {
                _querySession = querySession;
            }

            public IPagedList<ClinicalNeedCardViewModel> Handle(ClinicalNeedCardsQuery query)
            {
                var quickInfoes = new Dictionary<Guid, ClinicalNeedQuickInfo>();
                var userProfiles = new Dictionary<Guid, UserProfile>();

                var clinicalNeedsPaged = _querySession
                    .Query<ClinicalNeed>()
                    .Stats(out var queryStats)
                    .OrderByDescending(cn => cn.IndicatedAt)
                    .Include(cn => cn.Id, quickInfoes)
                    .Include(cn => cn.IndicatorUserId, userProfiles) // For indicator's full name
                    .Select(clinicalNeed => new ClinicalNeedCardViewModel
                    {
                        Id = clinicalNeed.Id,
                        Title = clinicalNeed.Title,
                        AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                        ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                        PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                        Keywords = clinicalNeed.Keywords,
                        LastActivityAt = clinicalNeed.IndicatedAt,
                        IndicatorUserId = clinicalNeed.IndicatorUserId
                    })
                    .Skip(query.Paging.SkipCount)
                    .Take(query.Paging.PageSize)
                    .ToList();

                var clinicalNeeds = clinicalNeedsPaged.Select(cn =>
                {
                    cn.NumberOfComments = quickInfoes[cn.Id].NumberOfComments;
                    cn.NumberOfRelatedProjects = quickInfoes[cn.Id].NumberOfRelatedProjects;
                    cn.IndicatorFullName = userProfiles[cn.IndicatorUserId].FullName;
                    cn.LastActivityAt = quickInfoes[cn.Id].LastActivityAt;
                    return cn;
                }).ToList();

                return new Domain.Infrastructure.Queries.PagedList<ClinicalNeedCardViewModel>(clinicalNeeds, query.Paging, (int)queryStats.TotalResults);
            }
        }
    }
}