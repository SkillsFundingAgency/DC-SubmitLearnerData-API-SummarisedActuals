using ESFA.DC.PublicApi.FCS.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ESFA.DC.PublicApi.FCS.Services.Pagination
{
    public class PagingHeader : IPagingHeader
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PagingHeader" /> class.
        /// </summary>
        /// <param name="totalItems">TotalItems</param>
        /// <param name="pageNumber">PageNumber</param>
        /// <param name="pageSize">PageSize</param>
        /// <param name="totalPages">TotalPages</param>
        public PagingHeader(
            int totalItems, int pageNumber, int pageSize, int totalPages)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public int TotalItems { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalPages { get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(
                this,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}