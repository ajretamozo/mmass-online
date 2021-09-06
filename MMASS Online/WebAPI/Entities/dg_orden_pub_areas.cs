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
    public class Dg_orden_pub_areas
    {
        public int Id_op_dg { get; set; }
        public int Id_detalle { get; set; }
        public int Id_area { get; set; }
        public int Codigo_area { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }

        public static Dg_orden_pub_areas getDg_orden_pub_areas(DataRow item)
        {
            Dg_orden_pub_areas mi = new Dg_orden_pub_areas
            {
                Id_op_dg = DB.DInt(item["Id_op_dg"].ToString()),
                Id_detalle = DB.DInt(item["Id_detalle"].ToString()),
                Id_area = DB.DInt(item["Id_area"].ToString()),
                Codigo_area = DB.DInt(item["Codigo_area"].ToString()),
                Descripcion = item["Descripcion"].ToString(),
                Tipo = DB.DInt(item["Tipo"].ToString())
            };
            return mi;
        }

        public static Dg_orden_pub_areas getByIdDetalle(int id_op_dg, int id_detalle)
        {
            string sqlCommand = " select id_op_dg, id_detalle, id_area, codigo_area, descripcion, tipo from dg_orden_pub_areas" +
                                " where id_op_dg = " + id_op_dg.ToString() + " and id_detalle = " + id_detalle.ToString();
            Dg_orden_pub_areas resultado;
            resultado = new Dg_orden_pub_areas();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_orden_pub_areas(t.Rows[0]);
            }
            return resultado;
        }

        public Dg_orden_pub_areas save()
        {
            string sql = "insert into dg_orden_pub_areas(id_op_dg, id_detalle, id_area, codigo_area, descripcion, tipo) " +
                         "values (@id_op_dg, @id_detalle, @id_area, @codigo_area, @descripcion, @tipo)";

            List<SqlParameter> parametrosf = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                            new SqlParameter()
                            { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                            new SqlParameter()
                            { ParameterName="@id_area",SqlDbType = SqlDbType.Int, Value = Id_area },
                            new SqlParameter()
                            { ParameterName="@codigo_area",SqlDbType = SqlDbType.Int, Value = Codigo_area },
                            new SqlParameter()
                            { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                            new SqlParameter()
                            { ParameterName="@tipo",SqlDbType = SqlDbType.Int, Value = Tipo }
                        };
            DB.Execute(sql, parametrosf);

            return this;
        }
    }
}
