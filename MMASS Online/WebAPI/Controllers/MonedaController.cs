using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MonedaController : ControllerBase
    {
        private IMonedaService _monedaService;

        public MonedaController(IMonedaService monedaService)
        {
            _monedaService = monedaService;
        }

        [HttpPost("getMonedaBase")]
        public IActionResult getMonedaBase()
        {
            var moneda = _monedaService.getMonedaBase();
            return Ok(moneda);
        }

        [HttpPost("getAllMonedas")]
        public IActionResult getAllMonedas()
        {
            var monedas = _monedaService.getAllMonedas();
            return Ok(monedas);
        }
    }
}
