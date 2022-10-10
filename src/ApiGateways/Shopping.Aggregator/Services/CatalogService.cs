using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _client;

    public CatalogService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<CatalogProductModel>> GetProducts()
    {
        var response = await _client.GetAsync("/api/v1/Catalog");
        return await response.ReadContentAs<List<CatalogProductModel>>();
    }

    public async Task<CatalogProductModel> GetProductById(string id)
    {
        var response = await _client.GetAsync($"/api/v1/Catalog/{id}");
        return await response.ReadContentAs<CatalogProductModel>();
    }

    public async Task<IEnumerable<CatalogProductModel>> GetProductByCategory(string category)
    {
        var response = await _client.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}");
        return await response.ReadContentAs<List<CatalogProductModel>>();
    }
}