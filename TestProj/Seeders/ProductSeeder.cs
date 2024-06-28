using TestProj.Entities;
using TestProj.Persistence;

namespace TestProj.Seeders
{
    public class ProductSeeder(SupplierAppDbContext dbContext)
    {
        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Products.Any())
                {
                    var products = new List<Product>
                {
                    new Product
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductCode = "00544533451",
                        Name = "Product 1",
                        Description = "Description of Product 1",
                        Price = 100,
                        StockQuantity = 5,
                        IsFlagged = true,
                        Photos = new List<string>
                        {
                            "https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=1999&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                    },
                    new Product
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductCode = "004522545242",
                        Name = "Product 2",
                        Description = "Description of Product 2",
                        Price = 200,
                        StockQuantity = 10,
                        Photos = new List<string>
                        {
                            "https://images.unsplash.com/photo-1532298229144-0ec0c57515c7?q=80&w=2022&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        },
                    }
                };

                    await dbContext.Products.AddRangeAsync(products);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
