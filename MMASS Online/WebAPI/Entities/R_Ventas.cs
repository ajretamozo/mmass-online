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
    public class R_Ventas
    {
        public int Id { get; set; }
        public int Id_agencia { get; set; }
        public string Agencia { get; set; }
        public int Id_anunciante { get; set; }
        public string Anunciante { get; set; }
        public int Id_op_dg { get; set; }
        public string Nro_orden { get; set; }
        public string Nro_orden_agencia { get; set; }
        public float Primer_neto { get; set; }
        public float Imp_conf_nc { get; set; }
        public float Imp_conf_fc { get; set; }
        public float Segundo_neto { get; set; }
        /* Detalle */
        public int Id_detalle { get; set; }
        public int Id_producto { get; set; }
        public string Producto { get; set; }
        public int Cantidad_impresiones { get; set; }
        public float Monto_neto_det { get; set; }
        public int Id_medio { get; set; }
        public string Medio { get; set; }
        public float Porc_ron { get; set; }

        //AGREGUE (modifique las fechas):
        public static List<R_Ventas> filterBy(List<Parametro> parametros)
        {
            string tipo = "0";
            foreach (Parametro p in parametros)
            {
                if ((p.ParameterName == "Tipo") && (p.Value.ToString() != ""))
                {
                    tipo = p.Value;
                }
            }
            string sqlCommand = @"select ap.id_op_dg, ap.id_agencia, ag.razon_social as agencia,
                            ap.id_anunciante, an.razon_social as anunciante, 
                            cast(ap.anio as varchar(4)) + '-' + cast(ap.mes as varchar(2)) + '-' + cast(ap.nro_orden as varchar(5)) as nro_orden,
                            ap.nro_orden_ag, ap.primer_neto, ap.imp_conf_nc, ap.imp_conf_fc, ap.seg_neto,
                            p.desc_producto as producto, cast(op.anio as varchar(4)) + '-' + cast(op.mes as varchar(2)) + '-' + cast(op.nro_orden as varchar(5)) as nro_orden_rel, ";
                            
            if (tipo != "0")
            {
                sqlCommand += "det.id_producto, det.id_detalle, det.cantidad, det.monto_neto as monto_neto_det";
            }
            else
            {
                sqlCommand += "ap.id_producto, sum(det.cantidad) as cantidad";
            }                                       
            if (tipo == "2")
            {
                sqlCommand += ",apm.id_medio, m.desc_medio as medio, apm.porcentaje";
            }
            sqlCommand += " from dg_orden_pub_ap ap inner join dg_orden_pub_as det on det.id_op_dg = ap.id_op_dg inner join contactos ag on ag.id_contacto = ap.id_agencia inner join contactos an on an.id_contacto = ap.id_anunciante inner join productos p on p.id_producto = ap.id_producto left outer join orden_pub_ap op on ap.id_op_relacionada = op.id_op ";
            
            if (tipo == "2")
            {
                sqlCommand += " inner join dg_orden_pub_medios apm on apm.id_op_dg = ap.id_op_dg and apm.id_op_dg = det.id_op_dg and apm.id_detalle = det.id_detalle inner join medios m on m.id_medio = apm.id_medio ";
            }
            sqlCommand += " where (ap.es_anulada = 0 or ap.es_anulada is null)";

            string groupby = "";
            if (tipo == "0")
            {
                groupby += " group by ap.id_op_dg, ap.id_agencia, ag.razon_social, ap.id_anunciante, an.razon_social, cast(ap.anio as varchar(4)) + '-' + cast(ap.mes as varchar(2)) + '-' + cast(ap.nro_orden as varchar(5)), ap.nro_orden_ag, ap.primer_neto, ap.imp_conf_nc, ap.imp_conf_fc, ap.seg_neto, ap.id_producto, p.desc_producto, cast(op.anio as varchar(4)) + '-' + cast(op.mes as varchar(2)) + '-' + cast(op.nro_orden as varchar(5)) ";
            }
            
            string mifiltro = "";
            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    //if ((p.ParameterName == "FechaDesde") && (p.Value.ToString() != ""))
                    //{
                    //    DateTime fecha = DateTime.Parse(p.Value);
                    //    string formatted = fecha.ToString("dd-MM-yyyy");
                    //    mifiltro = mifiltro + " and ap.fecha >='" + formatted + "'";
                    //}
                    //if ((p.ParameterName == "FechaHasta") && (p.Value.ToString() != ""))
                    //{
                    //    DateTime fecha = DateTime.Parse(p.Value);
                    //    string formatted = fecha.ToString("dd-MM-yyyy");
                    //    mifiltro = mifiltro + " and ap.fecha <='" + formatted + "'";
                    //}
                    if ((p.ParameterName == "FechaDesde") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ap.fecha >='" + p.Value + "'";
                    if ((p.ParameterName == "FechaHasta") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ap.fecha <='" + p.Value + "'";
                    if ((p.ParameterName == "ListaAgencias") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ap.id_agencia in (" + p.Value + ")";
                    if ((p.ParameterName == "ListaAnunciantes") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ap.id_anunciante in (" + p.Value + ")";
                    if ((p.ParameterName == "ListaMedios") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and apm.id_medio in (" + p.Value + ")";
                    if ((p.ParameterName == "ListaProductos") && (p.Value.ToString() != ""))
                    {
                        if(tipo == "0")
                        {
                            mifiltro = mifiltro + " and ap.id_producto in (" + p.Value + ")";
                        } else
                        {
                            mifiltro = mifiltro + " and det.id_producto in (" + p.Value + ")";
                        }
                        
                    }
                }
            }   

            List<R_Ventas> col = new List<R_Ventas>();
            R_Ventas elem;
            DataTable t = DB.Select(sqlCommand + mifiltro + groupby);
            int i = 0;
            foreach (DataRow item in t.Rows)
            {
                i++;
                elem = new R_Ventas
                {
                    Id = i,
                    Id_agencia = int.Parse(item["id_agencia"].ToString()),
                    Agencia = item["agencia"].ToString(),
                    Id_anunciante = int.Parse(item["id_anunciante"].ToString()),
                    Anunciante = item["anunciante"].ToString(),
                    Id_op_dg = int.Parse(item["id_op_dg"].ToString()),
                    Nro_orden_agencia = item["nro_orden_ag"].ToString(),
                    Primer_neto = float.Parse(item["primer_neto"].ToString()),
                    Imp_conf_nc = float.Parse(item["imp_conf_nc"].ToString()),
                    Imp_conf_fc = float.Parse(item["imp_conf_fc"].ToString()),
                    Segundo_neto = float.Parse(item["seg_neto"].ToString()),
                    Id_producto = int.Parse(item["id_producto"].ToString()),
                    Producto = item["producto"].ToString(),
                    Cantidad_impresiones = int.Parse(item["cantidad"].ToString()),                    
                };
                if (item["nro_orden_rel"].ToString() != "")
                {
                    elem.Nro_orden = item["nro_orden_rel"].ToString();
                }
                else
                {
                    elem.Nro_orden = item["nro_orden"].ToString();
                }
                if (tipo == "1" || tipo == "2")
                {
                    elem.Id_detalle = int.Parse(item["id_detalle"].ToString());
                    elem.Monto_neto_det = float.Parse(item["monto_neto_det"].ToString());
                };
                if (tipo == "2")
                {
                    elem.Id_medio = int.Parse(item["id_medio"].ToString());
                    elem.Medio = item["medio"].ToString();
                    elem.Porc_ron = float.Parse(item["porcentaje"].ToString());
                };

                col.Add(elem);
            }
            return col;
        }
    }
}