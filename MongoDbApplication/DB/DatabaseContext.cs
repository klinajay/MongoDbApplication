
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using MongoDB.Driver;
using MongoDbApplication.Models;
namespace MongoDbApplication.DB
{
    public class DatabaseContext
    {
        private readonly IConfiguration _configuration;
        public IMongoDatabase database;
        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient("mongodb://localhost:3015/");
            database = client.GetDatabase("InventoryManager");
        }
        public IMongoCollection<Product> Products => database.GetCollection<Product>("Products");
    }
}
