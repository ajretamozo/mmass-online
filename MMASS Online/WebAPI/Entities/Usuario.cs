using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Usuario
    {
        public int Id_usuario { get; set; }
        public string Nombre { get; set; }
        public string Nombrel { get; set; }
        public int Tipodoc { get; set; }
        public string Nrodoc { get; set; }
        public DateTime ? F_alta { get; set; }
        public DateTime ? F_baja { get; set; }
        public string Clave { get; set; }
        public string Clave_web { get; set; }
        public string Email { get; set; }
        public string Adusuario { get; set; }
        public int Ambito { get; set; }
        public int Usrrol { get; set; }
        public string Idioma { get; set; }
        public int Sistema { get; set; }
        public static Usuario getById(int Id)
        {
            string sqlCommand = " select id_usuario, nombre, nombrel, tipodoc, nrodoc, f_alta, f_baja, clave, clave_web, email, adusuario, Ambito, " +
                                " usrrol, idioma, sistema from usuarios where id_usuario = " + Id.ToString();
            Usuario resultado = null;

            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = new Usuario();
                resultado.Id_usuario = int.Parse(t.Rows[0]["id_usuario"].ToString());
                resultado.Nombre = t.Rows[0]["nombre"].ToString();
                resultado.Nombrel = t.Rows[0]["nombrel"].ToString();
                resultado.Tipodoc = int.Parse(t.Rows[0]["tipodoc"].ToString());
                resultado.Nrodoc = t.Rows[0]["nrodoc"].ToString();
                resultado.F_alta = DB.DFecha(t.Rows[0]["f_alta"].ToString());
                resultado.F_baja = DB.DFecha (t.Rows[0]["f_baja"].ToString());
                resultado.Clave = t.Rows[0]["clave"].ToString();
                resultado.Clave_web = t.Rows[0]["clave_web"].ToString();
                resultado.Email = t.Rows[0]["email"].ToString();
                resultado.Adusuario = t.Rows[0]["adusuario"].ToString();
                resultado.Ambito = DB.DInt(t.Rows[0]["Ambito"].ToString());
                resultado.Usrrol = DB.DInt(t.Rows[0]["usrrol"].ToString());
                resultado.Idioma = t.Rows[0]["idioma"].ToString();
                resultado.Sistema = DB.DInt(t.Rows[0]["sistema"].ToString());
            }
            return resultado;
        }

        public static Usuario getByNombre(string  pNombre)
        {
            string sqlCommand = " select id_usuario, nombre, nombrel, tipodoc, nrodoc, f_alta, f_baja, clave, clave_web, email, adusuario, Ambito, " +
                                " usrrol, idioma, sistema from usuarios where nombre = '" + pNombre+"'";
            Usuario resultado= null;
           
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = new Usuario();
                resultado.Id_usuario = int.Parse(t.Rows[0]["id_usuario"].ToString());
                resultado.Nombre = t.Rows[0]["nombre"].ToString();
                resultado.Nombrel = t.Rows[0]["nombrel"].ToString();
                resultado.Tipodoc = int.Parse(t.Rows[0]["tipodoc"].ToString());
                resultado.Nrodoc = t.Rows[0]["nrodoc"].ToString();
                resultado.F_alta = DB.DFecha(t.Rows[0]["f_alta"].ToString());
                resultado.F_baja = DB.DFecha(t.Rows[0]["f_baja"].ToString());
                resultado.Clave = t.Rows[0]["clave"].ToString();
                resultado.Clave_web = t.Rows[0]["clave_web"].ToString();
                resultado.Email = t.Rows[0]["email"].ToString();
                resultado.Adusuario = t.Rows[0]["adusuario"].ToString();
                resultado.Ambito = DB.DInt(t.Rows[0]["Ambito"].ToString());
                resultado.Usrrol = DB.DInt(t.Rows[0]["usrrol"].ToString());
                resultado.Idioma = t.Rows[0]["idioma"].ToString();
                resultado.Sistema = DB.DInt(t.Rows[0]["sistema"].ToString());
            }
            return resultado;
        }


    }
}
