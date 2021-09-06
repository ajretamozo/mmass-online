using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Dg_orden_pub_ejecutivos
    {
        public int Id_op_dg { get; set; }
        public int Id_ejecutivo { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Nro_orden { get; set; }
        public int Tipo_rol { get; set; }
        public float Porcentaje { get; set; }
        public int Tipo_ejecutivo { get; set; }
        public int Tipo_rol_padre { get; set; }

        public static Dg_orden_pub_ejecutivos getDg_orden_pub_ejecutivos(DataRow item)
        {
            Dg_orden_pub_ejecutivos mi = new Dg_orden_pub_ejecutivos
            {
                Id_op_dg = DB.DInt(item["Id_op_dg"].ToString()),
                Id_ejecutivo = DB.DInt(item["Id_ejecutivo"].ToString()),
                Anio = DB.DInt(item["Anio"].ToString()),
                Mes = DB.DInt(item["Mes"].ToString()),
                Nro_orden = DB.DInt(item["Nro_orden"].ToString()),
                Tipo_rol = DB.DInt(item["Tipo_rol"].ToString()),
                Porcentaje = DB.DFloat(item["Porcentaje"].ToString()),
                Tipo_ejecutivo = DB.DInt(item["Tipo_ejecutivo"].ToString()),
                Tipo_rol_padre = DB.DInt(item["Tipo_rol_padre"].ToString())
            };
            return mi;
        }

    }
}
