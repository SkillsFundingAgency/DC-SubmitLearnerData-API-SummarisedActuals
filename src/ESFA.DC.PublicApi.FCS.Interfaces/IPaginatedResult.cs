using System.Collections.Generic;

namespace ESFA.DC.PublicApi.FCS.Interfaces
{
    public interface IPaginatedResult<T>
    {
        int TotalItems { get; }

        int PageNumber { get; }

        int PageSize { get; }

        List<T> List { get; }

        int TotalPages { get; }
            
        bool HasPreviousPage { get; }

        bool HasNextPage { get; }

        int NextPageNumber { get; }

        int PreviousPageNumber { get; }

        IPagingHeader GetHeader();
    }
}
