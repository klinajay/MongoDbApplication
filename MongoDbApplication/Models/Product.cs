using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbApplication.Models
{
    public class Product
    {
        public string ProductName { get; set; }
        public ObjectId _id { get; set; }
        public string Description { get; set; }
        public string SupplierId { get; set; }
        public string ProductCategoryId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Double)]
        public decimal ProductPrice { get; set; } = decimal.Zero;
        public int ProductQuantity { get; set; }
        public string ImageUrl { get; set; }

    }
}
