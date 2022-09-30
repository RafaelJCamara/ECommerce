using Shopping.Aggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogProductModel>> GetProducts();
        Task<CatalogProductModel> GetProductById(string id);
        Task<IEnumerable<CatalogProductModel>> GetProductByCategory(string category);
    }
}
