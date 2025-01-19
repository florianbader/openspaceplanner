using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.WebApi;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public void UseConfiguration(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { { "DatabaseConnectionString", "memory" } })
            .Build();

        builder.UseConfiguration(configuration);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.UseEnvironment("Testing");
}
