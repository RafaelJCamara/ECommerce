using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static MongoDB.Driver.DeleteResult;

namespace CatalogAPI.UnitTests
{
    public  class CatalogTests
    {

        [Fact]
        public async Task Adds_Product_To_Database()
        {
            //Arrange
            var productToAdd = new Product { Id = "2", Name = "ProductTwo" };
            
            var context = new Mock<ICatalogContext>();
            var mongoCollection = new Mock<IMongoCollection<Product>>();
            mongoCollection.Setup(x => x.InsertOneAsync(It.IsAny<Product>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(productToAdd));
            context.SetupGet(x => x.Products).Returns(mongoCollection.Object);

            //Act
            var productRepository = new ProductRepository(context.Object);
            await productRepository.CreateProduct(productToAdd);

            //Assert
            mongoCollection.Verify(x => x.InsertOneAsync(It.Is<Product>(y => y==productToAdd), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()));
        }

        //dosn't work because Find is a static method
        [Fact]
        public async Task Get_Product_By_Id()
        {
            //Arrange
            var product = new Product { Id = "2", Name = "ProductTwo" };

            var context = new Mock<ICatalogContext>();
            var mongoCollection = new Mock<IMongoCollection<Product>>();
            mongoCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()))
                .Returns((IFindFluent<Product, Product>)Task.FromResult(It.IsAny<Product>()));
            context.SetupGet(x => x.Products).Returns(mongoCollection.Object);

            //Act
            var productRepository = new TestProductRepository(context.Object);
            await productRepository.GetProduct(product.Id);

            //Assert
            mongoCollection.Verify(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()));
        }

        //dosn't work because Find is a static method
        [Fact]
        public async Task Get_Product_By_Name()
        {
            //Arrange
            var product = new Product { Id = "2", Name = "ProductTwo" };

            var context = new Mock<ICatalogContext>();
            var mongoCollection = new Mock<IMongoCollection<Product>>();
            mongoCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()))
                .Returns((IFindFluent<Product, Product>)Task.FromResult(It.IsAny<Product>()));
            context.SetupGet(x => x.Products).Returns(mongoCollection.Object);

            //Act
            var productRepository = new ProductRepository(context.Object);
            await productRepository.GetProductByName(product.Name);

            //Assert
            mongoCollection.Verify(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()));
        }

        //dosn't work because Find is a static method
        [Fact]
        public async Task Get_Product_By_Category()
        {
            //Arrange
            var product = new Product { Id = "2", Name = "ProductTwo" };

            var context = new Mock<ICatalogContext>();
            var mongoCollection = new Mock<IMongoCollection<Product>>();
            mongoCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()))
                .Returns((IFindFluent<Product, Product>)Task.FromResult(It.IsAny<Product>()));
            context.SetupGet(x => x.Products).Returns(mongoCollection.Object);

            //Act
            var productRepository = new ProductRepository(context.Object);
            await productRepository.GetProductByCategory(product.Category);

            //Assert
            mongoCollection.Verify(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()));
        }

        //dosn't work because Find is a static method
        [Fact]
        public async Task Get_All_Products()
        {
            //Arrange
            var product = new Product { Id = "2", Name = "ProductTwo" };

            var context = new Mock<ICatalogContext>();
            var mongoCollection = new Mock<IMongoCollection<Product>>();
            mongoCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()))
                .Returns((IFindFluent<Product, Product>)Task.FromResult(It.IsAny<Product>()));
            context.SetupGet(x => x.Products).Returns(mongoCollection.Object);

            //Act
            var productRepository = new ProductRepository(context.Object);
            await productRepository.GetProducts();

            //Assert
            mongoCollection.Verify(x => x.Find(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions>()));
        }

        [Fact]
        public async Task Delete_Product_By_Id()
        {
            //Arrange
            var product = new Product { Id = "2", Name = "ProductTwo" };

            var context = new Mock<ICatalogContext>();
            var mongoCollection = new Mock<IMongoCollection<Product>>();
            mongoCollection.Setup(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<DeleteOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(It.IsAny<DeleteResult>()));
            context.SetupGet(x => x.Products).Returns(mongoCollection.Object);

            //Act
            var productRepository = new ProductRepository(context.Object);
            await productRepository.DeleteProduct(product.Id);

            //Assert
            mongoCollection.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<DeleteOptions>(), It.IsAny<CancellationToken>()));
        }

        //dosn't work because Find is a static method
        [Fact]
        public async Task Update_Product()
        {
            //Arrange
            var product = new Product { Id = "2", Name = "ProductTwo" };

            var context = new Mock<ICatalogContext>();
            var mongoCollection = new Mock<IMongoCollection<Product>>();
            mongoCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Product>>(), product, It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(It.IsAny<ReplaceOneResult>()));
            context.SetupGet(x => x.Products).Returns(mongoCollection.Object);

            //Act
            var productRepository = new ProductRepository(context.Object);
            await productRepository.UpdateProduct(product);

            //Assert
            mongoCollection.Verify(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Product>>(), product, It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()));
        }
    }
}
