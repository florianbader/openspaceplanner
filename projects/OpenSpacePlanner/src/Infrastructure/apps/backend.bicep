@description('Specifies the database connection string.')
param databaseConnectionString string

@description('Specifies the name of the container registry login server.')
param containerRegistryLoginServer string

@description('Specifies the container app environment id.')
param containerAppEnvironmentId string

@description('Specifies the Log Analytics workspace resource id.')
param logAnalyticsWorkspaceResourceId string

var resourceSuffix = 'api'

module containerApp '../../../../Common/src/Infrastructure/containerApp.bicep' = {
  name: 'backendContainerApp'
  params: {
    containerAppEnvironmentId: containerAppEnvironmentId
    containerRegistryLoginServer: containerRegistryLoginServer
    resourceSuffixName: resourceSuffix
    env: [
      { name: 'DatabaseConnectionString', value: databaseConnectionString }
    ]
  }
}

module appInsights '../../../../Common/src/Infrastructure/appInsights.bicep' = {
  name: 'backendAppInsights'
  params: {
    logAnalyticsWorkspaceResourceId: logAnalyticsWorkspaceResourceId
    resourceSuffixName: resourceSuffix
  }
}
