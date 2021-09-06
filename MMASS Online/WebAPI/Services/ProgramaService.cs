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
    public interface IProgramaService
    {
        Programa getById(int Id_p);
        IEnumerable<Programa> getAllProgramasMedio(string listaMedios);
    }

    public class ProgramaService : IProgramaService
    {

        public Programa getById(int Id_p)
        {
            return Programa.getById(Id_p);
        }

        public IEnumerable<Programa> getAllProgramasMedio(string listaMedios)
        {
            List<Programa> listaProgramas = new List<Programa>();
            var mediosRadio = Dg_medios_asociados.getAllmediosAsociados(listaMedios);
            foreach (Dg_medios_asociados medio in mediosRadio)
            {
                List<Programa> lista = Programa.getAllProgramasMedio(medio.Id_medio_asociado);
                listaProgramas = listaProgramas.Concat(lista).ToList();
            }

            return listaProgramas;
        }

    }
}
