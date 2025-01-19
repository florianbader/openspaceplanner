<#
.SYNOPSIS
Creates a new EF Core migration with the specified name.

.PARAMETER MigrationName
The name of the migration to be created.

.DESCRIPTION
This script is used to create a new EF Core migration in the project.
You need to provide a name for the migration as a parameter.

.EXAMPLE
.\CreateMigration.ps1 -MigrationName "AddNewTable"
#>
    [string]$MigrationName
)

$ScriptDirectory = $PSScriptRoot

dotnet ef migrations add $MigrationName --context ApplicationDbContext --project "$ScriptDirectory/Persistence.csproj" --startup-project "$ScriptDirectory/../WebApis/Backend/WebApi/Backend.WebApi.csproj" --output-dir Migrations
