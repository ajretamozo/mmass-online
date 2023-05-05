using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;
using System.Transactions;

namespace WebApi.Entities
{
    public class Plazos_Pagos
    {
        public int Id_condpagoap { get; set; }
        public int Id_categoria { get; set; }
        public string Desc_condpagoap { get; set; }
        public bool Es_canje { get; set; }
        public bool Es_borrado { get; set; }
        public int Dias { get; set; }


        private static Plazos_Pagos getPlazos_Pagos(DataRow item)
        {
            Plazos_Pagos mi = new Plazos_Pagos
            {
                Id_condpagoap = DB.DInt(item["id_condpagoap"].ToString()),
                Id_categoria = DB.DInt(item["id_categoria"].ToString()),
                Desc_condpagoap = item["desc_condpagoap"].ToString(),
                Es_canje = (item["es_canje"].ToString() == "1"),
                Es_borrado = (item["es_borrado"].ToString() == "1"),
                Dias = DB.DInt(item["dias"].ToString())
            };
            return mi;
        }

        public static Plazos_Pagos getById(int Id)
        {
            string sqlCommand = "select id_condpagoap, id_categoria, desc_condpagoap, es_canje, es_borrado, dias from cond_pago_ap where id_condpagoap = " + Id.ToString();
            Plazos_Pagos resultado;
            resultado = new Plazos_Pagos();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getPlazos_Pagos(t.Rows[0]);
            }
            return resultado;
        }

        public static List<Plazos_Pagos> getAll()
        {
            string sqlCommand = "select id_condpagoap, id_categoria, desc_condpagoap, es_canje, es_borrado, dias from cond_pago_ap where es_borrado = 0 ";

            List<Plazos_Pagos> col = new List<Plazos_Pagos>();
            Plazos_Pagos elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getPlazos_Pagos(item);
                col.Add(elem);
            }
            return col;
        }

    }
}
