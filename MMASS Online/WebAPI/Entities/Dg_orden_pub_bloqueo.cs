using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;

namespace WebApi.Entities
{
    public class Dg_orden_pub_bloqueo
    {
        public int Id_op_dg { get; set; }
        public int Id_usuario { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public int Bloqueado { get; set; }
        public DateTime? UltimoDesbloqueo { get; set; }
        public int Id_usuario_desbl { get; set; }

        public static Dg_orden_pub_bloqueo getDg_orden_pub_bloqueo(DataRow item)
        {
            Dg_orden_pub_bloqueo mi = new Dg_orden_pub_bloqueo();
            mi.Id_op_dg = DB.DInt(item["Id_op_dg"].ToString());
            mi.Id_usuario = DB.DInt(item["Id_usuario"].ToString());
            mi.UltimoAcceso = DB.DFecha(item["UltimoAcceso"].ToString());
            mi.Bloqueado = DB.DInt(item["Bloqueado"].ToString());
            mi.UltimoDesbloqueo = DB.DFecha(item["UltimoDesbloqueo"].ToString());
            mi.Id_usuario_desbl = DB.DInt(item["Id_usuario_desbl"].ToString());

            return mi;
        }

        public bool bloquear()
        {
            string sql = "";
            UltimoAcceso = DateTime.Now;
            bool existeRegistro = false;
            int bloqueado = 0;

            string sqlId = "select bloqueado from dg_orden_pub_bloqueo where id_op_dg = " + Id_op_dg.ToString();
            DataTable t = DB.Select(sqlId);
            if (t.Rows.Count == 1)
            {
                existeRegistro = true;
                foreach (DataRow r in t.Rows)
                {
                    bloqueado = DB.DInt(r["bloqueado"].ToString());
                }
            }

            if (existeRegistro == false)
            {
                sql = "insert into dg_orden_pub_bloqueo (id_op_dg, id_usuario, ultimoAcceso, bloqueado)" +
                      " values (@id_op_dg, @id_usuario, @ultimoAcceso, 1)";
            }

            else if (existeRegistro == true && bloqueado == 0)
            {
                sql = "update dg_orden_pub_bloqueo set id_op_dg = @id_op_dg, id_usuario = @id_usuario, ultimoAcceso=@ultimoAcceso, bloqueado = 1 where id_op_dg = @id_op_dg";
            }

            if (sql != "")
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@id_op_dg", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_op_dg) });
                parametros.Add(new SqlParameter() { ParameterName = "@id_usuario", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_usuario) });
                parametros.Add(new SqlParameter() { ParameterName = "@ultimoAcceso", SqlDbType = SqlDbType.DateTime, Value = UltimoAcceso });

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
            else
            {
                return true;
            }          
        }

        public bool desbloquear()
        {
            string sql = "";
            UltimoDesbloqueo = DateTime.Now;
            //bool existeRegistro = false;
            // int bloqueado = 0;

            //string sqlId = "select id_op_dg from dg_orden_pub_bloqueo where id_op_dg = " + Id_op_dg.ToString();
            //DataTable t = DB.Select(sqlId);
            //if (t.Rows.Count == 1)
            //{
            //    existeRegistro = true;
            //    foreach (DataRow r in t.Rows)
            //    {
            //        bloqueado = DB.DInt(r["bloqueado"].ToString());
            //    }
            //}

            //if (existeRegistro == false)
            //{
            //    sql = "insert into dg_orden_pub_bloqueo (id_op_dg, id_usuario, ultimoAcceso, bloqueado)" +
            //          " values (@id_op_dg, @id_usuario, @ultimoAcceso, 1)";
            //}

            //else if (existeRegistro == true && bloqueado == 0)
            //{
                sql = "update dg_orden_pub_bloqueo set id_op_dg = @id_op_dg, id_usuario_desbl = @id_usuario_desbl, ultimoDesbloqueo=@ultimoDesbloqueo, bloqueado = 0 where id_op_dg = @id_op_dg";
            //}

            //if (sql != "")
            //{
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@id_op_dg", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_op_dg) });
                parametros.Add(new SqlParameter() { ParameterName = "@id_usuario_desbl", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_usuario) });
                parametros.Add(new SqlParameter() { ParameterName = "@ultimoDesbloqueo", SqlDbType = SqlDbType.DateTime, Value = UltimoDesbloqueo });

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
            //}
            //else
            //{
            //    return true;
            //}          
        }

    }
}
