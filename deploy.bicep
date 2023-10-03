param appName string = resourceGroup().name

param location string = resourceGroup().location

var resourceSuffix = appName

var networkName = 'vnet-${resourceSuffix}'
var keyVaultName = 'kv-${resourceSuffix}'
var subnetName = 'internal'
var storageAccount = 'storageaccount-${resourceSuffix}-${uniqueString(resourceGroup().id)}'
var dbAccountName = 'cosmos-nosql-${resourceSuffix}'

resource virtualNetwork 'Microsoft.Network/virtualNetworks@2019-11-01' = {
  name: networkName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    subnets: [
      {
        name: subnetName
        properties: {
          addressPrefix: '10.0.0.0/24'
          serviceEndpoints: [
            {
              service: 'Microsoft.AzureCosmosDB'
              locations: ['*']
            }
            {
              service: 'Microsoft.KeyVault'
              locations: ['*']
            }
          ]
        }
      }
    ]
  }
}

resource dbAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: dbAccountName
  location: location
  kind: 'GlobalDocumentDB'
  dependsOn: [
    virtualNetwork
  ]
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
    backupPolicy: {
      type: 'Continuous'
      continuousModeProperties: {
        tier: 'Continuous7Days'
      }
    }
    isVirtualNetworkFilterEnabled: true
    minimalTlsVersion: 'Tls12'
    enableMultipleWriteLocations: false
    enableFreeTier: true
    capacity: {
      totalThroughputLimit: 1000
    }
    publicNetworkAccess: 'Enabled'
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  name: appName
  parent: dbAccount
  properties: {
    resource: {
      id: appName
    }
    options: {
      throughput: 1000
    }
  }

  resource accountsContainer 'containers' = {
    name: 'accounts'
    properties: {
      resource: {
        id: 'accounts'
        partitionKey: {
          paths: [ '/userId' ]
          kind: 'Hash'
          version: 2
        }
      }
    }
  }

  resource messagesContainer 'containers' = {
    name: 'messages'
    properties: {
      resource: {
        id: 'messages'
        partitionKey: {
          paths: [ '/accountId' ]
          kind: 'Hash'
          version: 2
        }
      }
    }
  }
}

resource vault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVaultName
  location: location
  dependsOn: [
    virtualNetwork
  ]
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    enableRbacAuthorization: true
    tenantId: subscription().tenantId
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'AzureServices'
      virtualNetworkRules: [
        {
          id: resourceId('Microsoft.Network/VirtualNetworks/subnets', networkName, subnetName)
          ignoreMissingVnetServiceEndpoint: false
        }
      ]
    }
  }
}

resource storageaccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccount
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

