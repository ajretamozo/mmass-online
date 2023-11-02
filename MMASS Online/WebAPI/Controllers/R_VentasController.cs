﻿using Microsoft.AspNetCore.Mvc;
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
    public class R_VentasController : ControllerBase
    {
        private IR_VentasService _r_ventasService;

        public R_VentasController(IR_VentasService r_ventasService)
        {
            _r_ventasService = r_ventasService;
        }

        [HttpPost("filterBy")]
        public IActionResult filterBy(ListaParametro parametros)
        {
            var lista = _r_ventasService.filterBy(parametros.Parametros);
            return Ok(lista);
        }

        [HttpPost("getDatosHome")]
        public IActionResult getDatosHome(ListaParametro parametros)
        {
            var lista = _r_ventasService.getDatosHome(parametros.Parametros);
            return Ok(lista);
        }

        [HttpPost("getRankingsHome")]
        public IActionResult getRankingsHome(ListaParametro parametros)
        {
            var lista = _r_ventasService.getRankingsHome(parametros.Parametros);
            return Ok(lista);
        }

        //[HttpPost("getRankingMedios")]
        //public IActionResult getRankingMedios(ListaParametro parametros)
        //{
        //    var lista = _r_ventasService.getRankingMedios(parametros.Parametros);
        //    return Ok(lista);
        //}

    }
}
