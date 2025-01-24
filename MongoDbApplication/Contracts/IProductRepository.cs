using System.ComponentModel;
using MongoDbApplication.Models;

namespace MongoDbApplication.Contracts
{
    public interface IProductRepository
    {
        public Task<bool> AddProduct(Product product);
        public Task<IEnumerable<Product>> GetAllProducts();
        public Task<bool> ReplaceProduct(Product product , string _id);
        public Task<long> UpdateProductDetails(ProductUpdates updates, string _id);
        public Task<long> UpdateProductPricesByName(string name, double price);
    }
}
