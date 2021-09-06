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
    public class Dg_medios_asociados
    {
        public int Id_medio_dg { get; set; }
        public Medio medio_dg { get; set; }
        public int Id_medio_asociado { get; set; }
        public Medio medio_asociado { get; set; }
        public static Dg_medios_asociados getDg_medios_asociados(DataRow item)
        {
            Dg_medios_asociados mi = new Dg_medios_asociados();
            mi.Id_medio_dg = DB.DInt(item["Id_medio_dg"].ToString());
            mi.Id_medio_asociado = DB.DInt(item["Id_medio_asociado"].ToString());
            mi.medio_dg = new Medio();
            mi.medio_dg.Desc_medio = item["medio_dg"].ToString();
            mi.medio_asociado = new Medio();
            mi.medio_asociado.Desc_medio = item["medio_asociado"].ToString();
            return mi;
        }

        public static Dg_medios_asociados getById(int Id_m_dg, int Id_m_as)
        {
            string sqlCommand = " select dma.id_medio_dg, dma.id_medio_asociado, md.desc_medio as medio_dg, " +
                                " ma.desc_medio as medio_asociado from dg_medios_asociados dma " +
                                " inner join medios md on md.id_medio = dma.id_medio_dg " +
                                " inner join medios ma on ma.id_medio = dma.id_medio_asociado " +
                                " where dma.id_medio_dg = " + Id_m_dg.ToString() + " and dma.id_medio_asociado = " + Id_m_as.ToString();
            Dg_medios_asociados resultado;
            resultado = new Dg_medios_asociados();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_medios_asociados(t.Rows[0]);
            }
            return resultado;
        }
        public static List<Dg_medios_asociados> getAllmediosAsociados(string listaMedios)
        {
            string sqlCommand = " select dma.id_medio_dg, dma.id_medio_asociado, md.desc_medio as medio_dg," +
                                " ma.desc_medio as medio_asociado from dg_medios_asociados dma " +
                                " inner join medios md on md.id_medio = dma.id_medio_dg " +
                                " inner join medios ma on ma.id_medio = dma.id_medio_asociado " +
                                " where dma.id_medio_dg in ( " + listaMedios.ToString() + ")";
            List<Dg_medios_asociados> col = new List<Dg_medios_asociados>();
            Dg_medios_asociados elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_medios_asociados(item);
                col.Add(elem);
            }
            return col;
        }

    }
}
