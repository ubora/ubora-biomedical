using System.Web;
using AngleSharp.Network.Default;
using Marten.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features._Shared.Paging
{
    public class Pager
    {
        public static Pager From<T>(IPagedList<T> pagedList)
        {
            var pager = new Pager
            {
                Count = pagedList.Count,
                FirstItemOnPage = pagedList.FirstItemOnPage,
                HasNextPage = pagedList.HasNextPage,
                HasPreviousPage = pagedList.HasPreviousPage,
                IsFirstPage = pagedList.IsFirstPage,
                IsLastPage = pagedList.IsLastPage,
                LastItemOnPage = pagedList.LastItemOnPage,
                PageCount = pagedList.PageCount,
                PageNumber = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                TotalItemCount = pagedList.TotalItemCount
            };
            return pager;
        }

        public int Count { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public int PageCount { get; private set; }

        public int TotalItemCount { get; private set; }

        public bool HasPreviousPage { get; private set; }

        public bool HasNextPage { get; private set; }

        public bool IsFirstPage { get; private set; }

        public bool IsLastPage { get; private set; }

        public int FirstItemOnPage { get; private set; }

        public int LastItemOnPage { get; private set; }
    }
}