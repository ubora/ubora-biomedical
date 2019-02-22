using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Queries;
using Ubora.Web._Features._Shared.Paging;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage
{
    public class ClinicalNeedCardsViewComponent : ViewComponent
    {
        private readonly IQueryProcessor _queryProcessor;

        public ClinicalNeedCardsViewComponent(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public Task<IViewComponentResult> InvokeAsync(int pageNumber = 1, int pageSize = int.MaxValue, bool showPager = true)
        {
            var clinicalNeedCardsPaged = _queryProcessor.ExecuteQuery(new ClinicalNeedCardsQuery
            {
                Paging = new Paging(pageNumber: pageNumber, pageSize: pageSize)
            });

            var viewModel = new ClinicalNeedCardsViewComponentViewModel
            {
                Pager = Pager.From(clinicalNeedCardsPaged),
                ShowPager = showPager,
                ClinicalNeedCards = clinicalNeedCardsPaged.ToList()
            };

            return Task.FromResult<IViewComponentResult>(
                View("~/_Areas/ClinicalNeedsArea/LandingPage/_ClinicalNeedsViewComponent.cshtml", viewModel));
        }
    }
}