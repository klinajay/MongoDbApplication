using System.ComponentModel;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbApplication.Contracts;
using MongoDbApplication.DB;
using MongoDbApplication.Models;

namespace MongoDbApplication.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _databaseContext;
        
        public ProductRepository(DatabaseContext databaseContext) {
            _databaseContext = databaseContext;
            
        }

        public async Task<bool> AddProduct(Product product) 
        {
            if (product == null) return false;

            await _databaseContext.Products.InsertOneAsync(product);
            return true;
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products =  _databaseContext.Products.AsQueryable();
            var queryResult = await products.Select(product => product).ToListAsync();
            return queryResult;
        }

    }
}
