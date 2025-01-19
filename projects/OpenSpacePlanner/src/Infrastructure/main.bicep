import * as common from '../../../Common/src/Infrastructure/_common.bicep'

targetScope = 'subscription'

param location string
param environment string
param applicationName string

var name = common.constructResourceGroupName(applicationName, environment, location)

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-07-01' = {
  name: name
  location: location
  tags: {
    environment: environment
    applicationName: applicationName
  }
}

module logAnalyticsWorkspace '../../../Common/src/Infrastructure/logAnalytics.bicep' = {
  name: 'logAnalytics'
  scope: resourceGroup
  params: {
    dailyQuotaGb: 1
  }
}

module databases 'databases/main.bicep' = {
  name: 'databases'
  scope: resourceGroup
}

module apps 'apps/main.bicep' = {
  name: 'apps'
  scope: resourceGroup
  params: {
    logAnalyticsWorkspaceResourceId: logAnalyticsWorkspace.outputs.logAnalyticsWorkspaceResourceId
    databaseConnectionString: databases.outputs.connectionString
  }
}

module development 'development/main.bicep' = if (environment == 'dev') {
  name: 'development'
  scope: resourceGroup
}
