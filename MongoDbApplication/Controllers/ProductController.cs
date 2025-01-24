using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDbApplication.Models;
using MongoDbApplication.Services;

namespace MongoDbApplication.Controllers
{
    [Route("/api/products")]
    
    public class ProductController : Controller
    {
        public ProductService productService;
        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct()
        {
            try
            {
                var bodyContent = await new StreamReader(Request.Body).ReadToEndAsync();

                if (string.IsNullOrEmpty(bodyContent)) return UnprocessableEntity("Product data expected.");

                var product = JsonSerializer.Deserialize<Product>(bodyContent);

                bool status = await productService.AddProduct(product);
                if (status)
                    return Ok();
                else
                    return NotFound();
            }
            catch(JsonException exception)
            {
                return BadRequest($"Invalid Json format: {exception.Message}");
            }
            catch(Exception exception)
            {
                return StatusCode(500 , $"Error occured while processing your request.");
            }
            
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProducts();
            if (products == null) return Ok(204);
            return Ok(products);
        }
    }
}
