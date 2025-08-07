// Inside CozyComfortAPI/Controllers/SellerInventoryController.cs
using CozyComfortAPI.Data;
using CozyComfortAPI.Models;
using CozyComfortAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; // Add this namespace

namespace CozyComfortAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerInventoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper; // Add this field

        public SellerInventoryController(AppDbContext context, IMapper mapper) // Update the constructor
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/SellerInventory/5
        [HttpGet("{sellerId}")]
        public async Task<IActionResult> GetInventory(int sellerId)
        {
            var inventory = await _context.SellerInventories
                .Where(i => i.SellerId == sellerId)
                .Include(i => i.BlanketModel)
                    .ThenInclude(bm => bm.Material)
                .ToListAsync();

            var inventoryDto = _mapper.Map<IEnumerable<SellerInventoryUpdateDTO>>(inventory);

            return Ok(inventoryDto);
        }

        // POST: api/SellerInventory/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdateInventory([FromBody] SellerInventoryUpdateDTO updateDto)
        {
            var inventory = await _context.SellerInventories
                .FirstOrDefaultAsync(i => i.SellerId == updateDto.SellerId && i.BlanketModelId == updateDto.BlanketModelId);

            if (inventory == null)
            {
                var newInventory = _mapper.Map<SellerInventory>(updateDto);
                _context.SellerInventories.Add(newInventory);
            }
            else
            {
                _mapper.Map(updateDto, inventory);
                _context.SellerInventories.Update(inventory);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}