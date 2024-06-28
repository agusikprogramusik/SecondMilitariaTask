using Microsoft.EntityFrameworkCore;
using TestProj.Entities;
using TestProj.Interfaces;
using TestProj.Persistence;

namespace TestProj.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SupplierAppDbContext _dbContext;

        public ProductRepository(SupplierAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _dbContext.Products
                .OrderBy(p => p.ProductCode)
                .ToListAsync();
            return products;
        }
        public async Task<Product?> GetById(string id)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task Update(Product existingProduct)
        {
            if (_dbContext.Entry(existingProduct).State == EntityState.Detached)
            {
                _dbContext.Products.Attach(existingProduct);
            }
            _dbContext.Entry(existingProduct).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }

        public async Task Add(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>?> GetFlaggedProducts()
        {
            var flaggedProducts = await _dbContext.Products
                .Where(p => p.IsFlagged)
                .ToListAsync();
            return flaggedProducts;
        }

        public async Task<Product?> FlagProduct(string id)
        {
            var product = _dbContext.Products
                .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.IsFlagged = !product.IsFlagged;
                await _dbContext.SaveChangesAsync();
            }

            return product;
        }
    }
}
