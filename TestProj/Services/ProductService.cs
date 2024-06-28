using TestProj.Entities;
using TestProj.Interfaces;

namespace TestProj.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _productRepository.GetAll();
            return products;
        }

        public async Task<Product?> GetById(string id)
        {
            var product = await _productRepository.GetById(id);
            return product;
        }

        public async Task SaveOrUpdateProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                var existingProduct = await _productRepository.GetById(product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.ProductCode = product.ProductCode;
                    existingProduct.StockQuantity = product.StockQuantity;
                    existingProduct.Photos = product.Photos;
                    existingProduct.IsFlagged = product.IsFlagged;

                    await _productRepository.Update(existingProduct);
                }
                else
                {
                    await _productRepository.Add(product);
                }
            }
        }

        public async Task<IEnumerable<Product>?> GetFlaggedProducts()
        {
            var flaggedProducts = await _productRepository.GetFlaggedProducts();
            return flaggedProducts;
        }

        public async Task<Product?> FlagProduct(string id)
        {
            var product = await _productRepository.FlagProduct(id);
            return product;
        }
    }
}
