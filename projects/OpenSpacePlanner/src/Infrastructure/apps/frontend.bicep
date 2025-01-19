@description('Specifies the name of the container registry login server.')
param containerRegistryLoginServer string

@description('Specifies the container app environment id.')
param containerAppEnvironmentId string

@description('Specifies the Log Analytics workspace resource id.')
param logAnalyticsWorkspaceResourceId string

var resourceSuffix = 'frontend'

module containerApp '../../../../Common/src/Infrastructure/containerApp.bicep' = {
  name: 'frontendContainerApp'
  params: {
    containerAppEnvironmentId: containerAppEnvironmentId
    containerRegistryLoginServer: containerRegistryLoginServer
    resourceSuffixName: resourceSuffix
  }
}

module appInsights '../../../../Common/src/Infrastructure/appInsights.bicep' = {
  name: 'frontendAppInsights'
  params: {
    logAnalyticsWorkspaceResourceId: logAnalyticsWorkspaceResourceId
    resourceSuffixName: resourceSuffix
  }
}
