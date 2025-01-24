using MongoDB.Bson;
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
        public async Task<bool> ReplaceProduct(Product product , string _id)
        {
            return await _productRepository.ReplaceProduct(product, _id);
            
        }
        public async Task<long> UpdateProductDetails(ProductUpdates updates, string _id)
        {
            return await _productRepository.UpdateProductDetails(updates, _id); 
        }
        public async Task<long> UpdateProductPricesByName(string name, double price)
        {

            return await _productRepository.UpdateProductPricesByName(name, price);


        }
    }
}
