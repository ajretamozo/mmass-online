using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Tipo_Producto
    {
        public int Id_tipo_producto { get; set; }
        public string Desc_tipo_producto { get; set; }
        public int Id_competitivo { get; set; }
        public bool Es_borrado { get; set; }

        public static Tipo_Producto getById(int Id)
        {
            string sqlCommand = "select id_tipo_producto, desc_tipo_producto, id_competitivo, es_borrado from tipo_producto where id_tipo_producto=" + Id.ToString();
            Tipo_Producto resultado;
            resultado = new Tipo_Producto();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado.Id_tipo_producto = int.Parse(t.Rows[0]["id_tipo_producto"].ToString());
                resultado.Desc_tipo_producto = t.Rows[0]["desc_tipo_producto"].ToString();
                resultado.Id_competitivo = int.Parse(t.Rows[0]["id_competitivo"].ToString());
                resultado.Es_borrado = (t.Rows[0]["es_borrado"].ToString() == "1");
            }
            return resultado;
        }
    }
}
