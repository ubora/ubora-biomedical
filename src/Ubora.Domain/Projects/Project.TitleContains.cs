using Marten.Util;
using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects
{
    public class TitleContains : Specification<Project>
    {
        public string Title { get; }

        public TitleContains(string title)
        {
            Title = title;
        }

        //Method string.ToLower() doesn't works well in Query from marten
        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return p => p.Title.Contains(Title, StringComparison.OrdinalIgnoreCase);
        }
    }
}
