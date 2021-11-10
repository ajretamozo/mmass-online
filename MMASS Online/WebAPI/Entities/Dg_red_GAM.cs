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
    public class Dg_red_GAM
    {

        public int Id_red { get; set; }
        public long Codigo_red { get; set; }
        public string Descripcion { get; set; }
        public bool Es_borrado { get; set; }


        public static Dg_red_GAM getDg_red_GAM(DataRow item)
        {
            Dg_red_GAM mi = new Dg_red_GAM();
            mi.Id_red = DB.DInt(item["Id_red"].ToString());
            mi.Codigo_red = DB.DLong(item["Codigo_red"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            //mi.Es_borrado = (item["Es_borrado"].ToString() == "1");
            return mi;
        }

        public static Dg_red_GAM getByCodigo(long netCode)
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM" +
                                " where es_borrado = 0 and codigo_red = " + netCode.ToString();
            Dg_red_GAM resultado;
            resultado = new Dg_red_GAM();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_red_GAM(t.Rows[0]);
            }
            return resultado;
        }

        public static Dg_red_GAM getById(int id)
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM" +
                                " where es_borrado = 0 and id_red = " + id.ToString();
            Dg_red_GAM resultado;
            resultado = new Dg_red_GAM();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_red_GAM(t.Rows[0]);
            }
            return resultado;
        }

        public static List<Dg_red_GAM> getAll()
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM where es_borrado = 0";
            List<Dg_red_GAM> col = new List<Dg_red_GAM>();
            Dg_red_GAM elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_red_GAM(item);
                col.Add(elem);
            }
            return col;
        }

        public static List<string> getCodigos()
        {
            string sqlCommand = " select codigo_red from dg_red_GAM where es_borrado = 0";
            List<string> col = new List<string>();
            string elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = item["codigo_red"].ToString();
                col.Add(elem);
            }
            return col;
        }

        public bool save()
        {
            string sql = "";

            if (Id_red == 0)
            {
                string sqlId = "select max(id_red) as maximo from dg_red_GAM";
                int nuevoId = 0;
                DataTable t = DB.Select(sqlId);

                if (t.Rows.Count == 1)
                {
                    nuevoId = DB.DInt(t.Rows[0]["maximo"].ToString());
                    nuevoId++;
                    Id_red = nuevoId;
                }

                sql = "insert into dg_red_GAM (id_red, descripcion, codigo_red, es_borrado)" +
                                     " values (@id_red, @descripcion, @codigo_red, 0)";
            }

            else
            {
                sql = "update dg_red_GAM set descripcion = @descripcion, codigo_red = @codigo_red where id_red = @id_red";
            }

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@id_red", SqlDbType = SqlDbType.Int, Value = Id_red },
                new SqlParameter()
                { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                new SqlParameter()
                { ParameterName="@codigo_red", SqlDbType = SqlDbType.BigInt, Value = Codigo_red }
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

        public bool deleteRed()
        {
            string sql = "update dg_red_GAM set es_borrado = 1 where id_red = " + Id_red.ToString() ;
            
            if (Id_red != 0)
            {
                try
                {
                    DB.Execute(sql);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }

        public static List<Dg_red_GAM> filter(List<Parametro> parametros)
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM where es_borrado = 0";

            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "descripcion") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and descripcion like '%" + p.Value + "%'";
                }
            }

            List<Dg_red_GAM> col = new List<Dg_red_GAM>();
            Dg_red_GAM elem;
            DataTable t = DB.Select(sqlCommand + mifiltro);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_red_GAM(item);
                col.Add(elem);
            }
            return col;
        }

    }
}
