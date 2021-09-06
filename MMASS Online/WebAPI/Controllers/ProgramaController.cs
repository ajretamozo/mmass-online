using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProgramaController : ControllerBase
    {
        private IProgramaService _programaService;

        public ProgramaController(IProgramaService programaService)
        {
            _programaService = programaService;
        }

        [HttpPost("getAllProgramasMedio")]
        public IActionResult getAllProgramasMedio([FromBody] string listaMedios)
        {
            var programa = _programaService.getAllProgramasMedio(listaMedios);
            return Ok(programa);
        }


        [HttpPost("getById")]
        public IActionResult getById(Programa miobj)
        {
            var programa = _programaService.getById(miobj.Id_programa);
            return Ok(programa);
        }


    }
}
