using AspNetCoreSample;
using AspNetCoreSample.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetSample.E2ETests
{
    public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        public ProductsApiTests(WebApplicationFactory<Program> factory)
        {
            Factory = factory;
        }

        public WebApplicationFactory<Program> Factory { get; }

        [Fact]
        public async Task CreateProductOk()
        {
            var client = Factory.CreateClient();
            var product = new Product { Name = "Test product", Price = 10 };

            var response = await client.PostAsJsonAsync("/api/products", product);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateProductReturnBadRequestWhenEmptyName()
        {
            var client = Factory.CreateClient();
            var product = new Product { Name = "", Price = 10 };

            var response = await client.PostAsJsonAsync("/api/products", product);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProductsOk()
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync("/api/products");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadAsAsync<IList<Product>>();
            Assert.NotEmpty(products);
        }
    }
}
