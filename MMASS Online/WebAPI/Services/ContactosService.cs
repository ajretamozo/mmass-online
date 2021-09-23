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
    public interface IContactoService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Contacto> GetAgencias();
        IEnumerable<Contacto> GetAnunciantes();
        IEnumerable<Contacto> GetEjecutivos();
        IEnumerable<Contacto> GetAnunciantesPorAgencia(int IdAgencia);
        IEnumerable<Contacto> GetAgenciasPorAnunciante(int IdAnunciante);
        IEnumerable<Contacto> GetAnunciantesPorProducto(int IdProducto);
        bool saveASRelation(int Id_contacto, string Id_ContactoDigital, int Id_Red);
    }

    public class ContactoService : IContactoService
    {
        
        public IEnumerable<Contacto> GetAgencias()
        {
            //System.Threading.Thread.Sleep(10000); 
            return Contacto.GetContactosPorTipo("0");
        }
        public IEnumerable<Contacto> GetEjecutivos()
        {            
            return Contacto.GetContactosPorTipo("6");
        }


        public IEnumerable<Contacto> GetAnunciantes()
        {
            return Contacto.GetContactosPorTipo("1");
        }
        public IEnumerable<Contacto> GetAnunciantesPorAgencia(int idAgencia)
        {
            return Contacto.GetContactosPorAgencia(idAgencia);
        }

        public IEnumerable<Contacto> GetAgenciasPorAnunciante(int IdAnunciante)
        {
            return Contacto.GetAgenciasPorAnunciante(IdAnunciante);
        }

        public IEnumerable<Contacto> GetAnunciantesPorProducto(int IdProducto)
        {
            return Contacto.GetAnunciantesPorProducto(IdProducto);
        }

        public bool saveASRelation(int Id_contacto, string Id_ContactoDigital, int Id_Red)
        {
            return Contacto.saveASRelation(Id_contacto, Id_ContactoDigital, Id_Red);
        }
    }
}