using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Models;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers
{
    [ApiController]
    [Route("api/warehouseproduct")]
    public class WarehouseProductsController : ControllerBase
    {
        private readonly WarehouseService _service;

        public WarehouseProductsController(WarehouseService service)
        {
            _service = service;
        }

        // Get all Warehouse Products
        [HttpGet]
        public async Task<IActionResult> GetWarehouseProducts([FromQuery] int? warehouseId, [FromQuery] string? productCode)
        {
            var warehouseProducts = await _service.GetProductsInWarehouseAsync(warehouseId, productCode);
            return Ok(warehouseProducts);
        }

        // Get a specific Warehouse Product by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseProduct(int id)
        {
            var warehouseProduct = await _service.GetProductsInWarehouseAsync(null, null)
                .ContinueWith(task => task.Result.FirstOrDefault(wp => wp.Id == id));

            if (warehouseProduct == null)
                return NotFound(new { error = "Warehouse Product not found." });

            return Ok(warehouseProduct);
        }

        // Add a new Warehouse Product
        [HttpPost]
        public async Task<IActionResult> CreateWarehouseProduct([FromBody] WarehouseProduct warehouseProduct)
        {
            try
            {
                var existingProduct = await _service.GetProductsInWarehouseAsync(warehouseProduct.WarehouseId, null)
                    .ContinueWith(task => task.Result.FirstOrDefault(wp => wp.ProductId == warehouseProduct.ProductId));

                if (existingProduct != null)
                    return BadRequest(new { error = "This product already exists in the warehouse." });

                // Add the new product
                warehouseProduct.Warehouse = null!;
                warehouseProduct.Product = null!;
                await _service.CreateWarehouseProductAsync(warehouseProduct);

                return CreatedAtAction(nameof(GetWarehouseProduct), new { id = warehouseProduct.Id }, warehouseProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Update Warehouse Product Quantity
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouseProduct(int id, [FromBody] int quantity)
        {
            try
            {
                var warehouseProduct = await _service.GetProductsInWarehouseAsync(null, null)
                    .ContinueWith(task => task.Result.FirstOrDefault(wp => wp.Id == id));

                if (warehouseProduct == null)
                    return NotFound(new { error = "Warehouse Product not found." });

                warehouseProduct.Quantity = quantity;
                await _service.UpdateWarehouseProductAsync(warehouseProduct);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
