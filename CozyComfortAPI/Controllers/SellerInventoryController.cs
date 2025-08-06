using CozyComfortAPI.Data;
using CozyComfortAPI.Models;
using CozyComfortAPI.DTO; // Include your DTO namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CozyComfortAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerInventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SellerInventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SellerInventory/5
        [HttpGet("{sellerId}")]
        public async Task<IActionResult> GetInventory(int sellerId)
        {
            var inventory = await _context.SellerInventories
                .Where(i => i.SellerId == sellerId)
                .ToListAsync();
            return Ok(inventory);
        }

        // POST: api/SellerInventory/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdateInventory([FromBody] SellerInventoryUpdateDTO updateDto)
        {
            // First, find the existing inventory item
            var inventory = await _context.SellerInventories
                .FirstOrDefaultAsync(i => i.SellerId == updateDto.SellerId && i.BlanketModelId == updateDto.BlanketModelId);

            if (inventory == null)
            {
                // If no record exists, create a new one
                var newInventory = new SellerInventory
                {
                    SellerId = updateDto.SellerId,
                    BlanketModelId = updateDto.BlanketModelId,
                    Quantity = updateDto.Quantity
                };
                _context.SellerInventories.Add(newInventory);
            }
            else
            {
                // If a record exists, update its quantity
                inventory.Quantity = updateDto.Quantity;
                _context.SellerInventories.Update(inventory);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}