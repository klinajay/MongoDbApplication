using System.ComponentModel;
using System.Security.Cryptography;
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
            if (!string.IsNullOrEmpty(updates.ProductName))
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductName, updates.ProductName));
            }
            if(updates.ProductPrice != decimal.Zero)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductPrice, updates.ProductPrice));
                
            }
            if (!string.IsNullOrEmpty(updates.Description))
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.Description, updates.Description));

            }
            if (!string.IsNullOrEmpty(updates.SupplierId))
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.SupplierId, updates.SupplierId));

            }
            if (!string.IsNullOrEmpty(updates.ProductCategoryId))
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductCategoryId, updates.ProductCategoryId));

            }
            if (updates.ProductQuantity != Decimal.Zero)
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ProductQuantity, updates.ProductQuantity));

            }
            if(!string.IsNullOrEmpty(updates.ImageUrl))
            {
                updateDefinition.Add(Builders<Product>.Update.Set(p => p.ImageUrl, updates.ImageUrl));

            }
            var combinedUpdate = Builders<Product>.Update.Combine(updateDefinition);
            var result = await _databaseContext.Products.UpdateOneAsync(p => p._id == _id, combinedUpdate);
            if (result.MatchedCount == 0) return -1;
            else return result.ModifiedCount;
        }
        public async Task<long> UpdateProductPricesByName(string name , double price)
        {

            var update = Builders<Product>.Update
                        .Inc("ProductPrice", price);
            var result = await _databaseContext.Products.UpdateManyAsync(p => p.ProductName == name, update);
            return result.ModifiedCount;
        }
        public async Task<Product> DeleteProduct(string _id)
        {
            var product = await _databaseContext.Products.FindOneAndDeleteAsync(p => p._id == _id);
            return product;
        }
        public async Task<long> DeleteProductsHavingQuantityZero()
        {
             
            var deleteResult = await _databaseContext.Products.DeleteManyAsync(product => product.ProductQuantity == 0);
            return deleteResult.DeletedCount;
        }
    }
}
