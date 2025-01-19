import { constructResourceName } from '_common.bicep'

@description('Specifies the name of the application.')
param applicationName string = resourceGroup().tags.applicationName

@description('Specifies the environment name.')
param environment string = resourceGroup().tags.environment

@description('Specifies the location name.')
param location string = resourceGroup().location

@description('Specifies the resource suffix name.')
param resourceSuffixName string

@description('Specifies the SKU name.')
param skuName string = 'Basic'

var name = constructResourceName('cr', applicationName, environment, location, resourceSuffixName, false)

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-11-01-preview' = {
  name: name
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  sku: {
    name: skuName
  }
  properties: {
    adminUserEnabled: true
  }
}

output containerRegistryName string = containerRegistry.name
output containerRegistryLoginServer string = containerRegistry.properties.loginServer
