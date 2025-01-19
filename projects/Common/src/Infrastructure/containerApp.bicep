import { constructResourceName } from '_common.bicep'

@description('Specifies the name of the application.')
param applicationName string = resourceGroup().tags.applicationName

@description('Specifies the environment name.')
param environment string = resourceGroup().tags.environment

@description('Specifies the location name.')
param location string = resourceGroup().location

@description('Specifies the resource suffix name.')
param resourceSuffixName string

@description('Specifies the container app environment ID.')
param containerAppEnvironmentId string

@description('Specifies the container registry login server.')
param containerRegistryLoginServer string

@description('Specifies the minimum number of replicas.')
@minValue(0)
@maxValue(25)
param minReplicas int = 0

@description('Specifies the maximum number of replicas.')
@minValue(0)
@maxValue(25)
param maxReplicas int = 1

@description('Specifies the environment variables of the container app.')
param env array = []

var name = constructResourceName('app', applicationName, environment, location, resourceSuffixName, true)

var acrPullRole = resourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')

resource userAssignedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-07-31-preview' = {
  name: 'id-${name}'
  location: location
}

@description('This allows the managed identity of the container app to access the registry. Scope is applied to the wider resource group not the container registry.')
resource userAssignedIdentityRoleBasedAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, userAssignedIdentity.id, acrPullRole)
  properties: {
    roleDefinitionId: acrPullRole
    principalId: userAssignedIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

// This uses Microsoft ASP.NET Core as the base image.
// This is a placeholder for the actual image that will be used and will be replaced by the CI/CD pipeline.

resource containerApp 'Microsoft.App/containerApps@2024-10-02-preview' = {
  name: name
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userAssignedIdentity.id}': {}
    }
  }
  properties: {
    managedEnvironmentId: containerAppEnvironmentId
    configuration: {
      activeRevisionsMode: 'multiple'
      maxInactiveRevisions: 1
      ingress: {
        external: true
        transport: 'http2'
        allowInsecure: false
        targetPort: 80
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
      }
      registries: [
        {
          server: containerRegistryLoginServer
          identity: userAssignedIdentity.id
        }
      ]
    }
    template: {
      containers: [
        {
          name: name
          image: 'mcr.microsoft.com/dotnet/samples:aspnetapp-9.0'
          env: [for item in env: { name: item.name, value: item.value }]
          resources: {
            cpu: 1
            memory: '2Gi'
          }
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: maxReplicas
        rules: [
          {
            name: 'http-requests'
            http: {
              metadata: {
                concurrentRequests: '10'
              }
            }
          }
        ]
      }
    }
  }
}

output appIdentityPrincipalId string = userAssignedIdentity.properties.principalId
