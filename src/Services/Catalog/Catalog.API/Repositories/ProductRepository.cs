using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var deletedResult = await _catalogContext
                .Products
                .DeleteOneAsync(filter);
            return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _catalogContext
                .Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _catalogContext
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _catalogContext
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext
                .Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedResult = await _catalogContext
                .Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);
            return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
        }
    }
}