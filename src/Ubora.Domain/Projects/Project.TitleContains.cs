using Marten.Util;
using System;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects
{
    public class TitleContains<T> : Specification<Project> where T : IQueryable<Project>
    {
        public string Title { get; }

        public TitleContains(string title)
        {
            Title = title;
        }

        //Method string.ToLower() doesn't works well in Query from marten
        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return p => p.Title.Contains(Title.ToLower(), StringComparison.CurrentCultureIgnoreCase);
        }
    }

    public class TitleContains : TitleContains<IQueryable<Project>>
    {
        public TitleContains(string title) : base(title)
        {
        }
    }
}
