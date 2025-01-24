using MongoDB.Driver;
using MongoDbApplication.Contracts;
using MongoDbApplication.DB;
using MongoDbApplication.Models;

namespace MongoDbApplication.Services
{
    public class ProductService

    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }   

        public async Task<bool> AddProduct(Product product)
        {
            
            return await _productRepository.AddProduct(product);
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return products;
        }
    }
}
