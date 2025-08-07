using CozyComfortAPI.Data;
using CozyComfortAPI.Model;
using CozyComfortAPI.DTO;
using CozyComfortAPI.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CozyComfortAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MaterialController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Material
        // Accessible by Manufacturer, Distributor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialReadDTO>>> GetMaterials()
        {
            var materials = await _context.Materials.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<MaterialReadDTO>>(materials));
        }

        // Accessible by Manufacturer, Distributor
        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialReadDTO>> GetMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MaterialReadDTO>(material));
        }

        // Accessible ONLY by Manufacturer
        [HttpPost]
        [ApiKeyAuth(requiredRole: "Manufacturer")]
        public async Task<ActionResult<MaterialReadDTO>> CreateMaterial(MaterialWriteDTO materialDto)
        {
            var material = _mapper.Map<Material>(materialDto);

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            // Return the created material with its generated ID
            var materialReadDto = _mapper.Map<MaterialReadDTO>(material);
            return CreatedAtAction(nameof(GetMaterial), new { id = materialReadDto.MaterialID }, materialReadDto);
        }

        // Accessible ONLY by Manufacturer
        [HttpPut("{id}")]
        [ApiKeyAuth(requiredRole: "Manufacturer")]
        public async Task<IActionResult> UpdateMaterial(int id, MaterialWriteDTO materialDto)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            _mapper.Map(materialDto, material);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // Accessible ONLY by Manufacturer
        [HttpDelete("{id}")]
        [ApiKeyAuth(requiredRole: "Manufacturer")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.MaterialID == id);
        }
    }
}