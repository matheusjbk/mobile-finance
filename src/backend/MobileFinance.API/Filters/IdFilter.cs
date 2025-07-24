using Microsoft.OpenApi.Models;
using MobileFinance.API.Binders;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MobileFinance.API.Filters;

public class IdFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var encryptedIds = context.ApiDescription.ParameterDescriptions
            .Where(x => x.ModelMetadata.BinderType == typeof(MobileFinanceIdBinder)).ToDictionary(d => d.Name, d => d);

        foreach(var parameter in operation.Parameters)
        {
            if(encryptedIds.TryGetValue(parameter.Name, out _))
            {
                parameter.Schema.Format = string.Empty;
                parameter.Schema.Type = "string";
            }
        }

        foreach(var schema in context.SchemaRepository.Schemas.Values)
        {
            foreach(var property in schema.Properties)
            {
                if(encryptedIds.TryGetValue(property.Key, out _))
                {
                    property.Value.Format = string.Empty;
                    property.Value.Type = "string";
                }
            }
        }
    }
}
