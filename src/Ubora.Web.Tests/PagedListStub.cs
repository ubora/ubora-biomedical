using System.Collections.Generic;
using Marten.Pagination;

namespace Ubora.Web.Tests
{
    public class PagedListStub<T> : List<T>, IPagedList<T>
    {
        public int Count { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int TotalItemCount { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
        public bool IsFirstPage { get; }
        public bool IsLastPage { get; }
        public int FirstItemOnPage { get; }
        public int LastItemOnPage { get; }
    }
}

