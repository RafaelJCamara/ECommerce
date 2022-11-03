using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            //var mongoClient = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(configuration.GetValue<string>("DatabaseSettings:ConnectionString")));
            clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
            var mongoClient = new MongoClient(clientSettings);

            /*
                Retrieves the specified database.
                If such database does not exists, it will create one
             */
            var catalogDatabase = mongoClient
                .GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = catalogDatabase.GetCollection<Product>(
                configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}