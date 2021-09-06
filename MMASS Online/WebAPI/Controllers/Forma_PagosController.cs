using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Forma_PagosController : ControllerBase
    {
        private IForma_PagoService  _forma_pagoService;

        public Forma_PagosController(IForma_PagoService forma_PagoService)
        {
            _forma_pagoService = forma_PagoService;
        }

        
        [HttpPost("getAll")]
        public IActionResult GetAll()
        {
            var fp =  _forma_pagoService.GetAll();
            return Ok(fp);
        }
        


    }
}
