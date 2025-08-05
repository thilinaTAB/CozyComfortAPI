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
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorStockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DistributorStockController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{distributorId}")]
        public async Task<ActionResult<IEnumerable<DistributorStockReadDTO>>> GetDistributorStockByDistributorId(int distributorId)
        {
            var stocks = await _context.DistributorStocks
                .Where(s => s.DistributorID == distributorId)
                .Include(s => s.BlanketModel)
                .ThenInclude(bm => bm.Material)
                .Select(s => new DistributorStockReadDTO
                {
                    DistributorStockID = s.DistributorStockID,
                    Inventory = s.Inventory,
                    DistributorID = s.DistributorID,
                    ModelID = s.ModelID,
                    BlanketModel = new ModelReadDTO
                    {
                        ModelID = s.BlanketModel.ModelID,
                        ModelName = s.BlanketModel.ModelName,
                        Price = s.BlanketModel.Price,
                        Description = s.BlanketModel.Description,
                        Stock = s.BlanketModel.Stock,
                        MaterialID = s.BlanketModel.MaterialID,
                        MaterialName = s.BlanketModel.Material.MaterialName,
                        MaterialDescription = s.BlanketModel.Material.Description
                    }
                })
                .ToListAsync();

            if (stocks == null || !stocks.Any())
            {
                return NotFound();
            }

            return Ok(stocks);
        }

        [HttpPost]
        public async Task<ActionResult<DistributorStockReadDTO>> CreateOrUpdateDistributorStock(DistributorStockWriteDTO stockDto)
        {
            var distributorStock = await _context.DistributorStocks
                .Include(s => s.BlanketModel)
                .ThenInclude(bm => bm.Material)
                .SingleOrDefaultAsync(s => s.DistributorID == stockDto.DistributorID && s.ModelID == stockDto.ModelID);

            if (distributorStock == null)
            {
                distributorStock = new DistributorStock
                {
                    DistributorID = stockDto.DistributorID,
                    ModelID = stockDto.ModelID
                };
                _context.DistributorStocks.Add(distributorStock);
            }
            else
            {
                distributorStock.Inventory = stockDto.Inventory;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                return StatusCode(500, $"An error occurred while saving changes: {innerException?.Message}");
            }
            if (distributorStock.BlanketModel == null)
            {
                await _context.Entry(distributorStock).Reference(s => s.BlanketModel).LoadAsync();
            }
            if (distributorStock.BlanketModel.Material == null)
            {
                await _context.Entry(distributorStock.BlanketModel).Reference(bm => bm.Material).LoadAsync();
            }

            var createdStockDto = new DistributorStockReadDTO
            {
                DistributorStockID = distributorStock.DistributorStockID,
                Inventory = distributorStock.Inventory,
                DistributorID = distributorStock.DistributorID,
                ModelID = distributorStock.ModelID,
                BlanketModel = new ModelReadDTO
                {
                    ModelID = distributorStock.BlanketModel.ModelID,
                    ModelName = distributorStock.BlanketModel.ModelName,
                    Price = distributorStock.BlanketModel.Price,
                    Description = distributorStock.BlanketModel.Description,
                    Stock = distributorStock.BlanketModel.Stock,
                    MaterialID = distributorStock.BlanketModel.MaterialID,
                    MaterialName = distributorStock.BlanketModel.Material.MaterialName,
                    MaterialDescription = distributorStock.BlanketModel.Material.Description
                }
            };

            return CreatedAtAction(nameof(GetDistributorStockByDistributorId), new { distributorId = createdStockDto.DistributorID }, createdStockDto);
        }

        [HttpPut("{distributorStockId}")]
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

        [HttpDelete("{distributorStockId}")]
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
