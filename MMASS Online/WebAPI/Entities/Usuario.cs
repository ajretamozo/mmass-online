using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;

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

        public static Usuario getUsuario(DataRow item)
        {
            Usuario miUsuario = new Usuario
            {
                Id_usuario = int.Parse(item["id_usuario"].ToString()),
                Nombre = item["nombre"].ToString(),
                Nombrel = item["nombrel"].ToString(),
                Tipodoc = int.Parse(item["tipodoc"].ToString()),
                Nrodoc = item["nrodoc"].ToString(),
                F_alta = DB.DFecha(item["f_alta"].ToString()),
                F_baja = DB.DFecha(item["f_baja"].ToString()),
                Clave = item["clave"].ToString(),
                Clave_web = item["clave_web"].ToString(),
                Email = item["email"].ToString(),
                Adusuario = item["adusuario"].ToString(),
                Ambito = DB.DInt(item["Ambito"].ToString()),
                Usrrol = DB.DInt(item["usrrol"].ToString()),
                Idioma = item["idioma"].ToString(),
                Sistema = DB.DInt(item["sistema"].ToString())
            };
            return miUsuario;
        }

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
                                " usrrol, idioma, sistema from usuarios where nombre = '" + pNombre+ "' and (f_baja is null or f_baja = '') ";
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

        public int save()
        {
            int respuesta = 0;
            string sql = "";
            F_alta = DateTime.Now;
            if (Id_usuario == 0)
            {
                if (existeUser(Nombre) == true)
                {
                    respuesta = 1;
                    return respuesta;
                }
                else
                {
                    string sqlId = "select max(id_usuario) as maximo from usuarios";
                    int nuevoId = 0;
                    DataTable t = DB.Select(sqlId);

                    if (t.Rows.Count == 1)
                    {
                        nuevoId = DB.DInt(t.Rows[0]["maximo"].ToString());
                        nuevoId++;
                        Id_usuario = nuevoId;
                    }

                    sql = "insert into usuarios (id_usuario, nombre, nombrel, tipodoc, nrodoc, f_alta, clave, clave_web, email, adusuario, usrrol)" +
                                       " values (@id_usuario, @nombre, @nombrel, 0, '', @f_alta, '', @clave_web, '', '', 0)";
                }
            }

            else
            {
                sql = "update usuarios set nombre = @nombre, nombrel = @nombrel, clave_web = @clave_web where id_usuario = @id_usuario";
            }

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@id_usuario", SqlDbType = SqlDbType.Int, Value = Id_usuario },
                new SqlParameter()
                { ParameterName="@nombre",SqlDbType = SqlDbType.VarChar, Value = Nombre },
                new SqlParameter()
                { ParameterName="@nombrel",SqlDbType = SqlDbType.VarChar, Value = Nombrel },
                new SqlParameter()
                { ParameterName="@clave_web", SqlDbType = SqlDbType.VarChar, Value = Clave_web },
                 new SqlParameter()
                { ParameterName="@f_alta", SqlDbType = SqlDbType.DateTime, Value = F_alta }
            };
            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = 2;
                return respuesta;
            }
            return respuesta;
        }

        public static List<Usuario> getAllUsers()
        {
            string sqlCommand = " select id_usuario, nombre, nombrel, tipodoc, nrodoc, f_alta, f_baja, clave, clave_web, email, adusuario, Ambito, " +
                                " usrrol, idioma, sistema from usuarios where clave_web is not null and clave_web != '' and (f_baja is null or f_baja = '')";

            List<Usuario> col = new List<Usuario>();
            Usuario user;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                user = getUsuario(item);
                col.Add(user);
            }
            return col;
        }

        public static List<Usuario> getByNombreList(string pNombre)
        {
            string sqlCommand = " select id_usuario, nombre, nombrel, tipodoc, nrodoc, f_alta, f_baja, clave, clave_web, email, adusuario, Ambito, " +
                                " usrrol, idioma, sistema from usuarios where clave_web is not null and nombre like '%" + pNombre + "%' and (f_baja is null or f_baja = '')";
            List<Usuario> col = new List<Usuario>();
            Usuario user;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                user = getUsuario(item);
                col.Add(user);
            }
            return col;
        }

        public bool deleteUser()
        {
            string sql = "";

            if (esUserTrafico(Id_usuario) == true)
            {
                sql = "update usuarios set clave_web = '' where id_usuario = @id_usuario";
            }
            else
            {
                sql = "update usuarios set F_BAJA = GETDATE() where id_usuario = @id_usuario";
            }
                
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@id_usuario", SqlDbType = SqlDbType.Int, Value = Id_usuario }
            };
            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        //public bool deleteUser()
        //{
        //    string sp = "eliminar_usuario";

        //    List<SqlParameter> parametros = new List<SqlParameter>()
        //    {
        //        new SqlParameter()
        //        { ParameterName="@id_us", SqlDbType = SqlDbType.Int, Value = Id_usuario }
        //    };
        //    if (Id_usuario != 0)
        //    {
        //        try
        //        {
        //            DB.ExecuteSp(sp, parametros);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public static bool cantMaxUsers()
        {
            string sqlCommand = "select count(*) as cant from usuarios where clave_web is not null and clave_web != ''";
            bool resultado = false;
            int cant = 0;

            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                cant = DB.DInt(t.Rows[0]["cant"].ToString());

                if (cant > 19)
                {
                    resultado = true;
                }
            }
            return resultado;
        }

        public static bool esUserTrafico(int idUser)
        {
            string sqlCommand = "select clave from usuarios where id_usuario = " + idUser.ToString();
            bool resultado = false;
            string clave = "";

            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                clave = t.Rows[0]["clave"].ToString();

                if (clave != "")
                {
                    resultado = true;
                }
            }
            return resultado;
        }

        public static bool existeUser(string user)
        {
            string sqlCommand = "select id_usuario from usuarios where (f_baja is null or f_baja = '') and nombre = '" + user + "'";
            bool resultado = false;

            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count > 0)
            {
                resultado = true;
            }
            return resultado;
        }

    }
}
