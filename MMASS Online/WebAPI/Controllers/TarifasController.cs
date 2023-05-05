using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TarifasController : ControllerBase
    {
        private ITarifaService _tarifaService;

        public TarifasController(ITarifaService tarifaService)
        {
            _tarifaService = tarifaService;
        }

        [HttpPost("getById")]
        public IActionResult getById(Dg_tarifas miobj)
        {
            var tarifas =  _tarifaService.getById(miobj.Id_tarifa_dg);
            return Ok(tarifas);
        }

        [HttpPost("getAll")]
        public IActionResult getAll()
        {
            var tarifas = _tarifaService.getAll();
            return Ok(tarifas);
        }

        [HttpPost("save")]
        public IActionResult saveTarifa(Dg_tarifas miobj)
        {
            var tarifas = _tarifaService.saveTarifa(miobj);
            return Ok(tarifas);
        }

        [HttpPost("remove")]
        public IActionResult removeTarifa(Dg_tarifas miobj)
        {
            var tarifas = _tarifaService.removeTarifa(miobj);
            return Ok(tarifas);
        }

        [HttpPost("getFormasUsoAll")]
        public IActionResult getFormasUsoAll()
        {
            var fu = _tarifaService.getFormasUso();
            return Ok(fu);
        }

        [HttpPost("filter")]
        public IActionResult filter(ListaParametro parametros)
        {

            var fu = _tarifaService.filter(parametros.Parametros);
            return Ok(fu);
        }
    }
}