using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;


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
        public IActionResult GetAnunciantes(ListaParametro parametros)
        {
            var contactos = _GoogleAdManagerService.GetAnunciantes(parametros.Parametros);
            return Ok(contactos);
        }


        [HttpPost("getOrderDetails")]
        public IActionResult GetOrderDetails([FromBody] int idOrden)
        {
            var res = _GoogleAdManagerService.GetOrderDetails(idOrden);
            return Ok(JsonConvert.SerializeObject(res));
        }

        //[HttpPost("getOrder")]
        //public IActionResult GetOrder([FromBody] long idGAM)
        //{
        //    var res = _GoogleAdManagerService.GetOrder(idGAM);
        //    return Ok(JsonConvert.SerializeObject(res));
        //}

        //[HttpPost("getOrderById")]
        //public IActionResult GetOrderById([FromBody] long idGAM)
        //{
        //    var res = _GoogleAdManagerService.GetOrderById(idGAM);
        //    return Ok(res);
        //}

        [HttpPost("getOrderListDetails2")]
        public IActionResult GetOrderDetails([FromBody] Dg_orden_pub_ap order)
        {
            var res = _GoogleAdManagerService.GetOrderDetails2(order);
            return Ok(JsonConvert.SerializeObject(res));
        }

        [HttpPost("printCertExcel")]
        public IActionResult printCertExcel([FromBody] Dg_orden_pub_ap order)
        {
            var res = _GoogleAdManagerService.printCertExcel(order);
            return Ok(JsonConvert.SerializeObject(res));
        }

        //V2
        //[HttpPost("createOrder")]
        //public IActionResult CreateOrder([FromBody] long idOrden)
        //{
        //    var res = _GoogleAdManagerService.CreateOrder(idOrden);
        //    return Ok(res);

        //}

        //V3
        [HttpPost("createOrder")]
        public IActionResult CreateOrder(ListaParametro parametros)
        {
            var res = _GoogleAdManagerService.CreateOrder(parametros.Parametros);
            return Ok(res);

        }

        [HttpPost("createLineItems")]
        public IActionResult CreateLineItems(Dg_orden_pub_as det)
        {
            var res = _GoogleAdManagerService.CreateLineItems(det);
            return Ok(res);

        }

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

        [HttpPost("getEmplazamientos")]
        public IActionResult GetEmplazamientos([FromBody] string codRed)
        {
            var emplazamientos = _GoogleAdManagerService.GetEmplazamientos(codRed);
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

        [HttpPost("getMedidasVideoTodasRedes")]
        public IActionResult GetMedidasVideoTodasRedes(ListaParametro parametros)
        {
            var res = _GoogleAdManagerService.GetMedidasVideoTodasRedes(parametros.Parametros);
            return Ok(res);
        }

        [HttpPost("archivarLineItems")]
        public IActionResult ArchivarPausarLineItems(Dg_orden_pub_ap op)
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
        
        [HttpPost("filtrarPedidos")]
        public IActionResult filtrarPedidos(ListaParametro parametros)
        {
            var ordenes = _GoogleAdManagerService.FiltrarPedidos(parametros.Parametros);
            return Ok(ordenes);
        }

        [HttpPost("comprobarModificacionesD")]
        public IActionResult ComprobarModificacionesD([FromBody] Dg_orden_pub_ap order)
        {
            var res = _GoogleAdManagerService.ComprobarModificacionesD(order);
            return Ok(res);
        }

        [HttpPost("ComprobarModificacionesDSincro")]
        public IActionResult ComprobarModificacionesDSincro(ListaParametro parametros)
        {
            var detalles = _GoogleAdManagerService.ComprobarModificacionesDSincro(parametros.Parametros);
            return Ok(detalles);
        }

        [HttpPost("obtenerProgresoLineasGam")]
        public IActionResult obtenerProgresoLineasGam([FromBody] Dg_orden_pub_ap order)
        {
            var res = _GoogleAdManagerService.obtenerProgresoLineasGam(order);
            return Ok(res);
        }

        [HttpPost("getDetNuevos")]
        public IActionResult getDetNuevos(ListaParametro parametros)
        {
            var detalles = _GoogleAdManagerService.GetDetNuevos(parametros.Parametros);
            return Ok(detalles);
        }

        [HttpPost("getMailCta")]
        public IActionResult getMailCta()
        {
            var mail = _GoogleAdManagerService.getMailCta();
            return Ok(mail);
        }

        [HttpPost("saveMail")]
        public IActionResult saveMail(Mail mail)
        {
            var res = _GoogleAdManagerService.saveMail(mail);
            return Ok(res);
        }

        [HttpPost("pruebaMail")]
        public IActionResult pruebaMail()
        {
            var resp = _GoogleAdManagerService.pruebaMail();
            return Ok(JsonConvert.SerializeObject(resp));
        }

        [HttpPost("checkAnunciantePedido")]
        public IActionResult checkAnunciantePedido([FromBody] Dg_orden_pub_as det)
        {
            var contacto = _GoogleAdManagerService.checkAnunciantePedido(det);
            return Ok(JsonConvert.SerializeObject(contacto));
        }

        [HttpPost("addLineItemToOrder")]
        public IActionResult getLineItem([FromBody] Dg_orden_pub_as detalle)
        {
            var orden = _GoogleAdManagerService.addLineItemToOrder(detalle);
            return Ok(orden);
        }
    }
}
