using Discount.API.Entities;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public interface IRepository<T, K>
        where T : EntityBase
        where K : notnull
    {
        Task<T> Get(K productName);
        Task<bool> Create(T cupon);
        Task<bool> Update(T cupon);
        Task<bool> Delete(K productName);
    }
}
