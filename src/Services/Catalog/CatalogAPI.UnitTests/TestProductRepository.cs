using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogAPI.UnitTests
{
    public class TestProductRepository : ProductRepository
    {
        public TestProductRepository(ICatalogContext catalogContext) : base(catalogContext)
        {

        }

        protected override async Task<Product> GetProductThatMatchCriteria(FilterDefinition<Product> filter)
        {
            return await base.GetProductThatMatchCriteria(filter);
        }

        protected override async Task<IEnumerable<Product>> GetProductsThatMatchCriteria(FilterDefinition<Product> filter)
        {
            return await base.GetProductsThatMatchCriteria(filter);
        }

    }
}
