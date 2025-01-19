import { constructResourceNameWithoutSuffix } from '_common.bicep'

@description('Specifies the name of the application.')
param applicationName string = resourceGroup().tags.applicationName

@description('Specifies the environment name.')
param environment string = resourceGroup().tags.environment

@description('Specifies the location name.')
param location string = resourceGroup().location

@description('Specifies the daily quota in GB.')
@minValue(0)
@maxValue(100)
param dailyQuotaGb int = 1

var name = constructResourceNameWithoutSuffix('log', applicationName, environment, location, true)

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: name
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    features: {
      workspaceCapping: {
        dailyQuotaGb: dailyQuotaGb
      }
      disableLocalAuth: true
      enableDataExport: true
      immediatePurgeDataOn30Days: false
    }
  }
}

output logAnalyticsWorkspaceResourceId string = logAnalytics.id
