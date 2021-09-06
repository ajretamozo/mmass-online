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
    public interface IR_VentasService
    {
        IEnumerable<R_Ventas> filterBy(List<Parametro> parametros);
    }

    public class R_VentasService : IR_VentasService
    {
        public IEnumerable<R_Ventas> filterBy(List<Parametro> parametros)
        {
            return R_Ventas.filterBy(parametros);
        }
    }

    
}
