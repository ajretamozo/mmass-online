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
    public interface IArchivosService
    {
        IEnumerable<Archivo> getArchivosPorOP(int id_op);        
        bool deleteFile(int id_file);
        int save(Archivo file);
    }

    public class ArchivosService : IArchivosService
    {

        public IEnumerable<Archivo> getArchivosPorOP(int id_op)
        {            
            return Archivo.getArchivosPorOP(id_op);
        }

        public bool deleteFile(int id_file)
        {
            return Archivo.deleteFile(id_file);
        }
        public int save(Archivo file)
        {
            return file.save();
        }

    }
}