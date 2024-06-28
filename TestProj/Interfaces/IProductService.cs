using TestProj.Entities;

namespace TestProj.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(string id);
        Task SaveOrUpdateProducts(IEnumerable<Product> products);
        Task<IEnumerable<Product>?> GetFlaggedProducts();
        Task<Product?> FlagProduct(string id);
    }
}
