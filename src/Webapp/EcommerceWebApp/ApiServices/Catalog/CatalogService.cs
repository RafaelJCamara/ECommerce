using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EcommerceWebApp.ApiServices.Catalog
{
    public class CatalogService : ICatalogService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Models.Catalog>> GetCatalogProducts()
        {

            var httpClient = _httpClientFactory.CreateClient("APIGatewayClientNoAuth");

            var request = new HttpRequestMessage(HttpMethod.Get, "/Catalog");

            var response = await httpClient.SendAsync(
                                                        request,
                                                        HttpCompletionOption.ResponseHeadersRead
                                                    )
                                                    .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var catalog = JsonConvert.DeserializeObject<List<Models.Catalog>>(content);
            return catalog;
        }
    }
}
