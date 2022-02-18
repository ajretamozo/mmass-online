using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

namespace WebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {
        private IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        
        [HttpPost("getProductosPorAnunciante")]
        public IActionResult getProductosPorAnunciante([FromBody]int idAnunciante)
        {
            var productos = _productoService.getProductosPorAnunciante(idAnunciante);
            return Ok(productos);
        }

        [HttpPost("getAll")]
        public IActionResult getAll()
        {
            var productos = _productoService.getAll();
            return Ok(productos);
        }

        [HttpPost("getProductoById")]
        public IActionResult getProductoById([FromBody]int id)
        {
            var producto = _productoService.getProductoById(id);
            return Ok(producto);
        }

    }
}
