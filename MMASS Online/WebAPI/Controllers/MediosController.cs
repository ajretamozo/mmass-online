using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MediosController : ControllerBase
    {
        private IMedioService _medioService;

        public MediosController(IMedioService medioService)
        {
            _medioService = medioService;
        }

        [HttpPost("getAll")]
        public IActionResult getAll()
        {
            var medios =  _medioService.getAll();
            return Ok(medios);
        }

        [HttpPost("getPorEmpresa")]
        public IActionResult getPorEmpresa([FromBody] int idEmp)
        {
            var medios = _medioService.getPorEmpresa(idEmp);
            return Ok(medios);
        }

        //AGREGUE (getallm, getalla, getalle):
        [HttpPost("getAllM")]
        public IActionResult getAllM()
        {
            var medidas = _medioService.getAllM();
            return Ok(medidas);
        }

        [HttpPost("getAllA")]
        public IActionResult getAllA()
        {
            var areasGeo = _medioService.getAllA();
            return Ok(areasGeo);
        }

        [HttpPost("getAllE")]
        public IActionResult getAllE([FromBody] int redGAM)
        {
            var emplazamientos = _medioService.getAllE(redGAM);
            return Ok(emplazamientos);
        }

        [HttpPost("saveE")]
        public IActionResult saveE(EmplazamientosList miobj)
        {
            var emp = _medioService.saveE(miobj);
            return Ok(emp);
        }

        [HttpPost("saveMedidas")]
        public IActionResult saveMedidas(Dg_medidas miobj)
        {
            var med = _medioService.saveMedidas(miobj);
            return Ok(med);
        }

        [HttpPost("getAllConvenios")]
        public IActionResult getAllConvenios()
        {
            var convenios = _medioService.getAllConvenios();
            return Ok(convenios);
        }

        [HttpPost("getConvenioById")]
        public IActionResult getConvenioById([FromBody] int Id)
        {
            var convenio = _medioService.getConvenioById(Id);
            return Ok(convenio);
        }

        [HttpPost("filter")]
        public IActionResult filter(ListaParametro parametros)
        {
            var fu = _medioService.filter(parametros.Parametros);
            return Ok(fu);
        }

        [HttpPost("getDetConvenioById")]
        public IActionResult getDetConvenioById(Conv_dg_detalle detConv)
        {
            var detConvenio = _medioService.getDetConvenioById(detConv.Id_convenio, detConv.Id_detalle);
            return Ok(detConvenio);
        }

        [HttpPost("getDetConveniosByIdConv")]
        public IActionResult getDetConveniosByIdConv([FromBody] int IdConv)
        {
            var detConvenios = _medioService.getDetConveniosByIdConv(IdConv);
            return Ok(detConvenios);
        }

        [HttpPost("getAllRedes")]
        public IActionResult getAllRedes()
        {
            var redes = _medioService.getAllRedes();
            return Ok(redes);
        }

        [HttpPost("getRedByCodigo")]
        public IActionResult getRedByCodigo([FromBody] long netCode)
        {
            var red = _medioService.getRedByCodigo(netCode);
            return Ok(red);
        }

        [HttpPost("getRedById")]
        public IActionResult getRedById([FromBody] int id)
        {
            var red = _medioService.getRedById(id);
            return Ok(red);
        }

        [HttpPost("getCodigosRed")]
        public IActionResult getCodigosRed()
        {
            var redes = _medioService.getCodigosRed();
            return Ok(redes);
        }

        [HttpPost("removeRed")]
        public IActionResult remove(Dg_red_GAM miobj)
        {
            var redGam = _medioService.deleteRed(miobj);
            return Ok(redGam);
        }

        [HttpPost("saveRed")]
        public IActionResult save(Dg_red_GAM miobj)
        {
            var redGam = _medioService.saveRed(miobj);
            return Ok(redGam);
        }

        [HttpPost("filterRedes")]
        public IActionResult filterRedes(ListaParametro parametros)
        {

            var fu = _medioService.filterRedes(parametros.Parametros);
            return Ok(fu);
        }

        [HttpPost("getAllConceptos")]
        public IActionResult getAllConceptos()
        {
            var conceptos = _medioService.getAllConceptos();
            return Ok(conceptos);
        }

    }
}
