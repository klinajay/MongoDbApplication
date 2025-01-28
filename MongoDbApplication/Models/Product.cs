using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbApplication.Models
{
    /// <summary>
    /// Product Model
    /// </summary>
    public class Product
    {
        [Required]
        public required string ProductName { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }
        public string Description { get; set; } = "";
        [Required]

        public required string SupplierId { get; set; }
        [Required]
        public required string ProductCategoryId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Double)]
        [Required]
        public required decimal ProductPrice { get; set; } = decimal.Zero;
        [Required]
        public required int ProductQuantity { get; set; }
        public string ImageUrl { get; set; } = "";
        [Required]
        public required ProductQuantityType QuantityType { get; set; }
        public enum ProductQuantityType
        {
            Piece,
            Kg,
            Liter,
            Dozen
        }
    }
}
