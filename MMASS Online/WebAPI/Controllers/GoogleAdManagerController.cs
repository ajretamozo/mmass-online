using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GoogleAdManagerController : ControllerBase
    {
        private IGoogleAdManagerService _GoogleAdManagerService;

        public GoogleAdManagerController(IGoogleAdManagerService GoogleAdManagerService)
        {
            _GoogleAdManagerService = GoogleAdManagerService;
        }


        [HttpPost("getAnunciantes")]
        public IActionResult GetAnunciantes([FromBody] Parametro nombre)
        {
            var contactos = _GoogleAdManagerService.GetAnunciantes(nombre);
            return Ok(contactos);
        }

        [HttpPost("getOrderDetails")]
        public IActionResult GetOrderDetails([FromBody] long idGAM)
        {
            var res = _GoogleAdManagerService.GetOrderDetails(idGAM);
            return Ok(JsonConvert.SerializeObject(res));
        }

        //[HttpPost("getOrder")]
        //public IActionResult GetOrder([FromBody] long idGAM)
        //{
        //    var res = _GoogleAdManagerService.GetOrder(idGAM);
        //    return Ok(JsonConvert.SerializeObject(res));
        //}

        [HttpPost("getOrderById")]
        public IActionResult GetOrderById([FromBody] long idGAM)
        {
            var res = _GoogleAdManagerService.GetOrderById(idGAM);
            return Ok(res);
        }

        [HttpPost("getOrderListDetails2")]
        public IActionResult GetOrderDetails([FromBody] Dg_orden_pub_ap order)
        {
            var res = _GoogleAdManagerService.GetOrderDetails2(order);
            return Ok(JsonConvert.SerializeObject(res));
        }

        //AGREGUE:
        [HttpPost("createOrder")]
        public IActionResult CreateOrder([FromBody] long idOrden)
        {
            var res = _GoogleAdManagerService.CreateOrder(idOrden);
            return Ok(res);

        }
        //AGREGUE:
        [HttpPost("createLineItems")]
        public IActionResult CreateLineItems(Dg_orden_pub_as det)
        {
            var res = _GoogleAdManagerService.CreateLineItems(det);
            return Ok(res);

        }
        //AGREGUE:
        [HttpPost("runAdExchangeReport")]
        public void RunAdExchangeReport()
        {
           _GoogleAdManagerService.RunAdExchangeReport();
        }
        //TEST
        [HttpPost("reporteTest")]
        public void reporteTest()
        {
            _GoogleAdManagerService.reporteTest();
        }

        [HttpPost("GetLineItemCreatives")]
        public IActionResult GetLineItemCreatives([FromBody] long lineItemId)
        {
            var res = _GoogleAdManagerService.GetLineItemCreatives(lineItemId);
            return Ok(res);

        }
        //AGREGUE:
        //[HttpPost("getEmplazamientos")]
        //public IActionResult GetEmplazamientos([FromBody] string redGAM)
        //{
        //    var emplazamientos = _GoogleAdManagerService.GetEmplazamientos(redGAM);
        //    return Ok(emplazamientos);
        //}

        [HttpPost("getEmplazamientos")]
        public IActionResult GetEmplazamientos()
        {
            var emplazamientos = _GoogleAdManagerService.GetEmplazamientos();
            return Ok(emplazamientos);
        }

        [HttpPost("getMedidas")]
        public IActionResult GetMedidas()
        {
            var medidas = _GoogleAdManagerService.GetMedidas();
            return Ok(medidas);
        }

        [HttpPost("getMedidasTodasRedes")]
        public IActionResult GetMedidasTodasRedes(ListaParametro parametros)
        {
            var res = _GoogleAdManagerService.GetMedidasTodasRedes(parametros.Parametros);
            return Ok(res);
        }

        [HttpPost("archivarLineItems")]
        public IActionResult ArchivarLineItems(Dg_orden_pub_ap op)
        {
            var res = _GoogleAdManagerService.ArchivarLineItems(op);
            return Ok(res);
        }

        [HttpPost("cambiarRed")]
        public void cambiarRed([FromBody] string redGAM)
        {
            _GoogleAdManagerService.CambiarRed(redGAM);
        }

        [HttpPost("getRedActual")]
        public IActionResult getRedActual()
        {
            var cod = _GoogleAdManagerService.GetRedActual();
            return Ok(cod);
        }

        [HttpPost("getOpNuevas")]
        public IActionResult getOpNuevas(ListaParametro parametros)
        {
            var ordenes = _GoogleAdManagerService.GetOpNuevas(parametros.Parametros);
            return Ok(ordenes);
        }

        [HttpPost("comprobarModificaciones")]
        public IActionResult ComprobarModificaciones([FromBody] Dg_orden_pub_ap order)
        {
            var res = _GoogleAdManagerService.ComprobarModificaciones(order);
            return Ok(res);
        }

        [HttpPost("comprobarModificacionesD")]
        public IActionResult ComprobarModificacionesD([FromBody] long idGam)
        {
            var res = _GoogleAdManagerService.ComprobarModificacionesD(idGam);
            return Ok(res);
        }

        [HttpPost("obtenerProgresoLineasGam")]
        public IActionResult obtenerProgresoLineasGam([FromBody] Dg_orden_pub_ap order)
        {
            var res = _GoogleAdManagerService.obtenerProgresoLineasGam(order);
            return Ok(res);
        }
    }
}
