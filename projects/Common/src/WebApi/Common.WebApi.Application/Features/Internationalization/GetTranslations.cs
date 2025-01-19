using System.IO.Abstractions;
using System.Reflection;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;
using Translations = System.Collections.Generic.Dictionary<string, string>;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Features.Internationalization;

public static class GetTranslations
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// The controller for handling internationalization.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    [Route("api/i18n/translations")]
    public class Controller(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Gets all localization texts.
        /// </summary>
        /// <param name="localeCode">The locale code. Defaults to english.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A dictionary of localization texts.</returns>
        [HttpGet("{localeCode}")]
        public Task<Translations> GetAllAsync(string localeCode, CancellationToken cancellationToken = default) =>
            mediator.Send(new Query(localeCode), cancellationToken);
    }

    public class Handler(IFileSystem fileSystem) : IRequestHandler<Query, Translations>
    {
        public async Task<Translations> Handle(Query request, CancellationToken cancellationToken)
        {
            var assemblyLocation =
                fileSystem.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                ?? throw new InvalidOperationException("Could not find assembly location");
            var contentRootPath = fileSystem.Path.Combine(assemblyLocation, "wwwroot");
            var translationFilesPath = fileSystem.Path.Combine(contentRootPath, "i18n");

            var translations = new Translations();
            var englishTranslationFiles = fileSystem.Directory.EnumerateFiles(translationFilesPath, "en.*.json");
            foreach (var translationFile in englishTranslationFiles)
            {
                var currentTranslations = await GetTranslationsFromFileAsync(
                    fileSystem,
                    translationFile,
                    cancellationToken
                );
                if (currentTranslations is null)
                {
                    continue;
                }

                MergeDictionaries(translations, currentTranslations);
            }

            return translations;
        }

        private static void MergeDictionaries(Translations translations, Translations currentTranslations)
        {
            foreach (var key in currentTranslations.Keys)
            {
                translations[key] = currentTranslations[key];
            }
        }

        private static async Task<Translations?> GetTranslationsFromFileAsync(
            IFileSystem fileSystem,
            string translationFile,
            CancellationToken cancellationToken
        )
        {
            await using var fileStream = fileSystem.File.OpenRead(translationFile);
            var currentTranslations = await JsonSerializer.DeserializeAsync<Translations>(
                fileStream,
                SerializerOptions,
                cancellationToken
            );
            return currentTranslations;
        }
    }

    public record Query(string LocaleCode) : IRequest<Translations>;
}
