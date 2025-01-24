using MongoDB.Bson;
using MongoDbApplication.Models;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ProductPriceType_WhenRetrieved_ShouldBeDecimal()
        {
            Product tomato = new Product { ProductId = "P001", ProductName = "Apple", ProductCategoryId = "C001", SupplierId = "S001", ImageUrl = "Images\\Apple.jfif" };
            Assert.AreEqual(tomato.ProductPrice.GetType(), typeof(decimal));
        }
    }
}