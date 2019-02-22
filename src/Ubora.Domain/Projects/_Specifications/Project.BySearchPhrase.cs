using System;
using System.Linq.Expressions;
using Marten.Util;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects._Specifications
{
    public class BySearchPhrase : Specification<Project>
    {
        public string SearchPhrase { get; }

        public BySearchPhrase(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        //Method string.ToLower() doesn't works well in Query from marten
        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return p =>
                p.Title.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ||
                p.ClinicalNeedTag.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ||
                p.AreaOfUsageTag.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ||
                p.PotentialTechnologyTag.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ||
                p.Keywords.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase);
        }
    }
}
