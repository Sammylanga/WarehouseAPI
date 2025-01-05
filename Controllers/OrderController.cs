using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Models;
using WarehouseApi.Services;

namespace WarehouseApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly WarehouseService _service;

        public OrderController(WarehouseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            try
            {
                var createdOrder = await _service.CreateOrderAsync(order);
                return CreatedAtAction(nameof(CreateOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = "InvalidOperationException",
                    message = ex.Message
                });
            }
        }
    }
}
