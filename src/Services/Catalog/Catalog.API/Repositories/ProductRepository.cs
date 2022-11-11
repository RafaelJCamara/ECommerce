using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly ICatalogContext _catalogContext;

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
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            return await GetProductThatMatchCriteria(filter);
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await GetProductsThatMatchCriteria(filter);
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await GetProductsThatMatchCriteria(filter);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var filter = Builders<Product>.Filter.Exists("Id", true);
            return await GetProductsThatMatchCriteria(filter);
        }

        //for testing purposes (it contains an extension method): workaround
        protected async virtual Task<Product> GetProductThatMatchCriteria(FilterDefinition<Product> filter)
        {
            return await _catalogContext
                                      .Products
                                      .Find(filter)
                                      .FirstOrDefaultAsync();
        }

        //for testing purposes (it contains an extension method): workaround
        protected async virtual Task<IEnumerable<Product>> GetProductsThatMatchCriteria(FilterDefinition<Product> filter)
        {
            return await _catalogContext
                                      .Products
                                      .Find(filter)
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