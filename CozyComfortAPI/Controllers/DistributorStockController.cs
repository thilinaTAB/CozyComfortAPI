using AutoMapper;
using CozyComfortAPI.Auth;
using CozyComfortAPI.Data;
using CozyComfortAPI.DTO;
using CozyComfortAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CozyComfortAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorStockController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DistributorStockController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Endpoint for a distributor to get their own specific stock
        [HttpGet("{distributorId}")]
        [ApiKeyAuth("Distributor")] // Only a distributor can access this
        public async Task<ActionResult<IEnumerable<DistributorStockReadDTO>>> GetDistributorStockByDistributorId(int distributorId)
        {
            var stocks = await _context.DistributorStocks
                .Where(s => s.DistributorID == distributorId)
                .Include(s => s.BlanketModel)
                .ThenInclude(bm => bm.Material)
                .ToListAsync();

            if (stocks == null || !stocks.Any())
            {
                return NotFound();
            }

            var stockReadDtos = _mapper.Map<List<DistributorStockReadDTO>>(stocks);
            return Ok(stockReadDtos);
        }

        // NEW: Endpoint for a seller to get ALL distributor stocks
        [HttpGet("all")]
        [ApiKeyAuth("Seller")] // Only a seller can access this
        public async Task<ActionResult<IEnumerable<DistributorStockReadDTO>>> GetAllDistributorStocks()
        {
            var stocks = await _context.DistributorStocks
                .Include(ds => ds.BlanketModel)
                .ThenInclude(bm => bm.Material)
                .ToListAsync();

            if (!stocks.Any())
            {
                return NotFound("No distributor stocks found.");
            }

            var stockReadDtos = _mapper.Map<List<DistributorStockReadDTO>>(stocks);
            return Ok(stockReadDtos);
        }

        // Endpoint for a distributor to create or update their stock
        [HttpPost]
        [ApiKeyAuth("Distributor")] // Only a distributor can access this
        public async Task<ActionResult<DistributorStockReadDTO>> CreateOrUpdateDistributorStock(DistributorStockWriteDTO stockDto)
        {
            var distributorStock = await _context.DistributorStocks
                .Include(s => s.BlanketModel)
                .ThenInclude(bm => bm.Material)
                .SingleOrDefaultAsync(s => s.DistributorID == stockDto.DistributorID && s.ModelID == stockDto.ModelID);

            if (distributorStock == null)
            {
                distributorStock = _mapper.Map<DistributorStock>(stockDto);
                _context.DistributorStocks.Add(distributorStock);
            }
            else
            {
                distributorStock.Inventory = stockDto.Inventory;
            }

            await _context.SaveChangesAsync();

            // Reload related entities after save to ensure they are available for mapping
            await _context.Entry(distributorStock).Reference(s => s.BlanketModel).LoadAsync();
            await _context.Entry(distributorStock.BlanketModel).Reference(bm => bm.Material).LoadAsync();

            var createdStockDto = _mapper.Map<DistributorStockReadDTO>(distributorStock);
            return CreatedAtAction(nameof(GetDistributorStockByDistributorId), new { distributorId = createdStockDto.DistributorID }, createdStockDto);
        }

        // Endpoint for a distributor to update a stock item
        [HttpPut("{distributorStockId}")]
        [ApiKeyAuth("Distributor")] // Only a distributor can access this
        public async Task<IActionResult> UpdateDistributorStock(int distributorStockId, DistributorStockWriteDTO stockDto)
        {
            var distributorStock = await _context.DistributorStocks.FindAsync(distributorStockId);
            if (distributorStock == null)
            {
                return NotFound();
            }
            distributorStock.Inventory = stockDto.Inventory;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!DistributorStockExists(distributorStockId))
            {
                return NotFound();
            }

            return NoContent();
        }

        // Endpoint for a distributor to delete a stock item
        [HttpDelete("{distributorStockId}")]
        [ApiKeyAuth("Distributor")] // Only a distributor can access this
        public async Task<IActionResult> DeleteDistributorStock(int distributorStockId)
        {
            var distributorStock = await _context.DistributorStocks.FindAsync(distributorStockId);
            if (distributorStock == null)
            {
                return NotFound();
            }

            _context.DistributorStocks.Remove(distributorStock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistributorStockExists(int id)
        {
            return _context.DistributorStocks.Any(e => e.DistributorStockID == id);
        }
    }
}