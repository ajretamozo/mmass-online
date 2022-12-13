using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Tipos_avisosController : ControllerBase
    {
        private ITipos_avisosService _tipos_AvisosService;

        public Tipos_avisosController(ITipos_avisosService tipos_AvisosService)
        {
            _tipos_AvisosService = tipos_AvisosService;
        }

        [HttpPost("getAll")]
        public IActionResult getAll()
        {
            var tipos_avisos = _tipos_AvisosService.getAll();
            return Ok(tipos_avisos);
        }

        [HttpPost("save")]
        public IActionResult save(Dg_tipos_avisos miobj)
        {
            var tipos_avisos = _tipos_AvisosService.saveTipo_aviso(miobj);
            return Ok(tipos_avisos);
        }

        [HttpPost("remove")]
        public IActionResult remove(Dg_tipos_avisos miobj)
        {
            var tipos_avisos = _tipos_AvisosService.removeTipo_aviso(miobj);
            return Ok(tipos_avisos);
        }

        [HttpPost("getById")]
        public IActionResult getById(Dg_tipos_avisos miobj)
        {
            var tipos_avisos = _tipos_AvisosService.getById(miobj.Id_categoria);
            return Ok(tipos_avisos);
        }

        [HttpPost("filter")]
        public IActionResult filter([FromBody] string descripcion)
        {
            var fu = _tipos_AvisosService.filter(descripcion);
            return Ok(fu);
        }
    }
}