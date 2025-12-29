using System.Net.Http.Json;
using Buynamics.Toolkit.AcceptanceTests.Exceptions;

namespace SortedTunes.Web.AcceptanceTests.Clients;

public class ProductHttpClient(HttpClientCreator httpClient)
{
    public static readonly string Audience = "buynamics-wtp-cost-models-api";

    public async Task<int> CreateProduct(string productName, int? regionId = null, int? industryId = null)
    {
        var client = await httpClient.GetClient(Audience);
        var productIdResponse = await client.PostAsJsonAsync(
            "Products",
            new
            {
                name = productName,
                regionId = regionId ?? null,
                industryId = industryId ?? null,
                tags = Array.Empty<string>(),
                metadata = new { }
            }
        );
        return productIdResponse.IsSuccessStatusCode
            ? int.Parse(await productIdResponse.Content.ReadAsStringAsync())
            : throw new ApiException(productIdResponse.StatusCode, await productIdResponse.Content.ReadAsStringAsync());
    }

    public async Task<int> CreateHistoricalPrices(int productId)
    {
        var client = await httpClient.GetClient(Audience);

        var productPurchasingPriceResponse = await client.PostAsJsonAsync(
           $"Products/{productId}/PurchasingPrices",
           new
           {
               productId,
               price = 2.00,
               Date = "2024-01-10",
               Quantity = 1
           }
       );

        return productPurchasingPriceResponse.IsSuccessStatusCode
        ? int.Parse(await productPurchasingPriceResponse.Content.ReadAsStringAsync())
        : throw new ApiException(productPurchasingPriceResponse.StatusCode, await productPurchasingPriceResponse.Content.ReadAsStringAsync());
    }

    public async Task<int> CreateProductCommodity(int productId, int commodityRegionId, int quantity)
    {
        var client = await httpClient.GetClient(Audience);
        var response = await client.PostAsJsonAsync(
            $"Products/{productId}/Commodities",
            new
            {
                productId,
                commodityRegionId,
                quantity,
                changeByAmount = 0,
                changeByPercentage = 0,
                waste = 0,
                weightUnit = "KG"
            }
        );
        return response.IsSuccessStatusCode
            ? int.Parse(await response.Content.ReadAsStringAsync())
            : throw new ApiException(response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    public async Task MarkAsComplete(int productId)
    {
        var client = await httpClient.GetClient(Audience);
        var response = await client.PostAsync($"Products/{productId}/MarkAsComplete", null);
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }

    public async Task DeleteProduct(int productId)
    {
        var client = await httpClient.GetClient(Audience);
        var response = await client.DeleteAsync($"Products/{productId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
