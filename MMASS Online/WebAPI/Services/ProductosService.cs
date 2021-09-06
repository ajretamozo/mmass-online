using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IProductoService
    {
        //Empresa GetAgencia(string username, string password);
        IEnumerable<Producto> getProductosPorAnunciante(int IdAnunciante);
        IEnumerable<Producto> getAll();

    }

    public class ProductoService : IProductoService
    {
        
        public IEnumerable<Producto> getProductosPorAnunciante(int IdAnunciante)
        {
            //System.Threading.Thread.Sleep(10000); 
            return Producto.GetProductosPorAnunciante(IdAnunciante);
        }
        public IEnumerable<Producto> getAll()
        {
            return Producto.GetAll();
        }
    }
}