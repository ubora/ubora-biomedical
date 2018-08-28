using System;
using System.Collections.Generic;
using Marten.Pagination;

namespace Ubora.Domain.Infrastructure.Queries
{
    public class InMemoryPagedList<T> : BasePagedList<T>
    {
        public InMemoryPagedList(IReadOnlyCollection<T> items, IPagedList<object> oldPagedList)
            : base(
                  pageNumber: oldPagedList.PageNumber > 0 ? oldPagedList.PageNumber : 1, // Weird solution because Marten currently leaves PageNumber and PageSize as zeroes when TotalItemCount is zero: https://github.com/JasperFx/marten/blob/master/src/Marten/Pagination/PagedList.cs#L36
                  pageSize: oldPagedList.PageSize > 0 ? oldPagedList.PageSize : 1,
                  totalItemCount: oldPagedList.TotalItemCount)
        {
            if (items.Count != oldPagedList.Count)
                throw new InvalidOperationException("The count of items in paged lists doesn't match. Please only use this to carry over the metadata from the old paged list.");

            _subset.AddRange(items);
        }

        public InMemoryPagedList(IReadOnlyCollection<T> items, Paging paging, int totalItemCount)
            : base(
                  pageNumber: paging.PageNumber, 
                  pageSize: paging.PageSize, 
                  totalItemCount: totalItemCount)
        {
            _subset.AddRange(items); 
        }
    }
}