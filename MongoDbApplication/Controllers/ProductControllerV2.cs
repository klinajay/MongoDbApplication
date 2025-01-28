using System.Text.Json;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MongoDbApplication.Models;
using MongoDbApplication.Services;

namespace MongoDbApplication.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/products")]
    
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class ProductControllerV2 : Controller
    {
        public ProductService productService;
        public ProductControllerV2(ProductService productService)
        {
            this.productService = productService;
        }
        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>Returns 200 if successful, 422 if product data is invalid , 500 if server error occurs.</returns>
        [HttpPost]
        [Consumes("application/xml", "application/json")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                //var bodyContent = await new StreamReader(Request.Body).ReadToEndAsync();

                if (product == null) return UnprocessableEntity("Product data expected.");

                //product = JsonSerializer.Deserialize<Product>(bodyContent);

                bool status = await productService.AddProduct(product);
                if (status)
                    return Ok();
                else
                    return StatusCode(500, "Internal server error.");
            }
            catch (JsonException exception)
            {
                return BadRequest($"Invalid Json format: {exception.Message}");
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error occured while processing your request; {exception.Message}");
            }


        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns>200 if successfull , 422 if product data is invalid , 404 if product not found. </returns>
        /// 
        [HttpGet]
        [Produces("application/xml")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProducts();
            if (products == null) return Ok(204);
            return Ok(products);
        }
        /// <summary>
        /// updates a single product.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>200 if successfull , 422 if product data is invalid , 404 if product not found.</returns>
        [HttpPatch("/{id}/update")]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> UpdateProductDetails(string id, [FromBody] ProductUpdates product)
        {
            try
            {
                if (product == null)
                    return UnprocessableEntity("Update data required.");
                long modifiedProduct = await productService.UpdateProductDetails(product, id);
                if (modifiedProduct != -1)
                {
                    return Ok($"{modifiedProduct} Product updated successfully.");
                }
                else
                {
                    return NotFound("Product not found.");
                }
            }
            catch (JsonException exception)
            {
                return BadRequest($"Invalid Json format: {exception.Message}");
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error occured while processing your request; {exception.Message}");
            }
        }
        /// <summary>
        /// updates thhe price of products those having the common names.
        /// </summary>
        /// <param name="data">Need to give name and price in form of dictionary</param>
        /// <returns>200 if successfull , 422 if product data is invalid , 404 if product not found. </returns>
        [HttpPatch("/update")]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> UpdateProductDetails([FromBody] Dictionary<string, string> data)
        {
            try
            {
                if (!data.ContainsKey("name") || !data.ContainsKey("price"))
                    return UnprocessableEntity("Update data required.");

                string name = data["name"]?.ToString();
                double price = Convert.ToDouble(data["price"]);
                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                    return UnprocessableEntity("Update data required.");
                long modifiedProductCount = await productService.UpdateProductPricesByName(name, price);
                if (modifiedProductCount != 0)
                {
                    return Ok($"{modifiedProductCount} Products updated successfully.");
                }
                else
                {
                    return NotFound($"Products not found with name {name}.");
                }
            }
            catch (JsonException exception)
            {
                return BadRequest($"Invalid Json format: {exception.Message}");
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error occured while processing your request; {exception.Message}");
            }
        }
        /// <summary>
        /// Deletes a single product.
        /// </summary>
        /// <param name="id">unique id of product.</param>
        /// <returns></returns>
        [HttpDelete("/{id}")]

        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var deletedProduct = await productService.DeleteProduct(id);
                if (deletedProduct != null)
                {
                    return Ok(deletedProduct);
                }
                else
                {
                    return NotFound("Product not found.");
                }
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error occured while processing your request; {exception.Message}");
            }
        }
        /// <summary>
        /// Remove out of stock products (cleanup).
        /// </summary>
        /// <returns></returns>
        [HttpDelete("/cleanup")]
        public async Task<IActionResult> DeleteProductsHavingQuantityZero()
        {
            try
            {
                long deletedProductsCount = await productService.DeleteProductHavingPriceZero();
                if (deletedProductsCount == 0)
                    return Ok("All products are available, no need for cleanup.");
                else
                    return Ok($"{deletedProductsCount} products removed from inventory.");
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error occured while processing your request; {exception.Message}");
            }

        }
    }
}
