using System.Collections.Generic;
using System.Data;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Clasificacion_op
    {
        public int Id_clasificacion_op { get; set; }
        public string Desc_clasificacion_op { get; set; }
        public bool Es_borrado { get; set; }
        public string Codigo_aux { get; set; }
        
        public static Clasificacion_op getById(int Id)
        {
            string sqlCommand = " SELECT id_clasificacion_op, desc_clasificacion_op, es_borrado, codigo_aux FROM clasificacion_op " +
                                " WHERE id_clasificacion_op = " + Id.ToString();
            Clasificacion_op resultado;
            resultado = new Clasificacion_op();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado.Id_clasificacion_op = int.Parse(t.Rows[0]["id_clasificacion_op"].ToString());
                resultado.Desc_clasificacion_op = t.Rows[0]["desc_clasificacion_op"].ToString();
                resultado.Es_borrado = (t.Rows[0]["es_borrado"].ToString() == "1");
                resultado.Codigo_aux = t.Rows[0]["codigo_aux"].ToString();
            }
            return resultado;
        }

        public static List<Clasificacion_op> getAll()
        {
            string sqlCommand = " SELECT id_clasificacion_op, desc_clasificacion_op, es_borrado, codigo_aux " +
                                " FROM clasificacion_op WHERE es_borrado=0";

            List<Clasificacion_op> col = new List<Clasificacion_op>();
            Clasificacion_op elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Clasificacion_op
                {
                    Id_clasificacion_op = int.Parse(item["id_clasificacion_op"].ToString()),
                    Desc_clasificacion_op = item["desc_clasificacion_op"].ToString(),
                    Es_borrado = (item["Es_borrado"].ToString() == "1"),
                    Codigo_aux = item["codigo_aux"].ToString(),
                };
                col.Add(elem);
            }
            return col;
        }
    }
}
