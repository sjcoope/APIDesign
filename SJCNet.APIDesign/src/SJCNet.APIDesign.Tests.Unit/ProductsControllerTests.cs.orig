using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SJCNet.APIDesign.API.Controllers;
using SJCNet.APIDesign.Model;
using SJCNet.APIDesign.Tests.Unit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace SJCNet.APIDesign.Tests.Unit
{
    public class ProductsControllerTests
    {
        private List<Product> _data;

        public ProductsControllerTests()
        {
            _data = new List<Product>
            {
                new Product { Id = 1, Name = "Cricket Bat", Price = 22.99m },
                new Product { Id = 2, Name = "Cap", Price = 9.99m },
                new Product { Id = 3, Name = "Ball", Price = 15.00m },
                new Product { Id = 4, Name = "Gloves", Price = 29.99m },
                new Product { Id = 5, Name = "Suncream (not needed in UK)", Price = 6.50m },
            };
        }

        #region Helper Methods

        private void ValidateProduct(Product product, string messagePrefix)
        {
            Assert.True(product != null, $"{messagePrefix}: object is null");
            Assert.True(product.Id != 0, $"{messagePrefix}: Id is not set");
            Assert.True(!String.IsNullOrEmpty(product.Name), $"{messagePrefix}: Name is null or empty");
            Assert.True(product.Price != 0m, $"{messagePrefix}: Price is not set");
        }

        #endregion

        [Fact]
        public void Can_Get_Products_With_Success()
        {
            // Arrange - Create mock repository
            var repository = new MockRepository<Product>(_data);

            // Arrange - Create controller
            var controller = new ProductsController(repository);

            // Act
            var actual = controller.Get() as ObjectResult;

            // Assert - Verify return value
            Assert.NotNull(actual);

            // Assert - Check for Http Status Code
            Assert.True(actual.StatusCode != null, "Reponse status code is null");
            Assert.True(actual.StatusCode == (int)HttpStatusCode.OK, "Response status code is not 200 (OK)");

            //Assert - Test response object
            Assert.True(actual != null, "Response object is null");
            Assert.True(actual.Value != null, "Response value is null");

            // Assert - Test type of response data
            var list = actual.Value as IEnumerable<Product>;
            Assert.True(list != null, "response data is not of type IEnumberable<Product>");

            // Assert - Test first object in response data.
            var first = list.First();
            ValidateProduct(first, "First Object");

            // Assert - Test last object in response data
            var last = list.Last();
            ValidateProduct(last, "Last Object");
        }

        [Fact]
        public void Can_Get_Product_With_Success()
        {
            // Arrange - Get expected product
            var expected = _data[2];

            // Arrange - Create mock repository
            var repository = new MockRepository<Product>(_data);
            
            // Arrange - Create controller
            var controller = new ProductsController(repository);

            // Act
            var actualResult = controller.Get(expected.Id) as ObjectResult;

            // Assert - Verify return value
            Assert.NotNull(actualResult);

            // Assert - Check for Http Status Code
            Assert.True(actualResult.StatusCode != null, "Reponse status code is null");
            Assert.True(actualResult.StatusCode == (int)HttpStatusCode.OK, "Response status code is not 200 (OK)");

            //Assert - Test response object
            Assert.True(actualResult != null, "Response object is null");
            Assert.True(actualResult.Value != null, "Response value is null");

            // Assert - Test type of response data
            var actual = actualResult.Value as Product;
            Assert.True(actual != null, "response data is not of type Product");

            // Assert - Test product.
            Assert.Equal<Product>(expected, actual);
        }

        public void Can_Add_Product_With_Success()
        {
            // Arrange - Create mock repository
            var repository = new MockRepository<Product>(_data);

            // Arrange - Create controller
            var controller = new ProductsController(repository);

            // Arrange - Set expected count
            var expectedCount = _data.Count + 1;

            // Arrange - Create new product
            var expected = new Product
            {
                Id = 10,
                Name = "New Product",
                Price = 19.99m
            };

            // Act
            var actualResult = controller.Post(expected) as ObjectResult;

            // Assert - Verify return value
            Assert.NotNull(actualResult);

            // Assert - Check for Http Status Code
            Assert.True(actualResult.StatusCode != null, "Reponse status code is null");
            Assert.True(actualResult.StatusCode == (int)HttpStatusCode.Created, "Response status code is not 201 (Created)");

            //Assert - Test response object
            Assert.True(actualResult != null, "Response object is null");
            Assert.True(actualResult.Value != null, "Response value is null");

            // Assert - Test type of response data
            var actual = actualResult.Value as Product;
            Assert.True(actual != null, "response data is not of type Product");

            // Assert - Test returned entity.
            Assert.Equal<Product>(expected, actual);

            // Assert - Check collection increated
            var actualCount = repository.Get().Count();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void Can_Update_Product_With_Success()
        {
            // Arrange - Create mock repository
            var repository = new MockRepository<Product>(_data);

            // Arrange - Create controller
            var controller = new ProductsController(repository);

            // Arrange - Set expected count
            var expectedCount = _data.Count + 1;

            // Arrange - Get product and update it
            var expected = _data[2];
            expected.Name = "Updated";
            expected.Price = 1.99m;
            
            // Act
            var actualResult = controller.Put(expected.Id, expected) as ObjectResult;

            // Assert - Verify return value
            Assert.NotNull(actualResult);

            // Assert - Check for Http Status Code
            Assert.True(actualResult.StatusCode != null, "Reponse status code is null");
            Assert.True(actualResult.StatusCode == (int)HttpStatusCode.OK, "Response status code is not 200 (Ok)");

            //Assert - Test response object
            Assert.True(actualResult != null, "Response object is null");
            Assert.True(actualResult.Value != null, "Response value is null");

            // Assert - Test type of response data
            var actual = actualResult.Value as Product;
            Assert.True(actual != null, "response data is not of type Product");

            // Assert - Test returned entity.
            Assert.Equal<Product>(expected, actual);
        }

        [Fact]
        public void Can_Partially_Update_Product_With_Success()
        {
            // Arrange - Create mock repository
            var repository = new MockRepository<Product>(_data);

            // Arrange - Create controller
            var controller = new ProductsController(repository);

            // Arrange - Get product to update
            var expected = _data[2];
            var expectedName = "PatchUpdatedName";

            // Arrange - Build update package
            var patchDocument = new JsonPatchDocument();
            patchDocument.Replace("Name", expectedName);

            // Act
            var actualResult = controller.Patch(expected.Id, patchDocument) as ObjectResult;

            // Assert - Verify return value
            Assert.NotNull(actualResult);

            // Assert - Check for Http Status Code
            Assert.True(actualResult.StatusCode != null, "Reponse status code is null");
            Assert.True(actualResult.StatusCode == (int)HttpStatusCode.OK, "Response status code is not 200 (Ok)");

            //Assert - Test response object
            Assert.True(actualResult != null, "Response object is null");
            Assert.True(actualResult.Value != null, "Response value is null");

            // Assert - Test type of response data
            var actual = actualResult.Value as Product;
            Assert.True(actual != null, "response data is not of type Product");

            // Assert - Test returned entity
            ValidateProduct(actual, "Patched Product");

            // Assert - Test updated property
            Assert.Equal(expectedName, actual.Name);
        }

        [Fact]
        public void Can_Delete_Product_With_Success()
        {
            // Arrange - Create mock repository
            var repository = new MockRepository<Product>(_data);

            // Arrange - Create controller
            var controller = new ProductsController(repository);

            // Arrange - Get product to delete
            var expected = _data[2];

            // Arrange - Set expected record count
            var expectedCount = _data.Count - 1;

            // Act
            var actualResult = controller.Delete(expected.Id);

            // Assert - Verify return value
            Assert.NotNull(actualResult);

            // Assert - Check for Http Status Code
            var actual = actualResult as NoContentResult;
            Assert.True(actual != null, "Response is not of type NoContentResult");
            Assert.True(actual.StatusCode == (int)HttpStatusCode.NoContent, "Response status code is not 204 (No Content)");
            
            // Assert - Check repository to confirm deletion
            var actualCount = repository.Get().Count();
            Assert.Equal(expectedCount, actualCount);
        }
    }
}
