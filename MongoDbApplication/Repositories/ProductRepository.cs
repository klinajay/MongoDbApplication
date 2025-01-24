using System.ComponentModel;
using MongoDB.Bson;
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
        public async Task<bool> ReplaceProduct(Product product, string _id)
        {
            var existingProduct = await _databaseContext.Products.FindAsync(p => p._id == _id);
            if(existingProduct == null)
            {
                return false;
            }
            product._id = _id;

            await _databaseContext.Products.ReplaceOneAsync(p => p._id == _id, product);
            return true;
        }
        public async Task<long> UpdateProductDetails(ProductUpdates updates, string _id)
        {
            if (updates == null) return -1;
            var existingProduct = await _databaseContext.Products.FindAsync(p => p._id == _id);
            if(existingProduct == null) { return -1; }

            var updateDefinition = new List<UpdateDefinition<Product>>();
            if(updates.ProductName != null)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductName, updates.ProductName));
            }
            if(updates.ProductPrice != decimal.Zero)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductPrice, updates.ProductPrice));
                
            }
            if (updates.Description != null)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.Description, updates.Description));

            }
            if (updates.SupplierId != null)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.SupplierId, updates.SupplierId));

            }
            if (updates.ProductCategoryId != null)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductCategoryId, updates.ProductCategoryId));

            }
            if (updates.ProductQuantity != null)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductQuantity, updates.ProductQuantity));

            }
            if(updates.ImageUrl != null)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ImageUrl, updates.ImageUrl));

            }
            var combinedUpdate = Builders<Product>.Update.Combine(updateDefinition);
            var result = await _databaseContext.Products.UpdateOneAsync(p => p._id == _id, combinedUpdate);
            if (result.MatchedCount == 0) return -1;
            else return result.ModifiedCount;
        }

    }
}
