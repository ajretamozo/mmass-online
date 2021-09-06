using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ArchivosController : ControllerBase
    {
        private IArchivosService _archivoService;

        public ArchivosController(IArchivosService archivoService)
        {
            _archivoService = archivoService;
        }

        [HttpPost("getArchivosPorOP")]
        public IActionResult getArchivosPorOP([FromBody] int id_op)
        {
            var archivos = _archivoService.getArchivosPorOP(id_op);
            return Ok(archivos);
        }

        [HttpPost("printDir")]
        public IActionResult printDir([FromBody] int id_op)
        {
            //var res = "";
            //var folder1 = System.IO.Directory.GetCurrentDirectory();
            //var parent1 = System.IO.Directory.GetParent(folder1);
            //var uploads1 = Path.Combine(parent1.FullName, "MMASSOnline", "files");
            //var folder2 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //var parent2 = System.IO.Directory.GetParent(folder2);
            //var uploads2 = Path.Combine(parent2.FullName, "MMASSOnline", "files");
            //var folder3 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //var parent3 = System.IO.Directory.GetParent(folder3);
            //var uploads3 = Path.Combine(parent3.FullName, "MMASSOnline", "files");
            //res += "Folder 1: " + folder1 + ", parent1: " + parent1 + "uploads1: " + uploads1 + " /// ";
            //res += "Folder 2: " + folder2 + ", parent2: " + parent2 + "uploads2: " + uploads2 + " /// ";
            //res += "Folder 3: " + folder3 + ", parent3: " + parent3 + "uploads3: " + uploads3 + " /// ";
            var res = new Dictionary<string, string>();
            res.Add("FRONTEND FOLDER", Startup.StaticConfig.GetSection("AppSettings").GetSection("FrontendFolder").Value);
            return Ok(res);
        }

        [HttpPost("delete")]        
        public IActionResult deleteFile([FromBody] string filename)
        {
            var folder = System.IO.Directory.GetCurrentDirectory();
            var parent = System.IO.Directory.GetParent(folder);
            var path = Path.Combine(parent.FullName, Startup.StaticConfig.GetSection("AppSettings").GetSection("FrontendFolder").Value, "files" ,filename);
            if ((System.IO.File.Exists(path)))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Deletion of file failed: " + ex.Message);
                }
            }
            char[] separator = {'.'};
            var nameExt = filename.Split(separator);
            var id_file = int.Parse(nameExt[0]);
            //falta el archivo en la db
            var delete = _archivoService.deleteFile(id_file);
            return Ok(delete);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [EnableCors("SiteCorsPolicy")]
        public IActionResult UploadImage([FromForm(Name = "archivo")] IFormFile file, [FromForm(Name = "id_op")] int id_op)
        {
            //fileExt=extension del archivo
            char[] separator = { '.' };
            var nameExt = file.FileName.Split(separator);
            var fileExt = nameExt[1];
            //guardo en la db
            Archivo objFile = new Archivo();
            objFile.Nombre = file.FileName;
            objFile.Id_op = id_op;
            var prox = objFile.save();
            if (prox != 0)
            {
                var folder = System.IO.Directory.GetCurrentDirectory();
                //var folder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var parent = System.IO.Directory.GetParent(folder);
                var uploads = Path.Combine(parent.FullName, Startup.StaticConfig.GetSection("AppSettings").GetSection("FrontendFolder").Value, "files");
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, prox.ToString() + "." + fileExt);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                //extraigo la extension del archivo

                //falta guardar el archivo en la db
                return Ok(file);
            }
            return Ok();
        }
    }
}
