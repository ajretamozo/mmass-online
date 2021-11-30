using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactosController : ControllerBase
    {
        private IContactoService _contactoService;

        public ContactosController(IContactoService contactoService)
        {
            _contactoService = contactoService;
        }

        
        [HttpPost("getAgencias")]
        public IActionResult GetAgencias()
        {
            var contactos =  _contactoService.GetAgencias();
            return Ok(contactos);
        }

        
        [HttpPost("getAnunciantes")]
        public IActionResult GetAnunciantes()
        {
            var contactos = _contactoService.GetAnunciantes();
            return Ok(contactos);
        }
        [HttpPost("getEjecutivos")]
        public IActionResult GetEjecutivos()
        {
            var contactos = _contactoService.GetEjecutivos();
            return Ok(contactos);
        }

        [HttpPost("saveASRelation")]
        public IActionResult saveASRelation(Contacto p)
        {
            var result = _contactoService.saveASRelation(p.Id,p.IdContactoDigital, p.IdRed);
            return Ok(result);
        }

        [HttpPost("getAnunciantesPorAgencia")]
        public IActionResult GetAnunciantesPorAgencia([FromBody] int idAgencia)
        {
            var contactos = _contactoService.GetAnunciantesPorAgencia(idAgencia);
            return Ok(contactos);
        }

        [HttpPost("getAgenciasPorAnunciante")]
        public IActionResult GetAgenciasPorAnunciante([FromBody] int idAnunciante)
        {
            var contactos = _contactoService.GetAgenciasPorAnunciante(idAnunciante);
            return Ok(contactos);
        }

        [HttpPost("getAnunciantesPorProducto")]
        public IActionResult GetAnunciantesPorProducto([FromBody] int idProducto)
        {
            var contactos = _contactoService.GetAnunciantesPorProducto(idProducto);
            return Ok(contactos);
        }

        [HttpPost("getContactoByIdGAMyRed")]
        public IActionResult getContactoByIdGAMyRed(Dg_contacto_red_GAM anun)
        {
            var contacto = _contactoService.getContactoByIdGAMyRed(anun.id_contactodigital, anun.Id_red);
            return Ok(contacto);
        }
    }
}
