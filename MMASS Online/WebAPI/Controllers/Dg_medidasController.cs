using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Dg_medidasController : ControllerBase
    {
        private IDg_medidasService _dg_MedidasService;

        public Dg_medidasController(IDg_medidasService Dg_medidasService)
        {
            _dg_MedidasService = Dg_medidasService;
        }

        //[HttpPost("getAll")]
        //public IActionResult getAll()
        //{
        //    var dg_medidas = _dg_MedidasService.getAll();
        //    return Ok(dg_medidas);
        //}

        [HttpPost("getById")]
        public IActionResult getById(Dg_medidas miobj)
        {
            var dg_medidas = _dg_MedidasService.getById(miobj.Id_medidadigital);
            return Ok(dg_medidas);
        }
    }
}
