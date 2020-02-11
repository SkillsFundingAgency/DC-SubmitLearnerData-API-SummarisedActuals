using ESFA.DC.Logging.Interfaces;
using ESFA.DC.PublicApi.FCS.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESFA.DC.PublicApi.FCS.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public LoggingFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogDebug($"External API called for url: {context.HttpContext.Request.GetCompleteUrl()}");
        }
    }
}
