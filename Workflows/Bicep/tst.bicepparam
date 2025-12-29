using './main.bicep'

// Virtual network
param virtualNetworkResourceGroupName = 'rg-weu-network-tst'
param virtualNetworkName = 'vnet-weu-tst-001'

// Key vault / Certificate
param certificateName = 'wa-weu-cost-models-api-tst-cert'
param keyVaultResourceGroupName = 'rg-weu-keyvault-tst'
param keyVaultName = 'kv-weu-buy-tst-002'

// Web app
param servicePlanName = 'sp-weu-cost-models-tst'
param servicePlanSkuName = 'B1'
param webAppName = 'wa-weu-cost-models-api-tst'
param webAppVirtualNetworkSubnetName = 'sn-weu-pri-cost-models-api-outbound-tst'
param webAppAppSettings = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: 'Test'
  }
]

// Web App / Hostname binding
param webAppHostNameBindingName = 'cost-models-tst.whatstheprice.com'

// Web app / Private endpoint
param webAppPrivateEndpointName = 'pe-wa-weu-cost-models-api-tst'
param webAppPrivateEndpointSubnetName = 'sn-weu-pri-cost-models-api-inbound-tst'
param webAppPrivateEndpointPrivateLinkServiceConnectionName = 'psc-pe-wa-weu-cost-models-api-tst'

// Static site
param staticSiteName = 'ss-weu-cost-models-web-tst'
param staticSiteSkuName = 'Standard'
param staticSiteSkuTier = 'Standard'

// Statice site / Private endpoint
param staticSitePrivateEndpointName = 'pe-ss-weu-cost-models-web-tst'
param staticSitePrivateEndpointSubnetName = 'sn-weu-pri-cost-models-web-tst'
param staticSitePrivateEndpointPrivateLinkServiceConnectionName = 'psc-pe-ss-weu-cost-models-web-tst'
