using System.Globalization;
using Hellang.Middleware.ProblemDetails;
using Microsoft.FeatureManagement;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Aspire.ServiceDefaults;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;
using RioScaffolding.OpenSpacePlanner.Persistence;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithCorrelationId()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Properties} {Message:lj} {Exception}",
        formatProvider: CultureInfo.InvariantCulture
    )
    .CreateLogger();

builder.Host.UseSerilog();
builder.AddServiceDefaults();

builder.Services.AddEntityFramework<TenantApplicationDbContext>(builder.Configuration);
builder.EnrichSqlServerDbContext<TenantApplicationDbContext>();

// Add framework services
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHealthChecks();
builder.Services.AddFeatureManagement();
builder.Services.AddMultiTenancy<TenantApplicationDbContext>();
builder.Services.AddCustomControllers();
builder.Services.AddCustomApiVersioning();
builder.Services.AddSwashbuckleOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediator();
builder.Services.AddProblemDetailsMapping();
builder.Services.AddGridify();
builder.Services.AddFileSystem();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseOpenApi();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCustomCors();
app.UseProblemDetails();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseMultiTenancy();

app.UseResponseCaching();
app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/health");

await app.RunAsync();

// Make the implicit Program class public so test projects can access it
#pragma warning disable S1118 // Utility classes should not have public constructors

public partial class Program;

#pragma warning restore S1118 // Utility classes should not have public constructors
