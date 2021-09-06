using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Dg_medios_asociadosController : ControllerBase
    {
        private IDg_medios_asociadosService _dg_medios_asociadosService;

        public Dg_medios_asociadosController(IDg_medios_asociadosService dg_medios_asociadosService)
        {
            _dg_medios_asociadosService = dg_medios_asociadosService;
        }

        [HttpPost("getById")]
        public IActionResult getById(Dg_medios_asociados miobj)
        {
            var dg_medios_asociados = _dg_medios_asociadosService.getById(miobj.Id_medio_dg, miobj.Id_medio_asociado);
            return Ok(dg_medios_asociados);
        }

        [HttpPost("getAllmediosAsociados")]
        public IActionResult getAllmediosAsociados([FromBody]string listaMedios)
        {
            var dg_medios_asociados = _dg_medios_asociadosService.getAllmediosAsociados(listaMedios);
            return Ok(dg_medios_asociados);
        }
    }
}
