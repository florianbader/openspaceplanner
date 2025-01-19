var environments = loadJsonContent('environments.json')
var locations = loadJsonContent('locations.json')

@export()
func constructResourceGroupName(applicationName string, environment string, location string) string =>
  'rg-${applicationName}-${environments[environment]}-${locations[location]}'

@export()
func constructResourceNameWithoutSuffix(
  resourceType string,
  applicationName string,
  environment string,
  location string,
  withDashes bool
) string =>
  withDashes
    ? '${resourceType}-${applicationName}-${environments[environment]}-${locations[location]}'
    : '${resourceType}${applicationName}${environments[environment]}${locations[location]}'

@export()
func constructResourceName(
  resourceType string,
  applicationName string,
  environment string,
  location string,
  resourceName string,
  withDashes bool
) string =>
  withDashes
    ? '${resourceType}-${applicationName}-${environments[environment]}-${locations[location]}-${resourceName}'
    : '${resourceType}${applicationName}${environments[environment]}${locations[location]}${resourceName}'

@export()
func constructResourceNameWithCounter(
  resourceType string,
  applicationName string,
  environment string,
  location string,
  resourceName string,
  counter int,
  withDashes bool
) string =>
  withDashes
    ? '${resourceType}-${applicationName}-${environments[environment]}-${locations[location]}-${resourceName}-${counter}'
    : '${resourceType}${applicationName}${environments[environment]}${locations[location]}${resourceName}${counter}'
