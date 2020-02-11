using System.Threading.Tasks;
using NSwag;
using NSwag.SwaggerGeneration.Processors;
using NSwag.SwaggerGeneration.Processors.Contexts;

namespace ESFA.DC.PublicApi.FCS.Filters
{
    public class SwaggerOperationFilter : IOperationProcessor
    {
        public Task<bool> ProcessAsync(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.Parameters.Add(
                new SwaggerParameter
                {
                    Name = "Accept-Encoding",
                    Kind = SwaggerParameterKind.Header,
                    Type = NJsonSchema.JsonObjectType.String,
                    IsRequired = false,
                    Description = "For compressed response set value to 'gzip'",
                    Default = string.Empty,
                });

            return Task.FromResult(true);
        }
    }
}
