using ESFA.DC.Logging.Interfaces;
using ESFA.DC.PublicApi.FCS.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESFA.DC.PublicApi.FCS.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError($"An error occured in External API for url: {context.HttpContext.Request.GetCompleteUrl()}", context.Exception);
        }
    }
}