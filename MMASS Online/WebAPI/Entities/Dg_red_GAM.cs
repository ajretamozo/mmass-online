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


        public static Dg_red_GAM getDg_red_GAM(DataRow item)
        {
            Dg_red_GAM mi = new Dg_red_GAM();
            mi.Id_red = DB.DInt(item["Id_red"].ToString());
            mi.Codigo_red = DB.DLong(item["Codigo_red"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            return mi;
        }

        public static Dg_red_GAM getByCodigo(long netCode)
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM" +
                                " where codigo_red = " + netCode.ToString();
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
                                " where id_red = " + id.ToString();
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
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM";
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
            string sqlCommand = " select codigo_red from dg_red_GAM";
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

        //public void save()
        //{
        //    String sqlId = "select max(id_medidadigital) as maximo from dg_medidas";
        //    int nuevoId = 0;
        //    DataTable t = DB.Select(sqlId);

        //    if (t.Rows.Count == 1)
        //    {
        //        nuevoId = DB.DInt(t.Rows[0]["maximo"].ToString());
        //        nuevoId++;
        //    }

        //    string sql = "insert into dg_medidas (id_medidadigital, descripcion, ancho, alto, tipo, es_borrado)" +
        //                         " values (@id_medidadigital, @descripcion, @ancho, @alto, @tipo, 0)";

        //    List<SqlParameter> parametros = new List<SqlParameter>()
        //            {
        //                new SqlParameter()
        //                { ParameterName="@id_medidadigital", SqlDbType = SqlDbType.Int, Value = nuevoId },
        //                new SqlParameter()
        //                { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
        //                new SqlParameter()
        //                { ParameterName="@ancho", SqlDbType = SqlDbType.Int, Value = Ancho },
        //                new SqlParameter()
        //                { ParameterName="@alto", SqlDbType = SqlDbType.Int, Value = Alto },
        //                new SqlParameter()
        //                { ParameterName="@tipo", SqlDbType = SqlDbType.Int, Value = Tipo },
        //            };
        //    try
        //    {
        //        DB.Execute(sql, parametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //}

        //public void updateMedidas()
        //{
        //    string sql = "update dg_medidas set es_borrado = 0 where descripcion = '" + Descripcion + "'";

        //    try
        //    {
        //        DB.Execute(sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public static bool getByDesc(string desc)
        //{
        //    bool resultado = false;
        //    string sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas" +
        //                        " where descripcion = '" + desc + "'";

        //    DataTable t = DB.Select(sqlCommand);

        //    if (t.Rows.Count == 1)
        //    {
        //        resultado = true;
        //    }
        //    return resultado;
        //}

        //public bool saveMedidas()
        //{
        //    bool ret = true;
        //    DB.Execute("update dg_medidas set es_borrado = 1");
        //    foreach (Dg_medidas med in Medidas)
        //    {
        //        if (Dg_medidas.getByDesc(med.Descripcion) == false)
        //        {
        //            med.save();
        //        }
        //        else
        //        {
        //            med.updateMedidas();
        //        }
        //    }
        //    return ret;
        //}

    }
}
