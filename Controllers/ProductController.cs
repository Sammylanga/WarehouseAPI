using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Models;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly WarehouseService _service;

        public ProductController(WarehouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _service.GetProductsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                var createdProduct = await _service.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProducts), new { id = createdProduct.Id }, createdProduct);
            }
            catch (InvalidOperationException ex)
            {
                // Return a JSON response for invalid operation exceptions
                return BadRequest(new
                {
                    error = "InvalidOperationException",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Return a general JSON response for other exceptions
                return StatusCode(500, new
                {
                    error = "ServerError",
                    message = ex.Message
                });
            }
        }
    }
}
