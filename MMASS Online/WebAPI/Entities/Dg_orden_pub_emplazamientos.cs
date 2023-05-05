using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;
//using Microsoft.IdentityModel.Protocols;

namespace WebApi.Entities
{
    public class Dg_orden_pub_emplazamientos
    {
        public int Id_op_dg { get; set; }
        public int Id_detalle { get; set; }
        public int Id_emplazamiento { get; set; }
        public string Descripcion { get; set; }
        public long Codigo_emplazamiento { get; set; }

        public static Dg_orden_pub_emplazamientos getDg_orden_pub_emplazamientos(DataRow item)
        {
            Dg_orden_pub_emplazamientos mi = new Dg_orden_pub_emplazamientos
            {
                Id_op_dg = DB.DInt(item["Id_op_dg"].ToString()),
                Id_detalle = DB.DInt(item["Id_detalle"].ToString()),
                Id_emplazamiento = DB.DInt(item["Id_emplazamiento"].ToString()),
                Descripcion = item["Descripcion"].ToString(),
                Codigo_emplazamiento = DB.DLong(item["Codigo_emplazamiento"].ToString())
        };
            return mi;
        }

        public static Dg_orden_pub_emplazamientos getByIdDetalle(int id_op_dg, int id_detalle)
        {
            string sqlCommand = " select id_op_dg, id_detalle, id_emplazamiento, descripcion, codigo_emplazamiento from dg_orden_pub_emplazamientos" +
                                " where id_op_dg = " + id_op_dg.ToString() + " and id_detalle = " + id_detalle.ToString();
            Dg_orden_pub_emplazamientos resultado;
            resultado = new Dg_orden_pub_emplazamientos();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_orden_pub_emplazamientos(t.Rows[0]);
            }
            return resultado;
        }

        public Dg_orden_pub_emplazamientos save()
        {
            string sql = "insert into dg_orden_pub_emplazamientos(id_op_dg, id_detalle, id_emplazamiento, descripcion, codigo_emplazamiento) " +
                         "values (@id_op_dg, @id_detalle, @id_emplazamiento, @descripcion, @codigo_emplazamiento)";

            List<SqlParameter> parametrosf = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                            new SqlParameter()
                            { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                            new SqlParameter()
                            { ParameterName="@id_emplazamiento",SqlDbType = SqlDbType.Int, Value = Id_emplazamiento },
                            new SqlParameter()
                            { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                             new SqlParameter()
                            { ParameterName="@codigo_emplazamiento", SqlDbType = SqlDbType.BigInt, Value = Codigo_emplazamiento }
                        };
            DB.Execute(sql, parametrosf);

            return this;
        }
    }
}
