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
    public interface IDg_medios_asociadosService
    {
        Dg_medios_asociados getById(int Id_m_dg, int Id_m_as);
        IEnumerable<Dg_medios_asociados> getAllmediosAsociados(string listaMedios);
    }

    public class Dg_medios_asociadosService: IDg_medios_asociadosService
    {
    
        public Dg_medios_asociados getById(int Id_m_dg, int Id_m_as)
        {
            return Dg_medios_asociados.getById(Id_m_dg, Id_m_as);
        }

        public IEnumerable<Dg_medios_asociados> getAllmediosAsociados(string listaMedios)
        {
            return Dg_medios_asociados.getAllmediosAsociados(listaMedios);
        }

    }
}
