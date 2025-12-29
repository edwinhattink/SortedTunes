@description('This specifies the Azure region where that resource will be deployed.')
param location string = resourceGroup().location

// Virtual network
param virtualNetworkResourceGroupName string
param virtualNetworkName string

// Certificate
param certificateName string
param keyVaultResourceGroupName string
param keyVaultName string

// Web app
param servicePlanName string
param servicePlanSkuName string
param webAppName string
param webAppVirtualNetworkSubnetName string
param webAppAppSettings array

// Web app / Private endpoint
param webAppPrivateEndpointName string
param webAppPrivateEndpointSubnetName string
param webAppPrivateEndpointPrivateLinkServiceConnectionName string

// Web app / Host name bindings
param webAppHostNameBindingName string

// Static site
param staticSiteName string
param staticSiteSkuName string
param staticSiteSkuTier string

// Statice site / Private endpoint
param staticSitePrivateEndpointName string
param staticSitePrivateEndpointSubnetName string
param staticSitePrivateEndpointPrivateLinkServiceConnectionName string

// TODO: Convert to single existing certificate resource which can be used for all web apps.
resource certificate 'Microsoft.Web/certificates@2024-04-01' = {
  name: certificateName
  location: location
  properties: {
    keyVaultId: resourceId(keyVaultResourceGroupName, 'Microsoft.KeyVault/vaults', keyVaultName)
    keyVaultSecretName: 'wildcard-certificate-whatstheprice-com'
  }
}

resource servicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: servicePlanName
  location: location
  sku: {
    name: servicePlanSkuName
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2024-04-01' = {
  name: webAppName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: servicePlan.id
    virtualNetworkSubnetId: resourceId(
      virtualNetworkResourceGroupName,
      'Microsoft.Network/virtualNetworks/subnets',
      virtualNetworkName,
      webAppVirtualNetworkSubnetName
    )
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|9.0'
      appSettings: webAppAppSettings
      appCommandLine: 'dotnet SortedTunes.Web.dll'
      cors: {
        allowedOrigins: [
          'https://*.whatstheprice.com'
        ]
        supportCredentials: false
      }
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource webAppPrivateEndpoint 'Microsoft.Network/privateEndpoints@2024-05-01' = {
  name: webAppPrivateEndpointName
  location: location
  properties: {
    subnet: {
      id: resourceId(
        virtualNetworkResourceGroupName,
        'Microsoft.Network/virtualNetworks/subnets',
        virtualNetworkName,
        webAppPrivateEndpointSubnetName
      )
    }
    privateLinkServiceConnections: [
      {
        name: webAppPrivateEndpointPrivateLinkServiceConnectionName
        properties: {
          privateLinkServiceId: webApp.id
          groupIds: [
            'sites'
          ]
        }
      }
    ]
  }
}

resource webAppHostNameBinding 'Microsoft.Web/sites/hostNameBindings@2024-04-01' = {
  parent: webApp
  name: webAppHostNameBindingName
  properties: {
    siteName: webApp.name
    hostNameType: 'Verified'
    thumbprint: certificate.properties.thumbprint
    sslState: 'SniEnabled'
  }
}

resource webAppPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' existing = {
  scope: resourceGroup('7396e1d6-8478-494b-a258-4d2839bfc861', 'rg-weu-privatedns-shr') // buy-shr-sub
  name: 'privatelink.azurewebsites.net'
}

resource webAppPrivateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2024-05-01' = {
  parent: webAppPrivateEndpoint
  name: '${webAppPrivateEndpoint.name}-default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: '${webAppPrivateEndpoint.name}-config'
        properties: {
          privateDnsZoneId: webAppPrivateDnsZone.id
        }
      }
    ]
  }
}

resource staticSite 'Microsoft.Web/staticSites@2024-04-01' = {
  name: staticSiteName
  location: location
  sku: {
    name: staticSiteSkuName
    tier: staticSiteSkuTier
  }
  properties: {}
  identity: {
    type: 'SystemAssigned'
  }
}

resource staticSitePrivateEndpoint 'Microsoft.Network/privateEndpoints@2024-05-01' = {
  name: staticSitePrivateEndpointName
  location: location
  properties: {
    subnet: {
      id: resourceId(
        virtualNetworkResourceGroupName,
        'Microsoft.Network/virtualNetworks/subnets',
        virtualNetworkName,
        staticSitePrivateEndpointSubnetName
      )
    }
    privateLinkServiceConnections: [
      {
        name: staticSitePrivateEndpointPrivateLinkServiceConnectionName
        properties: {
          privateLinkServiceId: staticSite.id
          groupIds: [
            'staticSites'
          ]
        }
      }
    ]
  }
}

resource staticSitePrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' existing = {
  scope: resourceGroup('7396e1d6-8478-494b-a258-4d2839bfc861', 'rg-weu-privatedns-shr') // buy-shr-sub
  name: 'privatelink.azurestaticapps.net'
}

resource staticSitePrivateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2024-05-01' = {
  parent: staticSitePrivateEndpoint
  name: '${staticSitePrivateEndpoint.name}-default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: '${staticSitePrivateEndpoint.name}-config'
        properties: {
          privateDnsZoneId: staticSitePrivateDnsZone.id
        }
      }
    ]
  }
}
