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
    public class Dg_areas_geo
    {
        public int Id_area { get; set; }  
        public long Codigo_area { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }

        public static Dg_areas_geo getDg_areas_geo(DataRow item)
        {
            Dg_areas_geo mi = new Dg_areas_geo();
            mi.Id_area = DB.DInt(item["Id_area"].ToString());
            mi.Codigo_area = DB.DLong(item["Codigo_area"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            mi.Tipo = DB.DInt(item["Tipo"].ToString());
            return mi;
        }

        public static Dg_areas_geo getById(int Id_area)
        {
            string sqlCommand = " select id_area, codigo_area, descripcion, tipo from dg_areas_geo" +
                                " where id_area = " + Id_area.ToString();
            Dg_areas_geo resultado;
            resultado = new Dg_areas_geo();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_areas_geo(t.Rows[0]);
            }
            return resultado;
        }
        public static List<Dg_areas_geo> getAll()
        {
            string sqlCommand = " select id_area, codigo_area, descripcion, tipo from dg_areas_geo order by id_area";
            List<Dg_areas_geo> col = new List<Dg_areas_geo>();
            Dg_areas_geo elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_areas_geo(item);
                col.Add(elem);
            }
            return col;
        }
    }
}
