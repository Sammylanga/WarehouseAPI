using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Models;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers
{
    [ApiController]
    [Route("api/warehouse")]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseService _service;

        public WarehouseController(WarehouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            return Ok(await _service.GetWarehousesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] Warehouse warehouse)
        {
            try
            {
                var createdWarehouse = await _service.CreateWarehouseAsync(warehouse);
                return CreatedAtAction(nameof(CreateWarehouse), new { id = createdWarehouse.Id }, createdWarehouse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    error = "InvalidOperationException",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "ServerError",
                    message = ex.Message
                });
            }
        }
    }
}
