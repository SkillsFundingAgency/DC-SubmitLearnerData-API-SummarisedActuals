using Microsoft.AspNetCore.Http;

namespace ESFA.DC.PublicApi.FCS.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetCompleteUrl(this HttpRequest request)
        {
            var url = $@"{request.Host.Value}{request.Path}";
            url = request.QueryString.HasValue ? $"{url}{request.QueryString.Value}" : url;
            return url;
        }
    }
}
