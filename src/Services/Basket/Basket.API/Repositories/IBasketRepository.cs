using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository : IRepository<ShoppingCart, string>
    {
    }
}