using System.ComponentModel;
using MongoDbApplication.Models;

namespace MongoDbApplication.Contracts
{
    public interface IProductRepository
    {
        public Task<bool> AddProduct(Product product);
        public Task<IEnumerable<Product>> GetAllProducts();
    }
}
