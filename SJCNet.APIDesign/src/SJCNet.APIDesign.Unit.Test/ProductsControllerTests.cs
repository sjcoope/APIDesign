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
    }
}
