using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Dg_orden_pub_pagos
    {
        public int Id_op_dg { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Nro_orden { get; set; }
        public Forma_Pago forma_pago { get; set; }
        public float Porcentaje { get; set; }
        public static Dg_orden_pub_pagos getDg_orden_pub_pagos(DataRow item)
        {
            Dg_orden_pub_pagos mi = new Dg_orden_pub_pagos
            {
                Id_op_dg = DB.DInt(item["Id_op_dg"].ToString()),
                Anio = DB.DInt(item["Anio"].ToString()),
                Mes = DB.DInt(item["Mes"].ToString()),
                Nro_orden = DB.DInt(item["Nro_orden"].ToString()),
                forma_pago = Forma_Pago.getById(DB.DInt(item["Id_formapago"].ToString())),
                Porcentaje = DB.DFloat(item["Porcentaje"].ToString())
            };
            return mi;
        }

    }
}
