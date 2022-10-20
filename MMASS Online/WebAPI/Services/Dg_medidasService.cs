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
    public interface IDg_medidasService
    {
        //IEnumerable<Dg_medidas> getAll();
        Dg_medidas getById(int Id);
    }

    public class Dg_medidasService : IDg_medidasService
    {

        //public IEnumerable<Dg_medidas> getAll()
        //{
        //    return Dg_medidas.getAll();
        //}
        public Dg_medidas getById(int Id)
        {
            return Dg_medidas.getById(Id);
        }


    }
}
