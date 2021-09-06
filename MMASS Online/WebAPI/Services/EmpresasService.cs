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
    public interface IEmpresaService
    {
        //Empresa GetAgencia(string username, string password);
        IEnumerable<Empresa> getAll();        

    }

    public class EmpresaService : IEmpresaService
    {
        
        public IEnumerable<Empresa> getAll()
        {
            //System.Threading.Thread.Sleep(10000); 
            return Empresa.getAll();

        }

        public IEnumerable<Empresa> GetAnunciantes()
        {
            return Empresa.getAll();
        }
    }
}