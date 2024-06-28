using TestProj.Entities;

namespace TestProj.Interfaces
{
    public interface ISuppliersProductsConverter
    {
        IEnumerable<Product> ConvertToProducts(string xmlContent, string supplierName);
    }
}
