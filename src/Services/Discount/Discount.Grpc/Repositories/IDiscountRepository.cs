using System.Threading.Tasks;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon cupon);
        Task<bool> UpdateDiscount(Coupon cupon);
        Task<bool> DeleteDiscount(string productName);
    }
}