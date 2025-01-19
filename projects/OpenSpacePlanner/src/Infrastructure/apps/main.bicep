@description('Specifies the database connection string.')
param databaseConnectionString string

@description('Specifies the Log Analytics workspace resource id.')
param logAnalyticsWorkspaceResourceId string

module containerRegistry '../../../../Common/src/Infrastructure/containerRegistry.bicep' = {
  name: 'appsContainerRegistry'
  params: {
    resourceSuffixName: 'apps'
  }
}

module containerAppsEnvironment '../../../../Common/src/Infrastructure/containerAppEnvironment.bicep' = {
  name: 'appsContainerAppEnvironment'
  params: {
    resourceSuffixName: 'apps'
    logAnalyticsWorkspaceResourceId: logAnalyticsWorkspaceResourceId
  }
}

module frontendContainerApp './frontend.bicep' = {
  name: 'appsFrontend'
  params: {
    containerAppEnvironmentId: containerAppsEnvironment.outputs.containerAppEnvironmentId
    containerRegistryLoginServer: containerRegistry.outputs.containerRegistryLoginServer
    logAnalyticsWorkspaceResourceId: logAnalyticsWorkspaceResourceId
  }
}

module apiContainerApp './backend.bicep' = {
  name: 'appsBackend'
  params: {
    containerAppEnvironmentId: containerAppsEnvironment.outputs.containerAppEnvironmentId
    containerRegistryLoginServer: containerRegistry.outputs.containerRegistryLoginServer
    logAnalyticsWorkspaceResourceId: logAnalyticsWorkspaceResourceId
    databaseConnectionString: databaseConnectionString
  }
}
