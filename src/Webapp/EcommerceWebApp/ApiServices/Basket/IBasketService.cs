using EcommerceWebApp.Models;
using System.Threading.Tasks;

namespace EcommerceWebApp.ApiServices.Basket
{
    public interface IBasketService
    {
        Task<ShoppingCart> GetBasketByUsername(string username);
    }
}
