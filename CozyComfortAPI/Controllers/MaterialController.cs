using CozyComfortAPI.Data;
using CozyComfortAPI.Model;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MaterialController : Controller
{
    private readonly AppDBContext _context;

    public MaterialController(AppDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<List<Material>> GetMaterials()
    {
        var materials = _context.Materials.ToList();
        return Ok(materials);
    }
}
