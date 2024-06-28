using Microsoft.EntityFrameworkCore;
using TestProj.Interfaces;
using TestProj.Persistence;
using TestProj.Repositories;
using TestProj.Seeders;
using TestProj.Services;

namespace TestProj.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SupplierAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TestProj")));
            services.AddScoped<ProductSeeder>();
            services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
        }

        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISuppliersProductsConverter, SuppliersProductsConverter>();
        }
    }
}
