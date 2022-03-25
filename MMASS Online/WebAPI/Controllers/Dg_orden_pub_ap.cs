using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using Google.Api.Ads.AdManager.v202105;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Dg_orden_pub_apsController : ControllerBase
    {
        private IDg_orden_pub_apService _Dg_orden_pub_apService;

        public Dg_orden_pub_apsController(IDg_orden_pub_apService Dg_orden_pub_apService)
        {
            _Dg_orden_pub_apService = Dg_orden_pub_apService;
        }
        
        [HttpPost("save")]
        public IActionResult save(Dg_orden_pub_ap op)
        {
            var result = _Dg_orden_pub_apService.save(op);
            return Ok(result);
        }
        [HttpPost("getById")]
        public IActionResult getById(Dg_orden_pub_ap op)
        {
            var result = _Dg_orden_pub_apService.getById(op.Id_op_dg);
            return Ok(result);
        }
        [HttpPost("getOpRadioById")]
        public IActionResult getOpRadioById([FromBody] int id)
        {
            var result = _Dg_orden_pub_apService.getOpRadioById(id);
            return Ok(result);
        }
        [HttpPost("getAll")]
        public IActionResult getAll()
        {
            var result = _Dg_orden_pub_apService.getAll();
            return Ok(result);
        }
        [HttpPost("getAllHTML")]
        public IActionResult getAllHTML()
        {
            var result = _Dg_orden_pub_apService.getAllHTML();
            return Ok(result);
        }
        [HttpPost("getOrdenesRadio")]
        public IActionResult getOrdenesRadio(Dg_orden_pub_ap op)
        {
            var result = _Dg_orden_pub_apService.getOrdenesRadio(op.agencia.Id , op.anunciante.Id);
            return Ok(result);
        }
        [HttpPost("filter")]
        public IActionResult filter(ListaParametro parametros)
        {
            var fu = _Dg_orden_pub_apService.filter(parametros.Parametros);
            return Ok(fu);
        }

        //AGREGUE:
        [HttpPost("getSponsorsPorFecha")]
        public IActionResult getSponsorsPorFecha(Dg_orden_pub_as det)
        {
            //var result = _Dg_orden_pub_apService.getSponsorsPorFecha((System.DateTime)det.Fecha_desde, (System.DateTime)det.Fecha_hasta);
            var result = _Dg_orden_pub_apService.getSponsorsPorFecha(det);
            return Ok(result);
        }

        [HttpPost("anularOrden")]
        public IActionResult anularOrden([FromBody] int id)
        {
            var result = _Dg_orden_pub_apService.anularOrden(id);
            return Ok(result);
        }

    }
}
