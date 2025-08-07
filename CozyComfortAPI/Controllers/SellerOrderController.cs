using AutoMapper;
using CozyComfortAPI.Data;
using CozyComfortAPI.DTO;
using CozyComfortAPI.Model;
using CozyComfortAPI.Models;
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
    public class SellerOrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SellerOrderController(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<SellerOrderReadDTO>> PlaceSellerOrder([FromBody] SellerOrderWriteDTO orderDto)
        {
            // 1. Authentication: Validate the API key from appsettings.json
            if (!Request.Headers.TryGetValue("X-API-KEY", out var requestApiKey))
            {
                return Unauthorized("API Key is missing.");
            }

            var sellerApiKey = _configuration["ApiKeys:CozyComfortSellerKey"];
            if (string.IsNullOrEmpty(sellerApiKey) || requestApiKey.ToString() != sellerApiKey)
            {
                return Unauthorized("Invalid API Key.");
            }

            // 2. Validate the SellerId from the DTO
            var seller = await _context.Sellers.FindAsync(orderDto.SellerId);
            if (seller == null)
            {
                return NotFound($"Seller with ID {orderDto.SellerId} not found.");
            }

            // 3. Find the DistributorStock based on the ID from the DTO
            var distributorStock = await _context.DistributorStocks
                .Include(ds => ds.BlanketModel)
                .FirstOrDefaultAsync(ds => ds.DistributorStockID == orderDto.DistributorStockID);

            if (distributorStock == null)
            {
                return NotFound($"Distributor stock with ID {orderDto.DistributorStockID} not found.");
            }

            // 4. Check inventory
            if (distributorStock.Inventory < orderDto.Quantity)
            {
                return BadRequest($"Insufficient stock. Only {distributorStock.Inventory} units of {distributorStock.BlanketModel.ModelName} are available.");
            }

            // 5. Create the order
            var order = _mapper.Map<SellerOrder>(orderDto);

            // 6. Manually set properties not included in the DTO
            order.SellerID = orderDto.SellerId;
            // DistributorStockID is already set from the DTO via mapping
            order.OrderDate = System.DateTime.Now;
            order.Status = "Pending";
            order.Total = distributorStock.BlanketModel.Price * orderDto.Quantity;

            // 7. Update the stock
            distributorStock.Inventory -= orderDto.Quantity;
            _context.DistributorStocks.Update(distributorStock);
            _context.SellerOrders.Add(order);
            await _context.SaveChangesAsync();

            // 8. Prepare the read DTO for the response
            var orderReadDto = new SellerOrderReadDTO
            {
                SellerOrderID = order.SellerOrderID,
                OrderDate = order.OrderDate,
                ModelName = distributorStock.BlanketModel.ModelName,
                Quantity = order.Quantity,
                Price = distributorStock.BlanketModel.Price,
                Total = order.Total,
                Status = order.Status
            };

            return CreatedAtAction(nameof(GetOrdersForSeller), new { sellerId = order.SellerID }, orderReadDto);
        }

        [HttpGet("{sellerId}")]
        public async Task<ActionResult<IEnumerable<SellerOrderReadDTO>>> GetOrdersForSeller(int sellerId)
        {
            var orders = await _context.SellerOrders
                                       .Where(o => o.SellerID == sellerId)
                                       .Include(o => o.DistributorStock)
                                       .ThenInclude(ds => ds.BlanketModel)
                                       .OrderByDescending(o => o.OrderDate)
                                       .Select(o => new SellerOrderReadDTO
                                       {
                                           SellerOrderID = o.SellerOrderID,
                                           OrderDate = o.OrderDate,
                                           ModelName = o.DistributorStock.BlanketModel.ModelName,
                                           Quantity = o.Quantity,
                                           Price = o.DistributorStock.BlanketModel.Price,
                                           Total = o.Total,
                                           Status = o.Status
                                       })
                                       .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("No orders found for this seller.");
            }

            return Ok(orders);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateSellerOrderStatus(int orderId, [FromQuery] string status)
        {
            // This endpoint is for a distributor or admin to manage orders, not for sellers.
            return BadRequest("This endpoint is designed for distributor/admin access to manage orders.");
        }

        private bool SellerOrderExists(int id)
        {
            return _context.SellerOrders.Any(e => e.SellerOrderID == id);
        }
    }
}