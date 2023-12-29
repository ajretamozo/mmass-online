using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;


namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Orden_presup_apsController : ControllerBase
    {
        private IOrden_presup_apService _Orden_presup_apService;

        public Orden_presup_apsController(IOrden_presup_apService Orden_presup_apService)
        {
            _Orden_presup_apService = Orden_presup_apService;
        }

        [HttpPost("save")]
        public IActionResult save(Orden_presup_ap presup)
        {
            var result = _Orden_presup_apService.save(presup);
            return Ok(result);
        }
        [HttpPost("getById")]
        public IActionResult getById(Orden_presup_ap presup)
        {
            var result = _Orden_presup_apService.getById(presup.Id_presup);
            return Ok(result);
        }
        [HttpPost("getAll")]
        public IActionResult getAll()
        {
            var result = _Orden_presup_apService.getAll();
            return Ok(result);
        }
        [HttpPost("filter")]
        public IActionResult filter(ListaParametro parametros)
        {
            var fu = _Orden_presup_apService.filter(parametros.Parametros);
            return Ok(fu);
        }

        //[HttpPost("bloquearOp")]
        //public IActionResult bloquearOp(Dg_orden_pub_bloqueo opb)
        //{
        //    var result = _Dg_orden_pub_apService.bloquearOP(opb);
        //    return Ok(result);
        //}

        //[HttpPost("desbloquearOp")]
        //public IActionResult desbloquearOp(Dg_orden_pub_bloqueo opb)
        //{
        //    var result = _Dg_orden_pub_apService.desbloquearOP(opb);
        //    return Ok(result);
        //}

        [HttpPost("existePresupNombre")]
        public IActionResult existePresupNombre(Orden_presup_ap presup)
        {
            var result = _Orden_presup_apService.existePresupNombre(presup.Id_presup, presup.Descripcion);
            return Ok(result);
        }

        //[HttpPost("desbloquearTodas")]
        //public IActionResult desbloquearTodas([FromBody] int idUsuario)
        //{
        //    var result = _Dg_orden_pub_apService.desbloquearTodas(idUsuario);
        //    return Ok(result);
        //}

        [HttpPost("grabarLog")]
        public void grabarLog(ListaParametro parametros)
        {
            _Orden_presup_apService.grabarLog(parametros.Parametros);
        }

    }
}
