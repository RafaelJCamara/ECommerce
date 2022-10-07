using EcommerceWebApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace EcommerceWebApp.ApiServices.Basket
{
    public class BasketService : IBasketService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public BasketService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ShoppingCart> GetBasketByUsername(string username)
        {
            var httpClient = _httpClientFactory.CreateClient("APIGatewayClient");

            var request = new HttpRequestMessage(HttpMethod.Get, $"/Basket/{username}");

            var response = await httpClient.SendAsync(
                                                        request,
                                                        HttpCompletionOption.ResponseHeadersRead
                                                    )
                                                    .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var basket = JsonConvert.DeserializeObject<ShoppingCart>(content);
            return basket;
        }
    }
}
