using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Dg_orden_pub_medios
    {
        public int Id_op_dg { get; set; }
        public int Id_detalle { get; set; }
        public int Id_medio { get; set; }       
        public float Porcentaje { get; set; }
        public string Desc_medio { get; set; }
        public static Dg_orden_pub_medios getDg_orden_pub_medios(DataRow item)
        {
            Dg_orden_pub_medios mi = new Dg_orden_pub_medios
            {
                Id_op_dg = DB.DInt(item["Id_op_dg"].ToString()),
                Id_detalle = DB.DInt(item["Id_detalle"].ToString()),
                Id_medio = DB.DInt(item["Id_medio"].ToString()),
                Porcentaje = DB.DFloat(item["Porcentaje"].ToString())
            };
            //if (item["Desc_medio"].ToString() != "")
            //{
            //    mi.Desc_medio = item["Desc_medio"].ToString();
            //}
            return mi;
        }

        public  Dg_orden_pub_medios save() {
            string  sql = "insert into dg_orden_pub_medios(id_op_dg, id_detalle, id_medio, porcentaje) " +
                  "values (@id_op_dg, @id_detalle, @id_medio, @porcentaje)";
            
                List<SqlParameter> parametrosf = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                            new SqlParameter()
                            { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                            new SqlParameter()
                            { ParameterName="@id_medio",SqlDbType = SqlDbType.Int, Value = Id_medio },                            
                            new SqlParameter()
                            { ParameterName="@porcentaje",SqlDbType = SqlDbType.Float, Value = Porcentaje }
                        };
                DB.Execute(sql, parametrosf);          


            return this;
        }

    }
}
