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
    public class Dg_orden_pub_medidas
    {
        public int Id_op_dg { get; set; }
        public int Id_detalle { get; set; }
        public int Id_medidadigital { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public int Tipo { get; set; }

        public static Dg_orden_pub_medidas getDg_orden_pub_medidas(DataRow item)
        {
            Dg_orden_pub_medidas mi = new Dg_orden_pub_medidas
            {
                Id_op_dg = DB.DInt(item["Id_op_dg"].ToString()),
                Id_detalle = DB.DInt(item["Id_detalle"].ToString()),
                Id_medidadigital = DB.DInt(item["Id_medidadigital"].ToString()),
                Ancho = DB.DInt(item["Ancho"].ToString()),
                Alto = DB.DInt(item["Alto"].ToString()),
                Tipo = DB.DInt(item["Tipo"].ToString())
            };     
            return mi;
        }

        public static Dg_orden_pub_medidas getByIdDetalle(int id_detalle)
        {
            string sqlCommand = " select id_op_dg, id_detalle, id_medidadigital, ancho, alto, tipo from dg_orden_pub_medidas" +
                                " where id_detalle = " + id_detalle.ToString();
            Dg_orden_pub_medidas resultado;
            resultado = new Dg_orden_pub_medidas();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_orden_pub_medidas(t.Rows[0]);
            }
            return resultado;
        }

        public Dg_orden_pub_medidas save()
        {
            string sql = "insert into dg_orden_pub_medidas(id_op_dg, id_detalle, id_medidadigital, ancho, alto, tipo) " +
                  "values (@id_op_dg, @id_detalle, @id_medidadigital, @ancho, @alto, @tipo)";

            List<SqlParameter> parametrosf = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                            new SqlParameter()
                            { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                            new SqlParameter()
                            { ParameterName="@id_medidadigital",SqlDbType = SqlDbType.Int, Value = Id_medidadigital },
                            new SqlParameter()
                            { ParameterName="@ancho",SqlDbType = SqlDbType.Int, Value = Ancho },
                            new SqlParameter()
                            { ParameterName="@alto",SqlDbType = SqlDbType.Int, Value = Alto },
                            new SqlParameter()
                            { ParameterName="@tipo",SqlDbType = SqlDbType.Int, Value = Tipo }
                        };
            DB.Execute(sql, parametrosf);

            return this;
        }

    }
}
