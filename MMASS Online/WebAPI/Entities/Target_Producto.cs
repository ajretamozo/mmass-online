using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;

namespace WebApi.Entities
{
    public class Target_Producto
    {
        public int Id_target_producto { get; set; }
        public string Desc_target_producto { get; set; }
        public int Id_nse { get; set; }
        public int Id_sexo { get; set; }
        public int Id_edad { get; set; }
        public bool Es_borrado { get; set; }

        public static Target_Producto getById(int Id)
        {
            string sqlCommand = "select id_target_producto, desc_target_producto, id_nse, id_sexo,id_edad, es_borrado from target_producto where id_target_producto=" + Id.ToString();
            Target_Producto resultado;
            resultado = new Target_Producto();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado.Id_target_producto = int.Parse(t.Rows[0]["id_target_producto"].ToString());
                resultado.Desc_target_producto = t.Rows[0][""].ToString();
                resultado.Id_nse = int.Parse(t.Rows[0]["id_nse"].ToString());
                resultado.Id_sexo = int.Parse(t.Rows[0]["id_sexo"].ToString());
                resultado.Id_edad= int.Parse(t.Rows[0]["id_edad"].ToString());
                resultado.Es_borrado= (t.Rows[0]["es_borrado"].ToString() == "1");
            }
            return resultado;
        }

        public bool Save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_target_producto == 0)
            {
                sql = " insert into target_producto (id_target_producto, desc_target_producto, id_nse, id_sexo, id_edad, es_borrado) " +
                      " values (@id_target_producto, @desc_target_producto, @id_nse, @id_sexo, @id_edad, @es_borrado) ";
                 DataTable t = DB.Select("select max(id_target_producto) as maximo from target_producto");

                if (t.Rows.Count == 1)
                {
                    Id_target_producto = int.Parse(t.Rows[0]["maximo"].ToString()) + 1;
                }
            }
            else
            {
                sql = "update target_producto set desc_target_producto = @desc_target_producto, id_nse = @id_nse, id_sexo = @id_sexo, id_edad = @id_edad, es_borrado = @es_borrado " +
                      " where id_target_producto = @id_target_producto";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName ="@id_target_producto",SqlDbType = SqlDbType.Int, Value = Id_target_producto},
                    new SqlParameter()
                    { ParameterName ="@Desc_target_producto", SqlDbType = SqlDbType.NVarChar, Value = Desc_target_producto },
                    new SqlParameter()
                    { ParameterName = "@id_nse", SqlDbType = SqlDbType.Int, Value = Id_nse},
                    new SqlParameter()
                    { ParameterName = "@es_borrado", SqlDbType = SqlDbType.SmallInt, Value = Es_borrado},
                    new SqlParameter()
                    { ParameterName = "@id_sexo",SqlDbType = SqlDbType.Int, Value = Id_sexo},
                    new SqlParameter()
                    { ParameterName = "@id_edad",SqlDbType = SqlDbType.Int, Value = Id_edad}
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
