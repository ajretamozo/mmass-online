﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAnunciantes([FromBody] string desc)
        {
            var contactos = _GoogleAdManagerService.GetAnunciantes(desc);
            return Ok(contactos);
        }

        [HttpPost("getOrderDetails")]
        public IActionResult GetOrderDetails([FromBody] long idGAM)
        {
            var res = _GoogleAdManagerService.GetOrderDetails(idGAM);
            return Ok(JsonConvert.SerializeObject(res));
        }

        [HttpPost("getOrder")]
        public IActionResult GetOrder([FromBody] long idGAM)
        {
            var res = _GoogleAdManagerService.GetOrder(idGAM);
            return Ok(JsonConvert.SerializeObject(res));
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
        [HttpPost("getEmplazamientos")]
        public IActionResult GetEmplazamientos([FromBody] string redGAM)
        {
            var emplazamientos = _GoogleAdManagerService.GetEmplazamientos(redGAM);
            return Ok(emplazamientos);
        }

        [HttpPost("getMedidas")]
        public IActionResult GetMedidas()
        {
            var medidas = _GoogleAdManagerService.GetMedidas();
            return Ok(medidas);
        }

        [HttpPost("archivarLineItems")]
        public IActionResult ArchivarLineItems([FromBody] long lineItemId)
        {
            var res = _GoogleAdManagerService.ArchivarLineItems(lineItemId);
            return Ok(res);
        }

    }
}