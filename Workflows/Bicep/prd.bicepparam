using './main.bicep'

// Virtual network
param virtualNetworkResourceGroupName = 'rg-weu-network-prd'
param virtualNetworkName = 'vnet-weu-prd-001'

// Key vault / Certificate
param certificateName = 'wa-weu-cost-models-api-prd-cert'
param keyVaultResourceGroupName = 'rg-weu-keyvault-prd'
param keyVaultName = 'kv-weu-buy-prd-002'

// Web app
param servicePlanName = 'sp-weu-cost-models-prd'
param servicePlanSkuName = 'B2'
param webAppName = 'wa-weu-cost-models-api-prd'
param webAppVirtualNetworkSubnetName = 'sn-weu-pri-cost-models-api-outbound-prd'
param webAppAppSettings = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: 'Production'
  }
]

// Web App / Hostname binding
param webAppHostNameBindingName = 'cost-models.whatstheprice.com'

// Web app / Private endpoint
param webAppPrivateEndpointName = 'pe-wa-weu-cost-models-api-prd'
param webAppPrivateEndpointSubnetName = 'sn-weu-pri-cost-models-api-inbound-prd'
param webAppPrivateEndpointPrivateLinkServiceConnectionName = 'psc-pe-wa-weu-cost-models-api-prd'

// Static site
param staticSiteName = 'ss-weu-cost-models-web-prd'
param staticSiteSkuName = 'Standard'
param staticSiteSkuTier = 'Standard'

// Statice site / Private endpoint
param staticSitePrivateEndpointName = 'pe-ss-weu-cost-models-web-prd'
param staticSitePrivateEndpointSubnetName = 'sn-weu-pri-cost-models-web-prd'
param staticSitePrivateEndpointPrivateLinkServiceConnectionName = 'psc-pe-ss-weu-cost-models-web-prd'
