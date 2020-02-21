using System.Threading.Tasks;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace ESFA.DC.PublicApi.FCS.Filters
{
    public class SwaggerOperationFilter : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Encoding",
                Kind = OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = false,
                Description = "For compressed response set value to 'gzip'",
                Default = string.Empty,
            });

            return true;
        }
    }
}
