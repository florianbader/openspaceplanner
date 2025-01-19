# Persistence

This project contains the persistence logic utilized by the project.

## Database

The Persistence project defines and manages the database connection using Entity Framework Core.

Database changes are deployed via migrations, which apply new changes to the database. After modifying the code, create a new migration by running the following command:

```powershell
CreateMigration.ps1 -MigrationName "AddNewTable"
```
