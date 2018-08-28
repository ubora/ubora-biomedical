using System;

namespace Ubora.Domain.Infrastructure.Queries
{
    public struct Paging
    {
        private int? _pageNumber;
        private int? _pageSize;

        public Paging(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentException();

            if (pageSize < 1)
                throw new ArgumentException();

            _pageNumber = pageNumber;
            _pageSize = pageSize;
        }

        public int PageSize
        {
            get => _pageSize ?? int.MaxValue;
            set => _pageSize = value;
        }

        public int PageNumber
        {
            get => _pageNumber ?? 1;
            set => _pageNumber = value;
        }

        public int SkipCount => (PageNumber - 1) * PageSize;
    }
}