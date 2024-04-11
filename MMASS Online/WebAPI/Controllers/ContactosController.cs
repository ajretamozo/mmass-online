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

        [HttpPost("deleteASRelation")]
        public IActionResult deleteASRelation(Contacto p)
        {
            var result = _contactoService.deleteASRelation(p.Id, p.IdContactoDigital, p.IdRed);
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

        [HttpPost("getContactoById")]
        public IActionResult getContactoById([FromBody] int id)
        {
            var contacto = _contactoService.getContactoById(id);
            return Ok(contacto);
        }

        [HttpPost("getContactoByIdyRed")]
        public IActionResult getContactoByIdyRed(Dg_contacto_red_GAM anun)
        {
            var contacto = _contactoService.getContactoByIdyRed(anun.Id_contacto, anun.Id_red);
            return Ok(contacto);
        }

        [HttpPost("getAnunSincro")]
        public IActionResult GetAnunSincro([FromBody] int idRed)
        {
            var contactos = _contactoService.GetAnunSincro(idRed);
            return Ok(contactos);
        }
        //hacer este metodo
        [HttpPost("chequearSincroContacto")]
        public IActionResult chequearSincroContacto(Dg_contacto_red_GAM anun)
        {
            var result = _contactoService.chequearSincroContacto(anun.Id_contacto, anun.Id_red);
            return Ok(result);
        }

        [HttpPost("saveCliPotencial")]
        public IActionResult saveCliPotencial(Contacto contacto)
        {
            var result = _contactoService.saveCliPotencial(contacto);
            return Ok(result);
        }

        [HttpPost("getEmailsPorContacto")]
        public IActionResult getEmailsPorContacto(Contacto contacto)
        {
            var mails = _contactoService.getEmailsPorContacto(contacto);
            return Ok(mails);
        }

        [HttpPost("saveEmail")]
        public IActionResult saveEmail(Contacto contacto)
        {
            var resp = _contactoService.saveEmail(contacto);
            return Ok(resp);
        }

        [HttpPost("deleteEmail")]
        public IActionResult deleteEmail([FromBody] int idEmail)
        {
            var contactos = _contactoService.deleteEmail(idEmail);
            return Ok(contactos);
        }

    }
}
