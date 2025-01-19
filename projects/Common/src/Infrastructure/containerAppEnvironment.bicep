import { constructResourceName } from '_common.bicep'

@description('Specifies the name of the application.')
param applicationName string = resourceGroup().tags.applicationName

@description('Specifies the environment name.')
param environment string = resourceGroup().tags.environment

@description('Specifies the location name.')
param location string = resourceGroup().location

@description('Specifies the resource suffix name.')
param resourceSuffixName string

@description('Specifies the Log Analytics workspace resource id.')
param logAnalyticsWorkspaceResourceId string

var name = constructResourceName('cae', applicationName, environment, location, resourceSuffixName, true)

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2024-10-02-preview' = {
  name: name
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: reference(logAnalyticsWorkspaceResourceId, '2023-09-01').customerId
        sharedKey: listKeys(logAnalyticsWorkspaceResourceId, '2023-09-01').primarySharedKey
      }
    }
  }
}

output containerAppEnvironmentId string = containerAppEnvironment.id
