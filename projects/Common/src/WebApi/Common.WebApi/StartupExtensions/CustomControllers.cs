using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class CustomControllers
{
    public static void AddCustomControllers(this IServiceCollection services) =>
        services
            .AddControllers(options =>
                options.Conventions.Add(
                    new CustomRouteToken("controller", c => c.ControllerType.Namespace?.Split('.').Last())
                )
            ) // adds namespace route parameter so the handler controller have the same parent name
            .ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new CustomControllerFeatureProvider());
            }) // configure custom controller feature provider to identify our handler controllers
            .AddApplicationPart(typeof(ApiControllerBase).Assembly) // adds the controllers from the Application project
            .AddControllersAsServices() // registers the controllers in dependency injection
            .AddJsonSerializerOptions(); // adds System.Text.Json serializer options for serializing and deserializing json requests and responses

    internal class CustomRouteToken(string tokenName, Func<ControllerModel, string?> valueGenerator)
        : IApplicationModelConvention
    {
        private readonly string _tokenRegex = $@"(\[{tokenName}])(?<!\[\1(?=]))";
        private readonly Func<ControllerModel, string?> _valueGenerator = valueGenerator;

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var tokenValue = _valueGenerator(controller);

                UpdateSelectors(controller.Selectors, tokenValue);
                UpdateSelectors(controller.Actions.SelectMany(a => a.Selectors), tokenValue);
            }
        }

        private void UpdateSelectors(IEnumerable<SelectorModel> selectors, string? tokenValue)
        {
            var routeModels = selectors.Select(s => s.AttributeRouteModel).OfType<AttributeRouteModel>();
            foreach (var routeModel in routeModels)
            {
                routeModel.Template = InsertTokenValue(routeModel.Template, tokenValue);
                routeModel.Name = InsertTokenValue(routeModel.Name, tokenValue);
            }
        }

        private string? InsertTokenValue(string? template, string? tokenValue)
        {
            if (template is null)
            {
                return template;
            }

            return Regex.Replace(template, _tokenRegex, tokenValue ?? string.Empty);
        }
    }

    internal class CustomControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var isCustomController = !typeInfo.IsAbstract && typeof(ApiControllerBase).IsAssignableFrom(typeInfo);
            return isCustomController || base.IsController(typeInfo);
        }
    }
}
