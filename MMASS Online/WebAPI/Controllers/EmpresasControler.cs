using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmpresasController : ControllerBase
    {
        private IEmpresaService _empresaService;

        public EmpresasController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        
        [HttpPost("getAll")]
        public IActionResult GeAll()
        {
            var contactos =  _empresaService.getAll();
            return Ok(contactos);
        }

        

    }
}
