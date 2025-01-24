using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbApplication.Models
{
    public class ProductUpdates
    {
        public string ProductName { get; set; } 
        public string Description { get; set; }
        public string SupplierId { get; set; }
        public string ProductCategoryId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Double)]
        public decimal ProductPrice { get; set; } = decimal.Zero;
        public int ProductQuantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
