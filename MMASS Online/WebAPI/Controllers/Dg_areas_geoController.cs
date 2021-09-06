using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Dg_areas_geoController : ControllerBase
    {
        private IDg_areas_geoService _Dg_areas_geoService;

        public Dg_areas_geoController(IDg_areas_geoService Dg_areas_geoService)
        {
            _Dg_areas_geoService = Dg_areas_geoService;
        }

        [HttpPost("getAll")]
        public IActionResult getAll()
        {
            var areasGeo = _Dg_areas_geoService.getAll();
            return Ok(areasGeo);
        }

    }
}
