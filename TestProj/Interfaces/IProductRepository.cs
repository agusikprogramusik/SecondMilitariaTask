using TestProj.Entities;

namespace TestProj.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(string id);
        Task Update(Product existingProduct);
        Task Add(Product product);
        Task<IEnumerable<Product>?> GetFlaggedProducts();
        Task<Product?> FlagProduct(string id);
    }
}
