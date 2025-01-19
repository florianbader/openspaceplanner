using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RioScaffolding.OpenSpacePlanner.Common.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Vogen;

[assembly: VogenDefaults(openApiSchemaCustomizations: OpenApiSchemaCustomizations.GenerateSwashbuckleSchemaFilter)]

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class OpenApi
{
    public static IApplicationBuilder UseOpenApi(this IApplicationBuilder builder) =>
        builder
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.EnableTryItOutByDefault();
                options.DisplayRequestDuration();
                options.EnableFilter();

                builder.AddApiVersioning(options);
            });

    public static IServiceCollection AddSwashbuckleOpenApi(
        this IServiceCollection serviceCollection,
        Action<SwaggerGenOptions>? configure = null
    ) =>
        serviceCollection
            .AddSwaggerGen(options =>
            {
                options.SchemaFilter<VogenSchemaFilterInRioScaffoldingOpenSpacePlannerCommonWebApi>();
                options.CustomSchemaIds(SchemaIdStrategy);

                options.CustomOperationIds(CustomOperationIds);

                options.TagActionsBy(EndpointNamespaceOrDefault);

                options.DescribeAllParametersInCamelCase();
                options.OperationFilter<OperationParameterOrdering>();

                options.IncludeXmlComments();

                options.SupportNonNullableReferenceTypes();
                options.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();
                options.ParameterFilter<RequireNonNullablePropertiesSchemaFilter>();
                options.RequestBodyFilter<RequireNonNullablePropertiesSchemaFilter>();

                configure?.Invoke(options);
            })
            .ConfigureOpenApiInfos();

    public static string SchemaIdStrategy(Type type)
    {
        var typeName = ConvertTypeName(type.Name);

        if (!type.IsGenericType)
        {
            return typeName;
        }

        var genericType = ConvertTypeName(type.GenericTypeArguments[0].Name);
        if (string.Equals(typeName, "ItemsResult", StringComparison.OrdinalIgnoreCase))
        {
            return $"{genericType}ItemsResult";
        }

        return $"{typeName}Of{genericType}";
    }

    private static string CustomOperationIds(ApiDescription operationIdSelector)
    {
        if (operationIdSelector.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            throw new InvalidOperationException(
                $"Could not find action controller action descriptor: {operationIdSelector.ActionDescriptor.DisplayName}"
            );
        }

        var declaringTypeName = controllerActionDescriptor.ControllerTypeInfo.DeclaringType?.Name;
        if (declaringTypeName is not null)
        {
            return declaringTypeName;
        }

        // fallback:
        // take the controller name which relates to the entity
        // and concat with the action name which should be the verb e.g. create, delete, update, get
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Namespace?.Split('.').Last();
        return controllerActionDescriptor.ActionName + controllerName;
    }

    private static void AddApiVersioning(this IApplicationBuilder builder, SwaggerUIOptions options)
    {
        var provider = builder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        var groupNames = provider.ApiVersionDescriptions.Select(d => d.GroupName);
        foreach (var groupName in groupNames)
        {
            var url = $"/swagger/{groupName}/swagger.json";
            var name = groupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    }

    private static IServiceCollection ConfigureOpenApiInfos(this IServiceCollection serviceCollection) =>
        serviceCollection.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    private static void IncludeXmlComments(this SwaggerGenOptions options)
    {
        var assemblyPrefix = Assembly.GetExecutingAssembly().GetProjectNamePrefix();

        foreach (var xmlFile in Directory.EnumerateFiles(AppContext.BaseDirectory, $"{assemblyPrefix}.*.xml"))
        {
            options.IncludeXmlComments(xmlFile);
        }
    }

    private static IList<string?> EndpointNamespaceOrDefault(ApiDescription api)
    {
        if (api.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
        {
            throw new InvalidOperationException(
                $"Could not find action controller action descriptor: {api.ActionDescriptor.DisplayName}"
            );
        }

        return new[] { actionDescriptor.ControllerTypeInfo.Namespace?.Split('.').Last() };
    }

    private static string ConvertTypeName(string typeName) =>
        typeName
            .Replace("Dto", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("`1", string.Empty, StringComparison.OrdinalIgnoreCase);

    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            var info = new OpenApiInfo() { Title = "API" };

            foreach (var description in _provider.ApiVersionDescriptions)
            {
                info.Version = description.ApiVersion.ToString();

                options.SwaggerDoc(description.GroupName, info);
            }
        }
    }

    internal class RequireNonNullablePropertiesSchemaFilter : ISchemaFilter, IParameterFilter, IRequestBodyFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var additionalRequiredProperties = schema
                .Properties.Where(property => !property.Value.Nullable && !schema.Required.Contains(property.Key))
                .Select(x => x.Key);

            foreach (var property in additionalRequiredProperties)
            {
                schema.Required.Add(property);
            }
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var isOptional = context.ParameterInfo?.IsOptional ?? false;
            parameter.Required = !isOptional && context.ApiParameterDescription.IsRequired;
        }

        public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context) =>
            requestBody.Required = context.BodyParameterDescription.IsRequired;
    }

    internal class OperationParameterOrdering : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // make sure we have a consistent order of parameters
            // 1. tenant id if it exists
            // 2. path parameters
            // 3. query parameters
            // 4. api version
            var newParameterOrder = new List<OpenApiParameter>(
                [
                    .. operation.Parameters.Where(p => p.Name.Equals("__tenant__", StringComparison.OrdinalIgnoreCase)),
                    .. operation.Parameters.Where(p =>
                        p.In == ParameterLocation.Path
                        && !p.Name.Equals("__tenant__", StringComparison.OrdinalIgnoreCase)
                    ),
                    .. operation.Parameters.Where(p =>
                        p.In == ParameterLocation.Query
                        && !p.Name.Equals("api-version", StringComparison.OrdinalIgnoreCase)
                    ),
                    .. operation.Parameters.Where(p => p.In == ParameterLocation.Header),
                    .. operation.Parameters.Where(p =>
                        p.Name.Equals("api-version", StringComparison.OrdinalIgnoreCase)
                    ),
                ]
            );

            operation.Parameters = newParameterOrder;
        }
    }
}
