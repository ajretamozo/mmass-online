using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
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
        bool deleteASRelation(int Id_contacto, string Id_ContactoDigital, int Id_Red);
        Contacto getContactoByIdGAMyRed(string idGam, int idRed);
        Contacto getContactoById(int id);
        Contacto getContactoByIdyRed(int id, int idRed);
        IEnumerable<Contacto> GetAnunSincro(int idRed);
        bool chequearSincroContacto(int idContacto, int idRed);
        Contacto saveCliPotencial(Contacto contacto);
        IEnumerable<Mail> getEmailsPorContacto(Contacto contacto);
        bool saveEmail(Contacto contacto);
        bool deleteEmail(int idEmail);
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
            return Contacto.GetContactosPorTipo("2");
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

        public bool deleteASRelation(int Id_contacto, string Id_ContactoDigital, int Id_Red)
        {
            return Contacto.deleteASRelation(Id_contacto, Id_ContactoDigital, Id_Red);
        }

        public Contacto getContactoByIdGAMyRed(string idGam, int idRed)
        {
            return Contacto.getContactoByIdGAMyRed(idGam, idRed);
        }

        public Contacto getContactoById(int id)
        {
            return Contacto.getContactoById(id);
        }

        public Contacto getContactoByIdyRed(int id, int idRed)
        {
            return Contacto.getContactoByIdyRed(id, idRed);
        }

        public IEnumerable<Contacto> GetAnunSincro(int idRed)
        {
            return Contacto.GetAnunSincro(idRed);
        }

        public bool chequearSincroContacto(int idContacto, int idRed)
        {
            return Dg_contacto_red_GAM.chequearSincroContacto(idContacto, idRed);
        }

        public Contacto saveCliPotencial(Contacto contacto)
        {
            Contacto ret = new Contacto();

            if (contacto.existeContactoNombre())
            {
                ret.Id_contacto = 0;
            }

            else
            {
                ret = contacto.saveCliPotencial();
            }

            return ret;
        }

        public IEnumerable<Mail> getEmailsPorContacto(Contacto contacto)
        {
            return contacto.GetEmailsPorContacto();
        }

        public bool saveEmail(Contacto contacto)
        {
            return contacto.saveEmail();
        }

        public bool deleteEmail(int idEmail)
        {
            return Contacto.deleteEmail(idEmail);
        }
    }
}