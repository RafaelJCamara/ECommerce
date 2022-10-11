using System.Collections.Generic;
using System.Threading.Tasks;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogProductModel>> GetProducts();
        Task<CatalogProductModel> GetProductById(string id);
        Task<IEnumerable<CatalogProductModel>> GetProductByCategory(string category);
    }
}