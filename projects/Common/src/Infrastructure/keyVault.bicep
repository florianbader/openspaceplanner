import { constructResourceName } from '_common.bicep'

@description('Specifies the name of the application.')
param applicationName string = resourceGroup().tags.applicationName

@description('Specifies the environment name.')
param environment string = resourceGroup().tags.environment

@description('Specifies the location name.')
param location string = resourceGroup().location

@description('Specifies the resource suffix name.')
param resourceSuffixName string

var name = constructResourceName('kv', applicationName, environment, location, resourceSuffixName, false)

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' = {
  name: name
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enablePurgeProtection: true
    enableSoftDelete: true
  }
}
