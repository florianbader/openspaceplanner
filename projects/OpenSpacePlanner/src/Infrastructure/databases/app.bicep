module sqlServer '../../../../Common/src/Infrastructure/sqlServer.bicep' = {
  name: 'sqlServer'
  params: {
    resourceSuffixName: 'app'
    adminTenantId: subscription().tenantId
    skuName: 'Basic'
  }
}

output connectionString string = sqlServer.outputs.connectionString
