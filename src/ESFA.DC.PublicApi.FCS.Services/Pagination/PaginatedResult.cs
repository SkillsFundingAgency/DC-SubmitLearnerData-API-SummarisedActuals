using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.PublicApi.FCS.Interfaces;

namespace ESFA.DC.PublicApi.FCS.Services.Pagination
{
    public class PaginatedResult<T> : IPaginatedResult<T>
    {
        public PaginatedResult(IQueryable<T> source, int pageSize, int pageNumber)
        {
            TotalItems = source.Count();
            PageNumber = pageNumber;
            PageSize = pageSize;
            List = source
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();
        }

        public int TotalItems { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public List<T> List { get; }

        public int TotalPages =>
            (int) Math.Ceiling(TotalItems / (double) PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public int NextPageNumber =>
            HasNextPage ? PageNumber + 1 : TotalPages;

        public int PreviousPageNumber =>
            HasPreviousPage ? PageNumber - 1 : 1;

        public IPagingHeader GetHeader()
        {
            return new PagingHeader(
                TotalItems, PageNumber, PageSize, TotalPages);
        }
    }
}