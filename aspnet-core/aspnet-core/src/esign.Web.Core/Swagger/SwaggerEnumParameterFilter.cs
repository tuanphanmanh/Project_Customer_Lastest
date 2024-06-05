using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace esign.Web.Swagger
{
    public class SwaggerEnumParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var type = Nullable.GetUnderlyingType(context.ApiParameterDescription.Type) ?? context.ApiParameterDescription.Type;
            if (type.IsEnum)
            {
                AddEnumParamSpec(parameter, type, context);
                parameter.Required = type == context.ApiParameterDescription.Type;
            }
            else if (type.IsArray || (type.IsGenericType && type.GetInterfaces().Contains(typeof(IEnumerable))))
            {
                var itemType = type.GetElementType() ?? type.GenericTypeArguments.First();
                AddEnumSpec(itemType, context);
            }
        }

        private static void AddEnumSpec(Type type, ParameterFilterContext context)
        {
            var schema = context.SchemaRepository.Schemas.GetOrAdd($"{type.Name}", () =>
                context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository)
            );

            if (schema.Reference == null || !type.IsEnum)
            {
                return;
            }

            var enumNames = new OpenApiArray();
            enumNames.AddRange(Enum.GetNames(type).Select(_ => new OpenApiString(_)));

            if (schema.Extensions.ContainsKey("x-enumNames"))
            {
                var existingEnums = schema.Extensions["x-enumNames"] as OpenApiArray;
                foreach (var enumName in enumNames)
                {
                    existingEnums.AddIfNotContains(enumName);
                }

                schema.Extensions["x-enumNames"] = existingEnums;
            }
            else
            {
                schema.Extensions.Add("x-enumNames", enumNames);   
            }
        }

        private static void AddEnumParamSpec(OpenApiParameter parameter, Type type, ParameterFilterContext context)
        {
            var schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
            if (schema.Reference == null)
            {
                return;
            }

            parameter.Schema = schema;
            
            var enumNames = new OpenApiArray();
            enumNames.AddRange(Enum.GetNames(type).Select(_ => new OpenApiString(_)));
            schema.Extensions.Add("x-enumNames", enumNames);
        }
    }

    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
            => _apiVersionDescriptionProvider = apiVersionDescriptionProvider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }

        private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "esign API",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " (deprecated)";
            }

            return info;
        }
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description is null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default is null && description.DefaultValue is not null)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}
