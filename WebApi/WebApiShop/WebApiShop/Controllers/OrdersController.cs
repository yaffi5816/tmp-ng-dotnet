using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> Get([FromQuery] int? userId)
        {
            if (userId.HasValue)
            {
                var userOrders = await _orderService.GetByUserIdAsync(userId.Value);
                return Ok(userOrders);
            }
            var orders = await _orderService.GetAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetByUserId(int userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("user/{userId}/draft")]
        public async Task<ActionResult<OrderDTO>> GetDraftByUserId(int userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId);
            var draftOrder = orders.FirstOrDefault(o => !o.CurrentStatus);
            if (draftOrder == null)
                return NotFound();
            return Ok(draftOrder);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            var newOrder = await _orderService.AddAsync(order);
            return CreatedAtAction(nameof(Get), new { id = newOrder.OrderId }, newOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderDTO order)
        {
            await _orderService.UpdateAsync(id, order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }
    }
}
