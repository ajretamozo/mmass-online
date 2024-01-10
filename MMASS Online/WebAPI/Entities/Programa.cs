using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;
//using Microsoft.IdentityModel.Protocols;

namespace WebApi.Entities
{
    public class Programa
    {
        public int Id_programa { get; set; }
        public int Id_tipo_trans { get; set; }
        public String Desc_programa { get; set; }
        public int Es_espacio { get; set; }
        public int Es_borrado { get; set; }
        public int Id_medio { get; set; }
        public int Id_familia_programa { get; set; }
        public int Hrs_variable { get; set; }
        public string Abrev { get; set; }
        public int Id_representacion { get; set; }
        public int Habilitado_bonificacion { get; set; }

        public static Programa getPrograma(DataRow item)
        {
            Programa mi = new Programa();
            mi.Id_programa = DB.DInt(item["Id_programa"].ToString());
            mi.Id_tipo_trans = DB.DInt(item["Id_tipo_trans"].ToString());
            mi.Desc_programa = item["Desc_programa"].ToString();
            mi.Es_espacio = DB.DInt(item["Es_espacio"].ToString());
            mi.Es_borrado = DB.DInt(item["Es_borrado"].ToString());
            mi.Id_medio = DB.DInt(item["Id_medio"].ToString());
            mi.Id_familia_programa = DB.DInt(item["Id_familia_programa"].ToString());
            mi.Hrs_variable = DB.DInt(item["Hrs_variable"].ToString());
            return mi;
        }

        public static Programa getPrograma2(DataRow item)
        {
            Programa mi = new Programa();
            mi.Id_programa = DB.DInt(item["Id_programa"].ToString());
            mi.Id_tipo_trans = DB.DInt(item["Id_tipo_trans"].ToString());
            mi.Desc_programa = item["Desc_programa"].ToString();
            mi.Es_espacio = DB.DInt(item["Es_espacio"].ToString());
            mi.Es_borrado = DB.DInt(item["Es_borrado"].ToString());
            mi.Id_medio = DB.DInt(item["Id_medio"].ToString());
            mi.Id_familia_programa = DB.DInt(item["Id_familia_programa"].ToString());
            mi.Hrs_variable = DB.DInt(item["Hrs_variable"].ToString());
            mi.Abrev = item["Abrev"].ToString();
            mi.Id_representacion = DB.DInt(item["Id_representacion"].ToString());
            mi.Habilitado_bonificacion = DB.DInt(item["Habilitado_bonificacion"].ToString());
            return mi;
        }
        public static Programa getById(int Id_p)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = "select distinct p.id_programa, p.id_tipo_trans, p.desc_programa, p.es_espacio, p.es_borrado, p.id_medio, p.id_familia_programa, " +
                                    "p.hrs_variable " +
                                    " from programas p where p.es_borrado = 0 and p.id_programa = " + Id_p.ToString();
            }
            else if (BD == 2)
            {
                sqlCommand = "select distinct p.id_programa, p.id_tipo_trans, p.desc_programa, p.es_espacio, p.es_borrado, p.id_medio, p.id_familia_programa, " +
                                    "p.hrs_variable, p.Abrev, p.id_representacion, p.habilitado_bonificacion " +
                                    " from programas p where p.es_borrado = 0 and p.id_programa = " + Id_p.ToString();
            }

            Programa resultado;
            resultado = new Programa();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                if (t.Rows.Count == 1)
                {
                    if (BD == 1)
                    {
                        resultado = getPrograma(t.Rows[0]);
                    }
                    else
                    {
                        resultado = getPrograma2(t.Rows[0]);
                    }
                }
            }
            return resultado;
        }

        //public static List<Programa> getAllProgramasMedio(int Id_m)
        //{
        //    string sqlCommand = "select distinct p.id_programa, p.id_tipo_trans, p.desc_programa, p.es_espacio, p.es_borrado, p.id_medio, p.id_familia_programa, " +
        //                        "p.hrs_variable, p.Abrev, p.id_representacion, p.habilitado_bonificacion " +
        //                        " from programas p where p.es_borrado = 0 and p.id_medio =" + Id_m.ToString();
        //    List<Programa> col = new List<Programa>();
        //    Programa elem;
        //    DataTable t = DB.Select(sqlCommand);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        elem = getPrograma(item);
        //        col.Add(elem);
        //    }
        //    return col;
        //}

        public static List<Programa> getAllProgramasMedio(string listaMedios)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = "select distinct p.id_programa, p.id_tipo_trans, p.desc_programa, p.es_espacio, p.es_borrado, p.id_medio, p.id_familia_programa, " +
                               "p.hrs_variable " +
                               " from programas p where p.es_borrado = 0 and p.id_medio in ( " + listaMedios + ")";
            }
            else
            {
                sqlCommand = "select distinct p.id_programa, p.id_tipo_trans, p.desc_programa, p.es_espacio, p.es_borrado, p.id_medio, p.id_familia_programa, " +
                               "p.hrs_variable, p.Abrev, p.id_representacion, p.habilitado_bonificacion " +
                               " from programas p where p.es_borrado = 0 and p.id_medio in ( " + listaMedios + ")";
            }
  
            List<Programa> col = new List<Programa>();
            Programa elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                if (BD == 1)
                {
                    elem = getPrograma(item);
                }
                else
                {
                    elem = getPrograma2(item);
                }
                col.Add(elem);
            }
            return col;
        }

        public static List<Programa> getAllProgramasMedioxFecha(int Id_m, string Fecha)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = "select distinct p.id_programa, p.id_tipo_trans, p.desc_programa, p.es_espacio, p.es_borrado, p.id_medio, p.id_familia_programa, " +
                               "p.hrs_variable " +
                               " from programas p " +
                               " inner join emisiones_pgma e on e.id_programa = p.id_programa " +
                               " where p.es_borrado = 0 and p.id_medio =" + Id_m.ToString() +
                               " and e.vigencia_desde < '" + Fecha + "' and (vigencia_hasta is null or vigencia_hasta > '" + Fecha + "')";
            }
            else if (BD == 2)
            {
                sqlCommand = "select distinct p.id_programa, p.id_tipo_trans, p.desc_programa, p.es_espacio, p.es_borrado, p.id_medio, p.id_familia_programa, " +
                                               "p.hrs_variable, p.Abrev, p.id_representacion, p.habilitado_bonificacion " +
                                               " from programas p " +
                                               " inner join emisiones_pgma e on e.id_programa = p.id_programa " +
                                               " where p.es_borrado = 0 and p.id_medio =" + Id_m.ToString() +
                                               " and e.vigencia_desde < '" + Fecha + "' and (vigencia_hasta is null or vigencia_hasta > '" + Fecha + "')";
            }
           
            List<Programa> col = new List<Programa>();
            Programa elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                if (BD == 1)
                {
                    elem = getPrograma(item);
                }
                else
                {
                    elem = getPrograma2(item);
                }
                col.Add(elem);
            }
            return col;
        }

    }
}
