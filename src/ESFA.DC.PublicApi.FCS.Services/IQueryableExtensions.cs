using System.Collections.Generic;
using System.Linq;

namespace ESFA.DC.PublicApi.FCS.Services
{
    public static class IQueryableExtensions
    {
        public static List<T> PagedList<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();
        }

        public static int TotalItems<T>(this IQueryable<T> query)
        {
            return query.Count();
        }
    }
}