using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects
{
    public class TitleContains<T> : Specification<Project> where T : Queue<Project>
    {
        public string Title { get; }

        public TitleContains(string title)
        {
            Title = title;
        }

        internal override Expression<Func<Project, bool>> ToExpression()
        {
            return p => p.Title.ToLower().Contains(Title.ToLower());
        }
    }

    public class TitleContains : TitleContains<Queue<Project>>
    {
        public TitleContains(string title) : base(title)
        {
        }
    }
}
