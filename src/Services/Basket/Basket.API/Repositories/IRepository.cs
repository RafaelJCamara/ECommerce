using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IRepository<T,K>
        where T : EntityBase
        where K : notnull
    {
        Task<T> GetById(K username);
        Task<T> Update(T basket);
        Task Delete(K username);
    }
}
