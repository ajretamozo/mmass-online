using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Dg_parametro
    {
        public int Id_parametro { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
        public bool Es_borrado { get; set; }


        public static Dg_parametro getDg_parametro(DataRow item)
        {
            Dg_parametro mi = new Dg_parametro();
            mi.Id_parametro = DB.DInt(item["Idparametro"].ToString());
            mi.Nombre = item["Nombre"].ToString();
            mi.Descripcion = item["Descripcion"].ToString();
            mi.Valor = item["Valor"].ToString();
            mi.Es_borrado = (item["Es_borrado"].ToString() == "1");
            return mi;
        }

        //public static Dg_red_GAM getByCodigo(long netCode)
        //{
        //    string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM" +
        //                        " where es_borrado = 0 and codigo_red = " + netCode.ToString();
        //    Dg_red_GAM resultado;
        //    resultado = new Dg_red_GAM();
        //    DataTable t = DB.Select(sqlCommand);

        //    if (t.Rows.Count == 1)
        //    {
        //        resultado = getDg_red_GAM(t.Rows[0]);
        //    }
        //    return resultado;
        //}

        public static Dg_parametro getById(int id)
        {
            string sqlCommand = " select * from dg_parametro" +
                                " where es_borrado = 0 and idparametro = " + id.ToString();
            Dg_parametro resultado;
            resultado = new Dg_parametro();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_parametro(t.Rows[0]);
            }
            return resultado;
        }

        //public static List<Dg_red_GAM> getAll()
        //{
        //    string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM where es_borrado = 0";
        //    List<Dg_red_GAM> col = new List<Dg_red_GAM>();
        //    Dg_red_GAM elem;
        //    DataTable t = DB.Select(sqlCommand);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        elem = getDg_red_GAM(item);
        //        col.Add(elem);
        //    }
        //    return col;
        //}

        //public static List<string> getCodigos()
        //{
        //    string sqlCommand = " select codigo_red from dg_red_GAM where es_borrado = 0";
        //    List<string> col = new List<string>();
        //    string elem;
        //    DataTable t = DB.Select(sqlCommand);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        elem = item["codigo_red"].ToString();
        //        col.Add(elem);
        //    }
        //    return col;
        //}

        public bool updateParam()
        {
            bool respuesta = true;

                string sql = "update dg_parametro set valor = @valor where idparametro = @idparametro";

                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@idparametro", SqlDbType = SqlDbType.Int, Value = Id_parametro },
                    new SqlParameter()
                    { ParameterName="@valor",SqlDbType = SqlDbType.NVarChar, Value = Valor },
                };
                try
                {
                    DB.Execute(sql, parametros);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    respuesta = false;
                }
                return respuesta;
        }

        //public bool deleteRed()
        //{
        //    string sql = "update dg_red_GAM set es_borrado = 1 where id_red = " + Id_red.ToString();

        //    if (Id_red != 0)
        //    {
        //        try
        //        {
        //            DB.Execute(sql);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public static List<Dg_red_GAM> filter(List<Parametro> parametros)
        //{
        //    string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM where es_borrado = 0";

        //    string mifiltro = "";

        //    foreach (Parametro p in parametros)
        //    {
        //        if (p.Value.ToString() != "")
        //        {
        //            if ((p.ParameterName == "descripcion") && (p.Value.ToString() != ""))
        //                mifiltro = mifiltro + " and descripcion like '%" + p.Value + "%'";
        //        }
        //    }

        //    List<Dg_red_GAM> col = new List<Dg_red_GAM>();
        //    Dg_red_GAM elem;
        //    DataTable t = DB.Select(sqlCommand + mifiltro);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        elem = getDg_red_GAM(item);
        //        col.Add(elem);
        //    }
        //    return col;
        //}

    }
}
