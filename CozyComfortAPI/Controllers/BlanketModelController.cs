using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozyComfortAPI.Data;
using CozyComfortAPI.Model;
using CozyComfortAPI.DTO;
using AutoMapper;

namespace CozyComfortAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlanketModelController : Controller
    {
        private IMapper mapper;
        private BlanketModelRepo repo;

        public BlanketModelController(IMapper _mapper, BlanketModelRepo _repo)
        {
            mapper = _mapper;
            repo = _repo;
        }
        [HttpPost]
        public ActionResult AddProduct(ModelWriteDTO dto)
        {
            var blanketModel = mapper.Map<BlanketModel>(dto);
            if (repo.Add(blanketModel))
                return Ok();
            return BadRequest();
        }
        [HttpGet]
        public ActionResult<List<ModelReadDTO>> GetblanketModels()
        {
            var blanketModels = repo.GetBlanketModels();
            return Ok(mapper.Map<List<ModelReadDTO>>(blanketModels));
        }
        [HttpGet("{id}")]
        public ActionResult<ModelReadDTO> GetProduct(int id)
        {
            var blanketModel = repo.GetBlanketModel(id);
            if (blanketModel != null)
            {
                return Ok(mapper.Map<ModelReadDTO>(blanketModel));
            }
            return NotFound();
        }
        [HttpPut("{id}")]
        public ActionResult UpdateProduct(ModelWriteDTO dto, int id)
        {
            var blanketModel = mapper.Map<BlanketModel>(dto);
            blanketModel.ModelID = id;
            if (repo.Update(blanketModel))
                return Ok();
            return NotFound();
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteblanketModel(int id)
        {
            var blanketModel = repo.GetBlanketModel(id);
            if (blanketModel != null)
            {
                repo.Remove(blanketModel);
                return Ok();
            }
            return NotFound();
        }
    }
}
