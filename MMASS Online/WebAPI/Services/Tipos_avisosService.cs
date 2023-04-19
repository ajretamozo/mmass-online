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
    public interface ITipos_avisosService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Dg_tipos_avisos> getAll();
        bool saveTipo_aviso(Dg_tipos_avisos miobj);
        bool removeTipo_aviso(Dg_tipos_avisos miobj);
        Dg_tipos_avisos getById(int Id);
        IEnumerable<Dg_tipos_avisos> filter(string descripcion);
        IEnumerable<Dg_tipos_avisos> getByTipoAds(int tipoAds);
    }

    public class Tipos_avisosService : ITipos_avisosService
    {
        
        public IEnumerable<Dg_tipos_avisos> getAll()
        {
            return Dg_tipos_avisos.getAll();
        }
        public bool saveTipo_aviso(Dg_tipos_avisos miobj)
        {            
            return miobj.save();
        }
        public bool removeTipo_aviso(Dg_tipos_avisos miobj)
        {
            return miobj.remove();
        }
        public Dg_tipos_avisos getById(int Id)
        {
            return Dg_tipos_avisos.getById(Id);
        }
        public IEnumerable<Dg_tipos_avisos> filter(string descripcion)
        {
            return Dg_tipos_avisos.filter(descripcion);
        }

        public IEnumerable<Dg_tipos_avisos> getByTipoAds(int tipoAds)
        {
            return Dg_tipos_avisos.getByTipoAds(tipoAds);
        }

    }
}