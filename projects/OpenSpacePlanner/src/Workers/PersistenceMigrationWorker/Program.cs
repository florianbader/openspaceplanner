using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Aspire.ServiceDefaults;
using RioScaffolding.OpenSpacePlanner.Persistence;
using RioScaffolding.OpenSpacePlanner.Workers.PersistenceMigrationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.Services.AddSingleton(new TenantInfo());
builder.Services.AddDbContext<TenantApplicationDbContext>(configure =>
    configure.UseSqlServer(builder.Configuration.GetConnectionString("database"))
);
builder.EnrichSqlServerDbContext<TenantApplicationDbContext>();

var host = builder.Build();
await host.RunAsync();
