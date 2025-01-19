module containerRegistry '../../../../Common/src/Infrastructure/containerRegistry.bicep' = {
  name: 'developmentContainerRegistry'
  params: {
    resourceSuffixName: 'development'
  }
}
