using ESFA.DC.PublicApi.FCS.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ESFA.DC.PublicApi.FCS.Extensions
{
    public static class ResponseHeaderExtensions
    {
        public static void AddPaginationHeader<T>(this HttpResponse response, IPaginatedResult<T> data)
        {
            response.Headers.Add("X-Pagination", data.GetHeader().ToJson());
        }
    }
}
