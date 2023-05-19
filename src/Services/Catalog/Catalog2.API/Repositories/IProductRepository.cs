using Catalog2.API.Entities;

namespace Catalog2.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);

        Task<IEnumerable<Product>> GetProductByName(string name);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> deleteProduct(string id);
    }
}
