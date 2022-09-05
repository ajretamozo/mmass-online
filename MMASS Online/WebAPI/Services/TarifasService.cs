using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public interface ITarifaService
    {
        //Contacto GetAgencia(string username, string password);
        Dg_tarifas getById(int Id);
        IEnumerable<Dg_tarifas> getAll();
        int saveTarifa(Dg_tarifas miobj);
        bool removeTarifa(Dg_tarifas miobj);
        IEnumerable<Dg_tarifa_forma_uso> getFormasUso();
        IEnumerable<Dg_tarifas> filter(List<Parametro> parametros);
    }

    public class TarifaService : ITarifaService
    {
        
        public Dg_tarifas getById(int Id)
        {
            return Dg_tarifas.getById(Id);
        }

        public IEnumerable<Dg_tarifas> getAll()
        {
            return Dg_tarifas.getAll();
        }
        public int saveTarifa(Dg_tarifas miobj)
        {
            return miobj.save();
        }

        public bool removeTarifa(Dg_tarifas miobj)
        {
            return miobj.remove();
        }

        public IEnumerable<Dg_tarifa_forma_uso> getFormasUso()
        {
            return Dg_tarifa_forma_uso.getAll();
        }

        public IEnumerable<Dg_tarifas> filter(List<Parametro> parametros)
        {
            return Dg_tarifas.filter(parametros);
        }
    }
}