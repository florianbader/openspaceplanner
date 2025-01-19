import { constructResourceName } from '_common.bicep'

@description('Specifies the name of the application.')
param applicationName string = resourceGroup().tags.applicationName

@description('Specifies the environment name.')
param environment string = resourceGroup().tags.environment

@description('Specifies the location name.')
param location string = resourceGroup().location

@description('Specifies the resource suffix name.')
param resourceSuffixName string

@description('Specifies the administrator login name.')
param adminLoginName string = 'admindb'

@description('Specifies the administrator SID.')
param adminSid string?

@description('Specifies the administrator tenant ID.')
param adminTenantId string = subscription().tenantId

@description('Specifies the principal type.')
param principalType string = 'User'

@description('Specifies whether SQL Defender should be enabled.')
param enableSqlDefender bool = false

@description('Specifies the SKU name for the database.')
param skuName string = 'Basic'

var serverName = constructResourceName('sql', applicationName, environment, location, resourceSuffixName, true)

resource userAssignedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-07-31-preview' = if (adminSid == null || adminSid == '') {
  name: 'id-${serverName}'
  location: location
}

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: serverName
  location: location
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      principalType: principalType
      login: adminLoginName
      sid: adminSid == null || adminSid == '' ? userAssignedIdentity.properties.principalId : adminSid
      tenantId: adminTenantId
    }
    minimalTlsVersion: '1.2'
    version: '12.0'
  }
}

var databaseName = constructResourceName('db', applicationName, environment, location, resourceSuffixName, true)

resource sqlDatabase 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  name: databaseName
  location: location
  parent: sqlServer
  sku: {
    name: skuName
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 1073741824
  }
}

output connectionString string = 'Server=tcp:${serverName}${az.environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlDatabase.name};Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

resource allowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2021-02-01-preview' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}

resource protectionSetting 'Microsoft.Sql/servers/advancedThreatProtectionSettings@2022-05-01-preview' = if (enableSqlDefender) {
  parent: sqlServer
  name: 'Default'
  properties: {
    state: 'Enabled'
  }
}

resource assessment 'Microsoft.Sql/servers/sqlVulnerabilityAssessments@2022-05-01-preview' = if (enableSqlDefender) {
  parent: sqlServer
  name: 'Default'
  properties: {
    state: 'Enabled'
  }
}

resource connectionPolicy 'Microsoft.Sql/servers/connectionPolicies@2022-05-01-preview' = {
  parent: sqlServer
  name: 'Default'
  properties: {
    connectionType: 'Default'
  }
}

output sqlServerName string = sqlServer.name
