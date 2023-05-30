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
    public interface IMonedaService
    {
        Moneda getMonedaBase();
        List<Moneda> getAllMonedas();
    }

    public class MonedaService : IMonedaService
    {

        public Moneda getMonedaBase()
        {
            return Moneda.getMonedaBase();
        }

        public List<Moneda> getAllMonedas()
        {
            return Moneda.getAll();
        }

    }
}