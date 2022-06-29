using Microsoft.OpenApi.Models;
using Quotes.Import.Service.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Quotes.Import.Service.OperationFilters
{
    internal class ApiKeyHeaderParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = ApiKeyAttribute.ApiKeyName,
                In = ParameterLocation.Header,
                Required = true
            });
        }
    }
}