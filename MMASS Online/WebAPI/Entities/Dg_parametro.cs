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

    }
}
