using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class JsonSerializer
{
    public static void AddJsonSerializerOptions(this IMvcBuilder mvcBuilder) =>
        mvcBuilder.AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore null values when serializing
            opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Ignore case when deserializing
            opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Use camel case for property names
            opt.JsonSerializerOptions.AllowTrailingCommas = true; // Allow trailing commas in JSON
            opt.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase; // Use camel case for dictionary keys
        });
}
