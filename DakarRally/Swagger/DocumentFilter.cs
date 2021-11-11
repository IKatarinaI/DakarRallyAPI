using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace DakarRally.API.Swagger
{
    public class DocumentFilter : IDocumentFilter
    {
        public DocumentFilter()
        {
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc == null)
                return;

            // Re-order the schemas alphabetically.
            swaggerDoc.Components.Schemas = swaggerDoc.Components.Schemas
                                                .OrderBy(kvp => kvp.Key, StringComparer.InvariantCulture)
                                                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
