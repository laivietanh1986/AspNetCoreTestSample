using AspNetCoreSample.Data;
using AspNetCoreSample.Pages.Products;
using AspNetCoreSample.Tests.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreSample.Tests.Products
{
    public class IndexTests
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel_WithAListOfProducts()
        {
            using (var context = new ApplicationDbContext(TestDbContextHelper.TestDbContextOptions()))
            {
                context.Products.Add(new Product { Name = "Product 1", Price = 99 });
                context.Products.Add(new Product { Name = "Product 2", Price = 500 });
                context.SaveChanges();

                var pageModel = new IndexModel(context);

                await pageModel.OnGetAsync();

                Assert.IsAssignableFrom<IList<Product>>(pageModel.Product);
                Assert.Equal(2, pageModel.Product.Count);
            }
        }
    }
}
