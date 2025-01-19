module appSqlDatabase 'app.bicep' = {
  name: 'databasesAppSqlDatabase'
}

output connectionString string = appSqlDatabase.outputs.connectionString
