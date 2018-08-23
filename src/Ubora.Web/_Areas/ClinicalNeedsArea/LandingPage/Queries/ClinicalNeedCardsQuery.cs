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
                    .Include(cn => cn.Id, quickInfoes)
                    .Include(cn => cn.IndicatorUserId, userProfiles) // Would actually prefer a "select" of full name (to optimize)
                    .Select(clinicalNeed => new ClinicalNeedCardViewModel
                    {
                        Id = clinicalNeed.Id,
                        Title = clinicalNeed.Title,
                        AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                        ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                        PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                        Keywords = clinicalNeed.Keywords,
                        IndicatedAt = clinicalNeed.IndicatedAt,
                        IndicatorUserId = clinicalNeed.IndicatorUserId
                    })
                    .AsPagedList(query.Paging.PageNumber, query.Paging.PageSize);

                var clinicalNeeds = clinicalNeedsPaged.Select(cn =>
                {
                    cn.NumberOfComments = quickInfoes[cn.Id].NumberOfComments;
                    cn.NumberOfRelatedProjects = quickInfoes[cn.Id].NumberOfRelatedProjects;
                    cn.IndicatorFullName = userProfiles[cn.IndicatorUserId].FullName;
                    return cn;
                }).ToList();

                return new PagedList2<ClinicalNeedCardViewModel>(clinicalNeeds, clinicalNeedsPaged);
            }
        }
    }
}