using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IDg_areas_geoService
    {
        IEnumerable<Dg_areas_geo> getAll(); 
    }
    public class Dg_areas_geoService : IDg_areas_geoService
    {
        public IEnumerable<Dg_areas_geo> getAll()
        {
            return Dg_areas_geo.getAll();
        }

    }
}
