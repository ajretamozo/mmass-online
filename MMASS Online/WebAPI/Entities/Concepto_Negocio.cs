using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;

namespace WebApi.Entities
{
    public class Concepto_Negocio
    {
        public int Id_concepto_negocio { get; set; }
        public int Id_linea_negocio { get; set; }
        public string Desc_concepto_negocio { get; set; }
        public bool Es_borrado { get; set; }
        public static Concepto_Negocio getById(int Id)
        {
            string sqlCommand = " select id_concepto_negocio, id_linea_negocio, desc_concepto_negocio, es_borrado from concepto_negocios " +
                                " where id_concepto_negocio = " + Id.ToString();
            Concepto_Negocio resultado;
            resultado = new Concepto_Negocio();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado.Id_concepto_negocio = int.Parse(t.Rows[0]["id_concepto_negocio"].ToString());
                resultado.Desc_concepto_negocio = t.Rows[0]["desc_concepto_negocio"].ToString();
                resultado.Id_linea_negocio = int.Parse(t.Rows[0]["id_linea_negocio"].ToString());
                resultado.Es_borrado = (t.Rows[0]["es_borrado"].ToString() == "1");
            }
            return resultado;
        }

        public static List<Concepto_Negocio> getAll()
        {
            string sqlCommand = " select id_concepto_negocio, id_linea_negocio, desc_concepto_negocio, es_borrado " +
                                " from concepto_negocios where es_borrado=0";

            List<Concepto_Negocio> col = new List<Concepto_Negocio>();
            Concepto_Negocio elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Concepto_Negocio
                {
                    Id_concepto_negocio = int.Parse(item["id_concepto_negocio"].ToString()),
                    Desc_concepto_negocio = item["desc_concepto_negocio"].ToString(),
                    Id_linea_negocio = DB.DInt(item["id_linea_negocio"]),
                    Es_borrado = (item["Es_borrado"].ToString() == "1")
                };
                col.Add(elem);
            }
            return col;
        }

        public bool save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_concepto_negocio == 0)
            {
                sql = "INSERT INTO concepto_negocios (id_concepto_negocio, id_linea_negocio, desc_concepto_negocio, es_borrado) " +
                             " VALUES (@id_concepto_negocio, @id_linea_negocio, @desc_concepto_negocio, @es_borrado)";
                DataTable t = DB.Select("select max(id_concepto_negocio) as maximo from concepto_negocios");

                if (t.Rows.Count == 1)
                {
                    Id_concepto_negocio = int.Parse(t.Rows[0]["maximo"].ToString()) + 1;
                }
            }
            else
            {
                sql = " update concepto_negocios set id_linea_negocio = @id_linea_negocio, desc_concepto_negocio = @desc_concepto_negocio, es_borrado = @es_borrado " +
                                 " where id_concepto_negocio = @id_concepto_negocio";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_concepto_negocio",SqlDbType = SqlDbType.Int, Value = Id_concepto_negocio },
                    new SqlParameter()
                    { ParameterName="@id_linea_negocio",SqlDbType = SqlDbType.Int, Value = Id_linea_negocio },
                    new SqlParameter()
                    { ParameterName = "@desc_concepto_negocio", SqlDbType = SqlDbType.NVarChar, Value = Desc_concepto_negocio },
                    new SqlParameter()
                    { ParameterName = "@es_borrado",SqlDbType = SqlDbType.Int,Value = Es_borrado }
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
    }
}
