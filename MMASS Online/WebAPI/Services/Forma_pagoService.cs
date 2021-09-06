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
    public interface IForma_PagoService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Forma_Pago> GetAll();
        
        
    }

    public class Forma_PagoService : IForma_PagoService
    {
        
        public IEnumerable<Forma_Pago> GetAll()
        {
            //System.Threading.Thread.Sleep(10000); 
            return Forma_Pago.getAll();
        }

    }
}