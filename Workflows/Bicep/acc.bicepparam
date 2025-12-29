using './main.bicep'

// Virtual network
param virtualNetworkResourceGroupName = 'rg-weu-network-acc'
param virtualNetworkName = 'vnet-weu-acc-001'

// Key vault / Certificate
param certificateName = 'wa-weu-cost-models-api-acc-cert'
param keyVaultResourceGroupName = 'rg-weu-keyvault-acc'
param keyVaultName = 'kv-weu-buy-acc-002'

// Web app
param servicePlanName = 'sp-weu-cost-models-acc'
param servicePlanSkuName = 'B1'
param webAppName = 'wa-weu-cost-models-api-acc'
param webAppVirtualNetworkSubnetName = 'sn-weu-pri-cost-models-api-outbound-acc'
param webAppAppSettings = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: 'Acceptance'
  }
]

// Web App / Hostname binding
param webAppHostNameBindingName = 'cost-models-acc.whatstheprice.com'

// Web app / Private endpoint
param webAppPrivateEndpointName = 'pe-wa-weu-cost-models-api-acc'
param webAppPrivateEndpointSubnetName = 'sn-weu-pri-cost-models-api-inbound-acc'
param webAppPrivateEndpointPrivateLinkServiceConnectionName = 'psc-pe-wa-weu-cost-models-api-acc'

// Static site
param staticSiteName = 'ss-weu-cost-models-web-acc'
param staticSiteSkuName = 'Standard'
param staticSiteSkuTier = 'Standard'

// Statice site / Private endpoint
param staticSitePrivateEndpointName = 'pe-ss-weu-cost-models-web-acc'
param staticSitePrivateEndpointSubnetName = 'sn-weu-pri-cost-models-web-acc'
param staticSitePrivateEndpointPrivateLinkServiceConnectionName = 'psc-pe-ss-weu-cost-models-web-acc'
