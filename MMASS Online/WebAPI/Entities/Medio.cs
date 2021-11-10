using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Medio
    {
        public int Id_medio { get; set; }
        public string Desc_medio { get; set; }
        public string Abreviatura { get; set; }
	    public DateTime? hrini_lunes { get; set; }
        public DateTime? hrini_martes { get; set; }
        public DateTime? hrini_miercoles { get; set; }
        public DateTime? hrini_jueves { get; set; }
        public DateTime? hrini_viernes { get; set; }
        public DateTime? hrini_sabado { get; set; }
        public DateTime? hrini_domingo { get; set; }
        public bool Es_borrado { get; set; }
        public int Id_empresa { get; set; }
        public bool Tope_legal { get; set; }
        public int Id_banda { get; set; }
        public int Id_localidad { get; set; }
        public int Id_region { get; set; }
        public string Ubic_export { get; set; }
        public string Prefijo_export { get; set; }
        public string Observa { get; set; }
        public int Opcion1 { get; set; }
        public int Opcion2 { get; set; }
        public int Opcion3 { get; set; }
        public bool Agrupainterior { get; set; }
        public bool Rut_24 { get; set; }
	    public bool Rut_upd_dyn_prg { get; set; }
        public bool Rut_upd_dyn_block { get; set; }
        public bool Rut_copy_mat_prg { get; set; }
        public string Aux1 { get; set; }
        public bool Repetidora { get; set; }
        public int Id_repetidora { get; set; }
        public int Tipo_medio { get; set; }
        public int Id_configuracion_hoja { get; set; }
        public DateTime? Vigencia_desde { get; set; }
        public DateTime? Vigencia_hasta { get; set; }
        public string Aux2 { get; set; }
        public bool Usa_frame { get; set; }
        public int Id_mercado { get; set; }

        //public static Medio getMedio(DataRow item)
        //{
        //    Medio miMedio = new Medio
        //    {
        //        Id_medio = int.Parse(item["Id_medio"].ToString()),
        //        Desc_medio = item["Desc_medio"].ToString(),
        //        Abreviatura = item["abreviatura"].ToString(),
        //        hrini_lunes = DB.DFecha(item["hrini_lunes"]),
        //        hrini_martes = DB.DFecha(item["hrini_martes"].ToString()),
        //        hrini_miercoles = DB.DFecha(item["hrini_miercoles"].ToString()),
        //        hrini_jueves = DB.DFecha(item["hrini_jueves"].ToString()),
        //        hrini_viernes = DB.DFecha(item["hrini_viernes"].ToString()),
        //        hrini_sabado = DB.DFecha(item["hrini_sabado"].ToString()),
        //        hrini_domingo = DB.DFecha(item["hrini_domingo"].ToString()),
        //        Es_borrado = (item["Es_borrado"].ToString() == "1"),
        //        Id_empresa = DB.DInt(item["id_empresa"].ToString()),
        //        Tope_legal = (item["tope_legal"].ToString() == "1"),
        //        Id_banda = DB.DInt(item["id_banda"].ToString()),
        //        Id_localidad = DB.DInt(item["id_localidad"].ToString()),
        //        Id_region = DB.DInt(item["id_region"].ToString()),
        //        Ubic_export = item["ubic_export"].ToString(),
        //        Prefijo_export = item["prefijo_export"].ToString(),
        //        Observa = item["observa"].ToString(),
        //        Opcion1 = DB.DInt(item["opcion1"].ToString()),
        //        Opcion2 = DB.DInt(item["opcion2"].ToString()),
        //        Opcion3 = DB.DInt(item["opcion3"].ToString()),
        //        Agrupainterior = (item["agrupainterior"].ToString() == "1"),
        //        Rut_24 = (item["rut_24"].ToString() == "1"),
        //        Rut_upd_dyn_prg = (item["rut_upd_dyn_prg"].ToString() == "1"),
        //        Rut_upd_dyn_block = (item["rut_upd_dyn_block"].ToString() == "1"),
        //        Rut_copy_mat_prg = (item["rut_copy_mat_prg"].ToString() == "1"),
        //        Aux1 = item["aux1"].ToString(),
        //        Aux2 = item["aux2"].ToString(),
        //        Repetidora = (item["repetidora"].ToString() == "1"),
        //        Id_repetidora = DB.DInt(item["id_repetidora"].ToString()),
        //        Tipo_medio = DB.DInt(item["tipo_medio"].ToString()),
        //        Id_configuracion_hoja = DB.DInt(item["id_configuracion_hoja"].ToString()),
        //        Vigencia_desde = DB.DFecha(item["vigencia_desde"].ToString()),
        //        Vigencia_hasta = DB.DFecha(item["vigencia_hasta"].ToString()),
        //        Usa_frame = (item["usa_frame"].ToString() == "1"),
        //        Id_mercado = DB.DInt(item["id_mercado"].ToString())
        //     };
        //    return miMedio;
        //}

        public static Medio getMedio(DataRow item)
        {
            Medio miMedio = new Medio
            {
                Id_medio = int.Parse(item["Id_medio"].ToString()),
                Desc_medio = item["Desc_medio"].ToString(),
                Abreviatura = item["abreviatura"].ToString(),
                hrini_lunes = DB.DFecha(item["hrini_lunes"]),
                hrini_martes = DB.DFecha(item["hrini_martes"].ToString()),
                hrini_miercoles = DB.DFecha(item["hrini_miercoles"].ToString()),
                hrini_jueves = DB.DFecha(item["hrini_jueves"].ToString()),
                hrini_viernes = DB.DFecha(item["hrini_viernes"].ToString()),
                hrini_sabado = DB.DFecha(item["hrini_sabado"].ToString()),
                hrini_domingo = DB.DFecha(item["hrini_domingo"].ToString()),
                Es_borrado = (item["Es_borrado"].ToString() == "1"),
                Id_empresa = DB.DInt(item["id_empresa"].ToString()),
                Tope_legal = (item["tope_legal"].ToString() == "1"),
                Id_banda = DB.DInt(item["id_banda"].ToString()),
                Id_localidad = DB.DInt(item["id_localidad"].ToString()),
                Id_region = DB.DInt(item["id_region"].ToString()),
                Ubic_export = item["ubic_export"].ToString(),
                Prefijo_export = item["prefijo_export"].ToString(),
                Observa = item["observa"].ToString(),
                Opcion1 = DB.DInt(item["opcion1"].ToString()),
                Opcion2 = DB.DInt(item["opcion2"].ToString()),
                Opcion3 = DB.DInt(item["opcion3"].ToString()),
                Agrupainterior = (item["agrupainterior"].ToString() == "1"),
                Rut_24 = (item["rut_24"].ToString() == "1"),
                Rut_upd_dyn_prg = (item["rut_upd_dyn_prg"].ToString() == "1"),
                Rut_upd_dyn_block = (item["rut_upd_dyn_block"].ToString() == "1"),
                Rut_copy_mat_prg = (item["rut_copy_mat_prg"].ToString() == "1"),
                Aux1 = item["aux1"].ToString(),
                Repetidora = (item["repetidora"].ToString() == "1"),
                Id_repetidora = DB.DInt(item["id_repetidora"].ToString()),
                Tipo_medio = DB.DInt(item["tipo_medio"].ToString())         
            };
            return miMedio;
        }

        //public static Medio getById(int Id)
        //{
        //    string sqlCommand = "select id_medio, desc_medio, abreviatura, hrini_lunes, hrini_martes, hrini_miercoles," +
        //                        " hrini_jueves, hrini_viernes, hrini_sabado, hrini_domingo, es_borrado, id_empresa, tope_legal, " +
        //                        " id_banda,id_localidad,id_region,ubic_export,prefijo_export,observa,opcion1,opcion2,opcion3,agrupainterior," +
        //                        " rut_24,rut_upd_dyn_prg,rut_upd_dyn_block,rut_copy_mat_prg,aux1,repetidora,id_repetidora,tipo_medio, " +
        //                        " id_configuracion_hoja,vigencia_desde,vigencia_hasta,aux2,usa_frame,id_mercado " +
        //                        " FROM medios where tipo_medio = 2 and Id_medio = " + Id.ToString();
        //    Medio resultado;
        //    resultado = new Medio();
        //    DataTable t = DB.Select(sqlCommand);

        //    if (t.Rows.Count == 1)
        //    {
        //        resultado = getMedio(t.Rows[0]);
        //    }
        //    return resultado;
        //}

        public static Medio getById(int Id)
        {
            string sqlCommand = @"select id_medio, desc_medio, abreviatura, hrini_lunes, hrini_martes, hrini_miercoles,
                                hrini_jueves, hrini_viernes, hrini_sabado, hrini_domingo, es_borrado, id_empresa, tope_legal,
                                id_banda, id_localidad, id_region, ubic_export, prefijo_export, observa, opcion1, opcion2, opcion3, agrupainterior,
                                rut_24, rut_upd_dyn_prg, rut_upd_dyn_block, rut_copy_mat_prg, aux1, repetidora, id_repetidora, tipo_medio
                                FROM medios where tipo_medio = 2 and Id_medio = " + Id.ToString();
            Medio resultado;
            resultado = new Medio();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getMedio(t.Rows[0]);
            }
            return resultado;
        }

        //public static List<Medio> getAll()
        //{
        //    string sql = "select id_medio, desc_medio, abreviatura, hrini_lunes, hrini_martes, hrini_miercoles, hrini_jueves, hrini_viernes, hrini_sabado, hrini_domingo," +
        //                 "id_empresa,tope_legal,id_banda, id_localidad, id_region, ubic_export, prefijo_export, observa, opcion1, opcion2, opcion3, agrupainterior, " +
        //                 "rut_24, rut_upd_dyn_prg, rut_upd_dyn_block, rut_copy_mat_prg, aux1, repetidora, id_repetidora, tipo_medio, id_configuracion_hoja, vigencia_desde, vigencia_hasta, aux2," +
        //                 "usa_frame, id_mercado, Es_borrado from medios where es_borrado = 0 and tipo_medio = 2 ";
        //    List<Medio> col = new List<Medio>();
        //    Medio elem;
        //    DataTable t = DB.Select(sql);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        elem = getMedio(item);                
        //        col.Add(elem);
        //    }
        //    return col;
        //}

        public static List<Medio> getAll()
        {
            string sql = @"select id_medio, desc_medio, abreviatura, hrini_lunes, hrini_martes, hrini_miercoles,
                                hrini_jueves, hrini_viernes, hrini_sabado, hrini_domingo, es_borrado, id_empresa, tope_legal,
                                id_banda, id_localidad, id_region, ubic_export, prefijo_export, observa, opcion1, opcion2, opcion3, agrupainterior,
                                rut_24, rut_upd_dyn_prg, rut_upd_dyn_block, rut_copy_mat_prg, aux1, repetidora, id_repetidora, tipo_medio 
                                from medios where es_borrado = 0 and tipo_medio = 2 ";
            List<Medio> col = new List<Medio>();
            Medio elem;
            DataTable t = DB.Select(sql);

            foreach (DataRow item in t.Rows)
            {
                elem = getMedio(item);
                col.Add(elem);
            }
            return col;
        }

        public static List<Medio> getPorEmpresa(int idEmpresa)
        {
            string sql = @"select id_medio, desc_medio, abreviatura, hrini_lunes, hrini_martes, hrini_miercoles,
                                hrini_jueves, hrini_viernes, hrini_sabado, hrini_domingo, es_borrado, id_empresa, tope_legal,
                                id_banda, id_localidad, id_region, ubic_export, prefijo_export, observa, opcion1, opcion2, opcion3, agrupainterior,
                                rut_24, rut_upd_dyn_prg, rut_upd_dyn_block, rut_copy_mat_prg, aux1, repetidora, id_repetidora, tipo_medio 
                                from medios where es_borrado = 0 and tipo_medio = 2 and id_empresa= " + idEmpresa.ToString();
            List<Medio> col = new List<Medio>();
            Medio elem;
            DataTable t = DB.Select(sql);
            
            foreach (DataRow item in t.Rows)
            {
                elem = getMedio(item);
                col.Add(elem);
            }
            return col;
        }
    }
}
