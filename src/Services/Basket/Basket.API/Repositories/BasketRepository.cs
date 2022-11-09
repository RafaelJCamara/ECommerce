using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        //private readonly IDistributedCache _redisCache;

        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public BasketRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<ShoppingCart> GetBasket(string username)
        {
            var redis = _connectionMultiplexer.GetDatabase();
            var basket = await redis.StringGetAsync(username);
            if (string.IsNullOrEmpty(basket)) return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            var redis = _connectionMultiplexer.GetDatabase();
            await redis.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string username)
        {
            var redis = _connectionMultiplexer.GetDatabase();
            await redis.KeyDeleteAsync(username);
        }
    }
}