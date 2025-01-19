var builder = DistributedApplication.CreateBuilder(args);

var sqlServerPassword = builder.AddParameter("sqlServerPassword", secret: true);

var database = builder
    .AddSqlServer("sqlserver", sqlServerPassword, 1433)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .AddDatabase("database", "openspaceplanner");

var databseMigrations = builder
    .AddProject<Projects.Workers_PersistenceMigrationWorker>("migrations")
    .WithReference(database)
    .WaitFor(database);

var backendApi = builder
    .AddProject<Projects.Backend_WebApi>("backend-webapi")
    .WithReference(database)
    .WaitFor(databseMigrations);

builder
    .AddNodeApp("frontend-openspaceplanner", "node_modules/.bin/nx", "../../../../", ["serve", "openspaceplanner"])
    .WaitFor(backendApi)
    .PublishAsDockerFile();

await builder.Build().RunAsync();
