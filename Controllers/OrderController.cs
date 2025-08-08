using AutoMapper;
using CozyComfortAPI.Data;
using CozyComfortAPI.DTO;
using CozyComfortAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CozyComfortAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public OrderController(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDTO>> PlaceOrder([FromBody] OrderWriteDTO orderDto)
        {
            if (!Request.Headers.TryGetValue("X-API-KEY", out var requestApiKey))
            {
                return Unauthorized("API Key is missing.");
            }

            var distributorApiKey = _configuration["ApiKeys:CozyComfortDistributorKey"];

            if (string.IsNullOrEmpty(distributorApiKey) || requestApiKey.ToString() != distributorApiKey)
            {
                return Unauthorized("Invalid API Key.");
            }

            int distributorId = 1;

            var blanketModel = await _context.BlanketModels.FirstOrDefaultAsync(b => b.ModelID == orderDto.ModelID);

            if (blanketModel == null)
            {
                return NotFound($"Model with ID {orderDto.ModelID} not found.");
            }

            if (blanketModel.Stock < orderDto.Quantity)
            {
                return BadRequest($"Insufficient stock. Only {blanketModel.Stock} units of {blanketModel.ModelName} are available.");
            }

            var order = _mapper.Map<Order>(orderDto);
            order.DistributorID = distributorId;
            order.OrderDate = System.DateTime.Now;
            order.Status = "Pending";
            order.Total = blanketModel.Price * orderDto.Quantity;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderReadDto = _mapper.Map<OrderReadDTO>(order);

            return CreatedAtAction(nameof(GetOrdersForDistributor), new { distributorId = order.DistributorID }, orderReadDto);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status parameter is required.");
            }

            var order = await _context.Orders
                                      .Include(o => o.BlanketModel)
                                      .FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            if (order.Status == "Accepted" || order.Status == "Rejected")
            {
                return BadRequest($"Order with ID {orderId} has already been {order.Status.ToLower()}.");
            }

            if (status.Equals("Accepted", System.StringComparison.OrdinalIgnoreCase))
            {
                var blanketModel = order.BlanketModel;
                if (blanketModel.Stock < order.Quantity)
                {
                    return BadRequest($"Insufficient stock to accept order. Only {blanketModel.Stock} units of {blanketModel.ModelName} are available.");
                }

                blanketModel.Stock -= order.Quantity;
                order.Status = "Accepted";
            }
            else
            {
                order.Status = status;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(orderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("{distributorId}")]
        public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetOrdersForDistributor(int distributorId)
        {
            var orders = await _context.Orders
                                       .Where(o => o.DistributorID == distributorId)
                                       .Include(o => o.BlanketModel)
                                       .OrderByDescending(o => o.OrderDate)
                                       .Select(o => new OrderReadDTO
                                       {
                                           OrderID = o.OrderID,
                                           OrderDate = o.OrderDate,
                                           ModelName = o.BlanketModel.ModelName,
                                           Quantity = o.Quantity,
                                           Price = o.BlanketModel.Price,
                                           Total = o.BlanketModel.Price * o.Quantity,
                                           Status = o.Status
                                       })
                                       .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("No orders found for this distributor.");
            }

            return Ok(orders);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
