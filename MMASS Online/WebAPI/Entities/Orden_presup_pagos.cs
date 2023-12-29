using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Orden_presup_pagos
    {
        public int Id_presup { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Nro_presup { get; set; }
        public Forma_Pago forma_pago { get; set; }
        public float Porcentaje { get; set; }

        public static Orden_presup_pagos getOrden_presup_pagos(DataRow item)
        {
            Orden_presup_pagos mi = new Orden_presup_pagos
            {
                Id_presup = DB.DInt(item["Id_presup"].ToString()),
                Anio = DB.DInt(item["Anio"].ToString()),
                Mes = DB.DInt(item["Mes"].ToString()),
                Nro_presup = DB.DInt(item["Nro_presup"].ToString()),
                forma_pago = Forma_Pago.getById(DB.DInt(item["Id_formapago"].ToString())),
                Porcentaje = DB.DFloat(item["Porcentaje"].ToString())
            };
            return mi;
        }

    }
}
