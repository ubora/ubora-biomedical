using System;
using System.Collections.Generic;
using Marten.Pagination;

namespace Ubora.Domain.Infrastructure.Queries
{
    public class PagedList2<T> : BasePagedList<T>
    {
        public PagedList2(IReadOnlyCollection<T> items, IPagedList<object> oldPagedList)
            : base(oldPagedList.PageNumber, oldPagedList.PageSize, oldPagedList.TotalItemCount)
        {
            if (items.Count != oldPagedList.Count)
                throw new InvalidOperationException("The count of items in paged lists doesn't match. Please only use this to carry over the metadata from the old paged list.");

            _subset.AddRange(items);
        }
    }
}