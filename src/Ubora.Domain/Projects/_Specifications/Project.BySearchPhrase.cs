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
                p.ClinicalNeedTags.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ||
                p.AreaOfUsageTags.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ||
                p.PotentialTechnologyTags.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ||
                p.Gmdn.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase);
        }
    }
}
