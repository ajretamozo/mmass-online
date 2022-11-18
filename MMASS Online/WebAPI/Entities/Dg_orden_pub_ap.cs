using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebApi.Helpers;

namespace WebApi.Entities
{
    //AGREGUE Id_Google_Ad_Manager:
    public class Dg_orden_pub_ap
    {
        public int Id_op_dg { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Nro_orden { get; set; }
        public Empresa empresa { get; set; }
	    public Medio medio { get; set; }
        public bool Es_varios_medios { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? Fecha_expiracion { get; set; }
        public Contacto agencia { get; set; }        
        public Contacto anunciante { get; set; }
        public Producto producto { get; set; }
        public int Id_condpagoap { get; set; }
        public string Nro_orden_ag { get; set; }
        public int Facturar_a { get; set; }
        public int Tipo_orden { get; set; }
        public string Observ { get; set; }
        public float Monto_bruto { get; set; }
        public float Monto_bonif { get; set; }
        public float Monto_dto { get; set; }
        public float Primer_neto { get; set; }
        public bool Es_anulada { get; set; }
        public DateTime? Fecha_anulada { get; set; }
        public float Monto_desc_ag { get; set; }
        public float Monto_desc_an { get; set; }
        public float Monto_desc_agan { get; set; }
        public float Monto_desc_fact { get; set; }
        public float Monto_desc_difag { get; set; }
        public float Monto_desc_restfa { get; set; }
        public string Nro_orden_imp { get; set; }
        public float Porc_dto { get; set; }
        public float Porc_conf { get; set; }
        public float Seg_neto { get; set; }
        public DateTime? Fecha_alta { get; set; }
        public int Id_concepto_negocio { get; set; }
        public Usuario usuario { get; set; }
        public float Porcvol_ag { get; set; }
        public float Tercer_neto { get; set; }
        public int Localnacional { get; set; }
        public float Imp_conf_nc { get; set; }
        public float Imp_conf_fc { get; set; }
        public float Porcvol_an { get; set; }
        public float Desc2 { get; set; }
        public float Desc3 { get; set; }
        public float Desc4 { get; set; }
        public float Desc5 { get; set; }
        public int Id_cond_iva { get; set; }
        public int Id_moneda { get; set; }
        public float Cambio { get; set; }
        public int Bonificado { get; set; }
        public int Id_convenio { get; set; }
        public string Transferido { get; set; }
        public string Bitacora { get; set; }
        public bool Es_facturada { get; set; }
        public int Id_factura_ap { get; set; }
        public int Id_facturar { get; set; }        
        public int Id_op_relacionada { get; set; }
        public string Nro_op_relacionada { get; set; }
        public int Total_Avisos { get; set; }
        public int Total_Impresiones { get; set; }
        // Propiedades Descripciones Anexas
        public string Agencia_nombre { get; set; }
        public string Anunciante_nombre { get; set; }
        public string Producto_nombre { get; set; }
        public long Id_Google_Ad_Manager { get; set; }
        public int Id_red { get; set; }
        public long Codigo_red { get; set; }
        public int Parafacturar { get; set; }
        public int bloqueado { get; set; }
        public List<Dg_orden_pub_as> Detalles;
        public List<Dg_orden_pub_ejecutivos> Ejecutivos;
        public List<Dg_orden_pub_pagos> FormasPago;
        public string UsuarioSesion { get; set; } // Variable usada para enviar el usuario en los mails de alerta

        public static Dg_orden_pub_ap getDg_orden_pub_ap(DataRow item)
        {
            Dg_orden_pub_ap mi = new Dg_orden_pub_ap();
            mi.Id_op_dg = DB.DInt(item["Id_op_dg"].ToString());
            mi.Anio = DB.DInt(item["anio"].ToString());
            mi.Mes = DB.DInt(item["Mes"].ToString());
            mi.Nro_orden = DB.DInt(item["nro_orden"].ToString());
            mi.Es_facturada = (item["Es_facturada"].ToString() == "1");
            mi.Id_factura_ap = DB.DInt(item["id_factura_ap"].ToString());
            if (item["id_empresa"].ToString() != "")
            { mi.empresa = Empresa.getById(DB.DInt(item["id_empresa"].ToString())); };
            if (item["id_medio"].ToString() != "")
            { mi.medio = Medio.getById(DB.DInt(item["id_medio"].ToString())); };
            mi.Es_varios_medios = (item["Es_varios_medios"].ToString() == "1");
            mi.Fecha = DB.DFecha(item["Fecha"].ToString());
            mi.Fecha_expiracion = DB.DFecha(item["Fecha_expiracion"].ToString());
            if (item["id_agencia"].ToString() != "")
                mi.agencia = Contacto.getContactoById(DB.DInt(item["id_agencia"].ToString()));
            if (item["id_anunciante"].ToString() != "")
            {
                //if(DB.DInt(item["id_red"].ToString()) > 0)
                //{
                //    mi.anunciante = Contacto.getContactoByIdyRed(DB.DInt(item["id_anunciante"].ToString()), DB.DInt(item["id_red"].ToString()));
                //}
                mi.anunciante = Contacto.getContactoByIdyRed(DB.DInt(item["id_anunciante"].ToString()), DB.DInt(item["id_red"].ToString()));
                if (mi.anunciante.Id == 0)
                {
                    mi.anunciante = Contacto.getContactoById(DB.DInt(item["id_anunciante"].ToString()));
                }
                //else
                //{
                //    mi.anunciante = Contacto.getContactoById(DB.DInt(item["id_anunciante"].ToString()));
                //}
            }            
            if (item["id_producto"].ToString() != "")
            mi.producto = Producto.getById(DB.DInt(item["id_producto"].ToString()));
            mi.Id_condpagoap = DB.DInt(item["Id_condpagoap"].ToString());
            mi.Nro_orden_ag = item["Nro_orden_ag"].ToString();
            mi.Facturar_a = DB.DInt(item["Facturar_a"].ToString());
            mi.Tipo_orden = DB.DInt(item["Tipo_orden"].ToString());
            mi.Observ = item["Observ"].ToString();
            mi.Monto_bruto = DB.DFloat(item["Monto_bruto"].ToString());
            mi.Monto_bonif = DB.DFloat(item["Monto_bonif"].ToString());
            mi.Monto_dto = DB.DFloat(item["Monto_dto"].ToString());
            mi.Primer_neto = DB.DFloat(item["Primer_neto"].ToString());
            mi.Es_anulada = (item["Es_anulada"].ToString() == "1");
            mi.Fecha_anulada = DB.DFecha(item["Fecha_anulada"].ToString());
            mi.Monto_desc_ag = DB.DFloat(item["Monto_desc_ag"].ToString());
            mi.Monto_desc_an = DB.DFloat(item["Monto_desc_an"].ToString());
            mi.Monto_desc_agan = DB.DFloat(item["Monto_desc_agan"].ToString());
            mi.Monto_desc_fact = DB.DFloat(item["Monto_desc_fact"].ToString());
            mi.Monto_desc_difag = DB.DFloat(item["Monto_desc_difag"].ToString());
            mi.Monto_desc_restfa = DB.DFloat(item["Monto_desc_restfa"].ToString());
            mi.Nro_orden_imp = item["Nro_orden_imp"].ToString();
            mi.Porc_dto = DB.DFloat(item["Porc_dto"].ToString());
            mi.Porc_conf = DB.DFloat(item["Porc_conf"].ToString());
            mi.Seg_neto = DB.DFloat(item["Seg_neto"].ToString());
            mi.Fecha_alta = DB.DFecha(item["Fecha_alta"].ToString());
            mi.Id_concepto_negocio = DB.DInt(item["Id_concepto_negocio"].ToString());
            if (item["id_usuario"].ToString() != "")
            {
                mi.usuario = Usuario.getById(DB.DInt(item["id_usuario"].ToString()));
            }
            mi.Porcvol_ag = DB.DFloat(item["Porcvol_ag"].ToString());
            mi.Tercer_neto = DB.DFloat(item["Tercer_neto"].ToString());
            mi.Localnacional = DB.DInt(item["Localnacional"].ToString());
            mi.Imp_conf_nc = DB.DFloat(item["Imp_conf_nc"].ToString());
            mi.Imp_conf_fc = DB.DFloat(item["Imp_conf_fc"].ToString());
            mi.Porcvol_an = DB.DFloat(item["Porcvol_an"].ToString());
            mi.Desc2 = DB.DFloat(item["Desc2"].ToString());
            mi.Desc3 = DB.DFloat(item["Desc3"].ToString());
            mi.Desc4 = DB.DFloat(item["Desc4"].ToString());
            mi.Desc5 = DB.DFloat(item["Desc5"].ToString());
            mi.Id_cond_iva = DB.DInt(item["Id_cond_iva"].ToString());
            mi.Id_moneda = DB.DInt(item["Id_moneda"].ToString());
            mi.Cambio = DB.DFloat(item["Cambio"].ToString());
            mi.Bonificado = DB.DInt(item["Bonificado"].ToString());
            mi.Id_convenio = DB.DInt(item["Id_convenio"].ToString());
            mi.Transferido = item["Transferido"].ToString();
            mi.Bitacora = item["Bitacora"].ToString();
            mi.Total_Avisos = DB.DInt(item["Total_Avisos"].ToString());
            mi.Total_Impresiones = DB.DInt(item["Total_Impresiones"].ToString());
            mi.Id_facturar = DB.DInt(item["Id_facturar"].ToString());
            mi.Id_op_relacionada = DB.DInt(item["id_op_relacionada"].ToString());
            mi.Id_Google_Ad_Manager = DB.DLong(item["id_google_ad_manager"].ToString());
            mi.Id_red = DB.DInt(item["id_red"].ToString());
            mi.Parafacturar = DB.DInt(item["parafacturar"].ToString());
            if (item["bloqueado"] != null)
            {
                mi.bloqueado = DB.DInt(item["bloqueado"].ToString());
            }
            else
            {
                mi.bloqueado = 0;
            }
            if (item["nro_orden_rel"] != null)
            {
                mi.Nro_op_relacionada = item["nro_orden_rel"].ToString();
            }
            if (item.Table.Columns["agencia_nombre"] != null)
            {
                mi.Agencia_nombre = item["agencia_nombre"].ToString();
            }
            if (item.Table.Columns["anunciante_nombre"] != null)
            {
                mi.Anunciante_nombre = item["anunciante_nombre"].ToString();
            }
            if (item.Table.Columns["producto_nombre"] != null)
            {
                mi.Producto_nombre = item["producto_nombre"].ToString();
            }
            return mi;
        }
        //AGREGUE (id_area en Detalle):
        //completar metodo
        public static List<Dg_orden_pub_ap> filter(List<Parametro> parametros)
        {
            bool fillDetalles = false;
            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "fillDetalles"))
                        fillDetalles = (p.Value.ToLower() == "true");
                }
            }

            string sqlCommand = " select top 100 dg.id_op_dg, dg.anio, dg.mes, dg.nro_orden, dg.id_empresa, dg.id_medio, dg.es_varios_medios, dg.fecha, " +
                                " dg.fecha_expiracion, dg.id_agencia, dg.id_anunciante, dg.id_producto, dg.id_condpagoap, dg.nro_orden_ag, " +
                                " dg.facturar_a, dg.tipo_orden, dg.observ, dg.monto_bruto, dg.monto_bonif, dg.monto_dto, dg.primer_neto, dg.es_anulada, " +
                                " dg.fecha_anulada, dg.monto_desc_ag, dg.monto_desc_an, dg.monto_desc_agan, dg.monto_desc_fact, dg.monto_desc_difag, " +
                                " dg.monto_desc_restfa, dg.nro_orden_imp, dg.porc_dto, dg.porc_conf, dg.seg_neto, dg.fecha_alta, dg.id_concepto_negocio, " +
                                " dg.id_usuario, dg.porcvol_ag, dg.tercer_neto, dg.localnacional, dg.imp_conf_nc, dg.imp_conf_fc, dg.porcvol_an, dg.id_op_relacionada, " +
                                " dg.Desc2, dg.Desc3, dg.Desc4, dg.desc5, dg.id_cond_iva, dg.id_moneda, dg.cambio, dg.bonificado, dg.id_convenio, " +
                                " dg.transferido, dg.bitacora, dg.Id_facturar, dg.Total_Avisos, dg.Total_Impresiones, " +
                                " dg.Es_facturada, dg.Id_factura_ap, opb.bloqueado, " +
                                " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre, dg.id_google_ad_manager, dg.id_red, dg.parafacturar, " +
                                " cast(op.anio as varchar(4)) + '-' + cast(op.mes as varchar(2)) + '-' + cast(op.nro_orden as varchar(5)) as nro_orden_rel " +
                                " from Dg_orden_pub_ap dg " +
                                " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
                                " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
                                " left outer join productos p on p.id_producto = dg.id_producto " +
                                " left outer join orden_pub_ap op on dg.id_op_relacionada = op.id_op " +
                                " left outer join dg_orden_pub_bloqueo opb on dg.id_op_dg = opb.id_op_dg " +
                                " where 1 = 1 ";
            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "anio") && (p.Value.ToString() != ""))
                    {
                        mifiltro = mifiltro + " and ((dg.anio = " + p.Value + " and op.id_op is null)";
                        mifiltro = mifiltro + " or op.anio = " + p.Value+") ";
                    }
                    if ((p.ParameterName == "mes") && (p.Value.ToString() != ""))
                    {
                        mifiltro = mifiltro + " and ((dg.mes = " + p.Value + " and op.id_op is null)";
                        mifiltro = mifiltro + " or op.mes = " + p.Value + ") ";
                    }
                    if ((p.ParameterName == "nro_orden") && (p.Value.ToString() != ""))
                    {
                        mifiltro = mifiltro + " and ((dg.nro_orden = " + p.Value + " and op.id_op is null)";
                        mifiltro = mifiltro + " or op.nro_orden = " + p.Value + ") ";
                    }
                    if ((p.ParameterName == "bitacora") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.bitacora like '%" + p.Value + "%'";
                    if ((p.ParameterName == "nro_orden_ag") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.nro_orden_ag = '" + p.Value + "'";
                    //if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                    //{
                    //    DateTime fecha = DateTime.Parse(p.Value);
                    //    string formatted = fecha.ToString("dd-MM-yyyy");
                    //    mifiltro = mifiltro + " and dg.fecha >='" + formatted + "'";
                    //}
                    //if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                    //{
                    //    DateTime fecha = DateTime.Parse(p.Value);
                    //    string formatted = fecha.ToString("dd-MM-yyyy");
                    //    mifiltro = mifiltro + " and dg.fecha_expiracion <='" + formatted + "'";
                    //}
                    if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha >= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha_expiracion <= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "agencia_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ag.razon_social like '%" + p.Value + "%'";
                    if ((p.ParameterName == "anunciante_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and an.razon_social like '%" + p.Value + "%'";
                    if ((p.ParameterName == "producto_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and p.desc_producto like '%" + p.Value + "%'";
                    if ((p.ParameterName == "anuladas") && (p.Value.ToString() == "1"))
                    {
                        mifiltro = mifiltro + " and dg.es_anulada = 0";
                    }
                    if (p.ParameterName == "para_facturar")
                    {
                        if(p.Value.ToString() == "1")
                        {
                            mifiltro = mifiltro + " and dg.parafacturar = 0";
                        }
                        else
                        {
                            mifiltro = mifiltro + " and dg.parafacturar = 1";
                        }
                    }
                    if ((p.ParameterName == "anuladas") && (p.Value.ToString() == "1"))
                    {
                        mifiltro = mifiltro + " and dg.es_anulada = 0";
                    }
                    if ((p.ParameterName == "id_empresa") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.id_empresa = " + p.Value;
                    if ((p.ParameterName == "id_red") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.id_red = " + p.Value;
                }
            }
            List<Dg_orden_pub_ap> col = new List<Dg_orden_pub_ap>();
            Dg_orden_pub_ap elem;
            DataTable t = DB.Select(sqlCommand + mifiltro + " order by dg.id_op_dg desc");

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_orden_pub_ap(item);
                
                if (fillDetalles)
                {
                    elem.Detalles = new List<Dg_orden_pub_as>();
                    string strSql = " select id_op_dg, id_detalle, anio, mes, nro_orden, fecha_desde, fecha_hasta, id_producto, descripcion, id_programa, id_tarifa_dg," +
                                        " tarifa_manual, id_tipo_aviso_dg, tipo_tarifa, imp_tarifa, importe_unitario, cantidad, monto_bruto, porc_dto," +
                                        " monto_neto, netomanual, porcconfnc, porcconffc, impconfnc, impconffc," +
                                        " porc_dto1, imp_dto1, id_mtvo_dto1, tipo_dto1, porc_dto2, imp_dto2, id_mtvo_dto2, tipo_dto2," +
                                        " porc_dto3, imp_dto3, id_mtvo_dto3, tipo_dto3, porc_dto4, imp_dto4, id_mtvo_dto4, tipo_dto4," +
                                        " porc_dto5, imp_dto5, id_mtvo_dto5, tipo_dto5,id_google_ad_manager, ron, id_area, id_det_conv, id_red from Dg_orden_pub_as where id_op_dg = " + elem.Id_op_dg.ToString();
                    DataTable td = DB.Select(strSql);
                    Dg_orden_pub_as det;
                    foreach (DataRow r in td.Rows)
                    {
                        det = Dg_orden_pub_as.getDg_orden_pub_as(r);
                        elem.Detalles.Add(det);
                    }
                }
                col.Add(elem);
            }
            return col;
        }

        //Otro 'filter' para traer menos campos y hacer la consulta más liviana
        public static List<Dg_orden_pub_ap> filterLite(List<Parametro> parametros)
        {
            bool fillDetalles = false;
            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "fillDetalles"))
                        fillDetalles = (p.Value.ToLower() == "true");
                }
            }

            string sqlCommand = " select TOP 500 id_google_ad_manager, id_red from Dg_orden_pub_ap where es_anulada = 0 ";
            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "bitacora") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.bitacora like '%" + p.Value + "%'";
                    if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha >= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha_expiracion <= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "agencia_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ag.razon_social like '%" + p.Value + "%'";
                    if ((p.ParameterName == "anunciante_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and an.razon_social like '%" + p.Value + "%'";
                    if ((p.ParameterName == "producto_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and p.desc_producto like '%" + p.Value + "%'";
                    if ((p.ParameterName == "anuladas") && (p.Value.ToString() == "1"))
                    {
                        mifiltro = mifiltro + " and dg.es_anulada = 0";
                    }
                    if (p.ParameterName == "para_facturar")
                    {
                        if (p.Value.ToString() == "1")
                        {
                            mifiltro = mifiltro + " and dg.parafacturar = 0";
                        }
                        else
                        {
                            mifiltro = mifiltro + " and dg.parafacturar = 1";
                        }
                    }
                    if ((p.ParameterName == "anuladas") && (p.Value.ToString() == "1"))
                    {
                        mifiltro = mifiltro + " and dg.es_anulada = 0";
                    }
                    if ((p.ParameterName == "id_empresa") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.id_empresa = " + p.Value;
                    if ((p.ParameterName == "id_red") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.id_red = " + p.Value;
                }
            }
            List<Dg_orden_pub_ap> col = new List<Dg_orden_pub_ap>();
            Dg_orden_pub_ap elem;
            DataTable t = DB.Select(sqlCommand + mifiltro + " order by dg.id_op_dg desc");

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_orden_pub_ap(item);

                if (fillDetalles)
                {
                    elem.Detalles = new List<Dg_orden_pub_as>();
                    string strSql = " select id_op_dg, id_detalle, anio, mes, nro_orden, fecha_desde, fecha_hasta, id_producto, descripcion, id_programa, id_tarifa_dg," +
                                        " tarifa_manual, id_tipo_aviso_dg, tipo_tarifa, imp_tarifa, importe_unitario, cantidad, monto_bruto, porc_dto," +
                                        " monto_neto, netomanual, porcconfnc, porcconffc, impconfnc, impconffc," +
                                        " porc_dto1, imp_dto1, id_mtvo_dto1, tipo_dto1, porc_dto2, imp_dto2, id_mtvo_dto2, tipo_dto2," +
                                        " porc_dto3, imp_dto3, id_mtvo_dto3, tipo_dto3, porc_dto4, imp_dto4, id_mtvo_dto4, tipo_dto4," +
                                        " porc_dto5, imp_dto5, id_mtvo_dto5, tipo_dto5,id_google_ad_manager, ron, id_area, id_det_conv, id_red from Dg_orden_pub_as where id_op_dg = " + elem.Id_op_dg.ToString();
                    DataTable td = DB.Select(strSql);
                    Dg_orden_pub_as det;
                    foreach (DataRow r in td.Rows)
                    {
                        det = Dg_orden_pub_as.getDg_orden_pub_as(r);
                        elem.Detalles.Add(det);
                    }
                }
                col.Add(elem);
            }
            return col;
        }

        public static List<Dg_orden_pub_ap> getAll()
        {
            string sqlCommand = " select top 100 dg.id_op_dg, dg.anio, dg.mes, dg.nro_orden, dg.id_empresa, dg.id_medio, dg.es_varios_medios, dg.fecha, " +
                                " dg.fecha_expiracion, dg.id_agencia, dg.id_anunciante, dg.id_producto, dg.id_condpagoap, dg.nro_orden_ag, " +
                                " dg.facturar_a, dg.tipo_orden, dg.observ, dg.monto_bruto, dg.monto_bonif, dg.monto_dto, dg.primer_neto, dg.es_anulada, " +
                                " dg.fecha_anulada, dg.monto_desc_ag, dg.monto_desc_an, dg.monto_desc_agan, dg.monto_desc_fact, dg.monto_desc_difag, " +
                                " dg.monto_desc_restfa, dg.nro_orden_imp, dg.porc_dto, dg.porc_conf, dg.seg_neto, dg.fecha_alta, dg.id_concepto_negocio, " +
                                " dg.id_usuario, dg.porcvol_ag, dg.tercer_neto, dg.localnacional, dg.imp_conf_nc, dg.imp_conf_fc, dg.porcvol_an, dg.id_op_relacionada, " +
                                " dg.Desc2, dg.Desc3, dg.Desc4, dg.desc5, dg.id_cond_iva, dg.id_moneda, dg.cambio, dg.bonificado, dg.id_convenio, " +
                                " dg.transferido, dg.bitacora, dg.Id_facturar, dg.Total_Avisos, dg.Total_Impresiones, " +
                                " dg.Es_facturada, dg.Id_factura_ap, opb.bloqueado, " +
                                " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre, dg.id_google_ad_manager, dg.id_red, dg.parafacturar, " +
                                " cast(op.anio as varchar(4)) + '-' + cast(op.mes as varchar(2)) + '-' + cast(op.nro_orden as varchar(5)) as nro_orden_rel " +
                                " from Dg_orden_pub_ap dg " +
                                " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
                                " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
                                " left outer join productos p on p.id_producto = dg.id_producto " +
                                " left outer join orden_pub_ap op on dg.id_op_relacionada = op.id_op " +
                                " left outer join dg_orden_pub_bloqueo opb on dg.id_op_dg = opb.id_op_dg " +
                                " where dg.es_anulada = 0 order by dg.id_op_dg desc";
            List<Dg_orden_pub_ap> col = new List<Dg_orden_pub_ap>();
            Dg_orden_pub_ap elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_orden_pub_ap(item);
                col.Add(elem);
            }
            return col;
        }

        public static String getAllHTML()
        {
            string tResult = "";
            //string sql = "Select Id_tipo_aviso_dg, Descripcion, Permite_envio_ads, Es_borrado FROM Dg_tipos_avisos where (Es_borrado = 0) or (Es_borrado is null) ";
            string strSql = @"select top 100  op.id_op_dg , CONCAT(anio,'-', mes,'-', nro_orden) as numero,  ag.razon_social as agencia, an.razon_social as anunciante, p.desc_producto,op.tercer_neto as neto, op.fecha as f_desde, op.fecha_expiracion as f_hasta from Dg_orden_pub_ap op , contactos ag, contactos an, productos p
                                            where
                                op.id_agencia = ag.id_contacto and op.id_anunciante = an.id_contacto and p.id_producto = op.id_producto and op.id_concepto_negocio = 2 order by op.id_op_dg desc";
            DataTable dt = DB.Select(strSql);

            foreach (DataRow item in dt.Rows)
            {
                tResult = tResult + fillRowFromOrder(item);
            }
            return JsonConvert.SerializeObject(tResult);
        }
        /// <summary>
        /// Cambiar esto estoy devolviendo html deberia retornar una lista de ordenes
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static string fillRowFromOrder(DataRow dr)
        {
            string tRow = "";
            string vigencia = DateTime.Parse(dr["f_desde"].ToString()).ToString("dd/MM/yy") + " - " + DateTime.Parse(dr["f_hasta"].ToString()).ToString("dd/MM/yy");
            tRow = @"<tr data-id={0} onclick='openOrder(this)' >
                            <td class='font-weight-bold' > {1}</td>  
                            <td > {2}</td> 
                            <td > {3} </td>
                            <td >{4}</td>
                            <td >{5}</td>
                            <td >{6}</td>
                            <td >  <button type='button' class='btn-outline-dark' onclick='deleteRenglon(this)'> <span class='fa fa-envelope' title='Enviar por e-mail'></span></button>    <button type='button' class='btn-outline-dark' onclick='clonarRenglon(this)'> <span class='fa fa-pause' title='Pausar Orden'></span></button>      </td></tr>";
            tRow = string.Format(tRow, dr["id_op_dg"], dr["numero"], vigencia, dr["agencia"], dr["anunciante"], dr["desc_producto"], dr["neto"]);
            return tRow;
        }
        //AGREGUE (id_area en Detalle):
        public static Dg_orden_pub_ap getById(int Id)
        {
            string sqlCommand = " select dg.id_op_dg, dg.anio, dg.mes, dg.nro_orden, dg.id_empresa, dg.id_medio, dg.es_varios_medios, dg.fecha, " +
                                " dg.fecha_expiracion, dg.id_agencia, dg.id_anunciante, dg.id_producto, dg.id_condpagoap, dg.nro_orden_ag, " +
                                " dg.facturar_a, dg.tipo_orden, dg.observ, dg.monto_bruto, dg.monto_bonif, dg.monto_dto, dg.primer_neto, dg.es_anulada, " +
                                " dg.fecha_anulada, dg.monto_desc_ag, dg.monto_desc_an, dg.monto_desc_agan, dg.monto_desc_fact, dg.monto_desc_difag, " +
                                " dg.monto_desc_restfa, dg.nro_orden_imp, dg.porc_dto, dg.porc_conf, dg.seg_neto, dg.fecha_alta, dg.id_concepto_negocio, " +
                                " dg.id_usuario, dg.porcvol_ag, dg.tercer_neto, dg.localnacional, dg.imp_conf_nc, dg.imp_conf_fc, dg.porcvol_an, dg.id_op_relacionada, " +
                                " dg.Desc2, dg.Desc3, dg.Desc4, dg.desc5, dg.id_cond_iva, dg.id_moneda, dg.cambio, dg.bonificado, dg.id_convenio, " +
                                " dg.transferido, dg.bitacora, dg.Id_facturar, dg.Total_Avisos, dg.Total_Impresiones, " +
                                " dg.Es_facturada, dg.Id_factura_ap, opb.bloqueado, " +
                                " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre, dg.id_google_ad_manager, dg.id_red, dg.parafacturar, " +
                                " cast(op.anio as varchar(4)) + '-' + cast(op.mes as varchar(2)) + '-' + cast(op.nro_orden as varchar(5)) as nro_orden_rel " +
                                " from Dg_orden_pub_ap dg " +
                                " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
                                " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
                                " left outer join productos p on p.id_producto = dg.id_producto " +
                                " left outer join orden_pub_ap op on dg.id_op_relacionada = op.id_op " +
                                " left outer join dg_orden_pub_bloqueo opb on dg.id_op_dg = opb.id_op_dg " +
                                " where dg.id_op_dg = " + Id.ToString();
            Dg_orden_pub_ap resultado;
            resultado = new Dg_orden_pub_ap();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_orden_pub_ap(t.Rows[0]);
                // Detalles ...
                resultado.Detalles = new List<Dg_orden_pub_as>();
                string strSql = " select id_op_dg, id_detalle, anio, mes, nro_orden, fecha_desde, fecha_hasta, id_producto, descripcion, id_programa, id_tarifa_dg," +
                                    " tarifa_manual, id_tipo_aviso_dg, tipo_tarifa, imp_tarifa, importe_unitario, cantidad, monto_bruto, porc_dto," +
                                    " monto_neto, netomanual, porcconfnc, porcconffc, impconfnc, impconffc," +
                                    " porc_dto1, imp_dto1, id_mtvo_dto1, tipo_dto1, porc_dto2, imp_dto2, id_mtvo_dto2, tipo_dto2," +
                                    " porc_dto3, imp_dto3, id_mtvo_dto3, tipo_dto3, porc_dto4, imp_dto4, id_mtvo_dto4, tipo_dto4," +
                                    " porc_dto5, imp_dto5, id_mtvo_dto5, tipo_dto5,id_google_ad_manager, ron, id_area, id_det_conv, id_red from Dg_orden_pub_as where id_op_dg = " + resultado.Id_op_dg.ToString();
                DataTable td = DB.Select(strSql);
                Dg_orden_pub_as det;
                foreach (DataRow r in td.Rows)
                {
                    det = Dg_orden_pub_as.getDg_orden_pub_as(r);
                    resultado.Detalles.Add(det);
                }
                // Ejecutivos
                resultado.Ejecutivos = new List<Dg_orden_pub_ejecutivos>();
                td = DB.Select("select id_op_dg, id_ejecutivo, anio, mes, nro_orden, tipo_rol, porcentaje, tipo_ejecutivo, tipo_rol_padre " +
                                " from Dg_orden_pub_ejecutivos where id_op_dg = " + resultado.Id_op_dg.ToString());
                Dg_orden_pub_ejecutivos dej;
                foreach (DataRow r in td.Rows)
                {
                    dej = new Dg_orden_pub_ejecutivos();
                    dej.Id_op_dg = resultado.Id_op_dg;
                    dej.Id_ejecutivo = DB.DInt(r["id_ejecutivo"].ToString());
                    dej.Anio = DB.DInt(r["anio"].ToString());
                    dej.Mes = DB.DInt(r["mes"].ToString());
                    dej.Nro_orden = DB.DInt(r["nro_orden"].ToString());
                    dej.Tipo_rol = DB.DInt(r["tipo_rol"].ToString());
                    dej.Porcentaje = DB.DFloat(r["porcentaje"].ToString());
                    dej.Tipo_ejecutivo = DB.DInt(r["Tipo_ejecutivo"].ToString());
                    dej.Tipo_rol_padre = DB.DInt(r["tipo_rol_padre"].ToString());
                    resultado.Ejecutivos.Add(dej);
                }
                // FormasPago
                resultado.FormasPago = new List<Dg_orden_pub_pagos>();
                td = DB.Select("select id_op_dg, anio, mes, nro_orden, id_formapago, porcentaje " +
                            " from Dg_orden_pub_pagos where id_op_dg = " + resultado.Id_op_dg.ToString());
                Dg_orden_pub_pagos dfp;
                foreach (DataRow r in td.Rows)
                {
                    dfp = new Dg_orden_pub_pagos();
                    dfp.Id_op_dg = resultado.Id_op_dg;
                    dfp.Mes = DB.DInt(r["mes"].ToString());
                    dfp.Anio = DB.DInt(r["anio"].ToString());
                    dfp.Nro_orden = DB.DInt(r["nro_orden"].ToString());
                    dfp.forma_pago = Forma_Pago.getById(DB.DInt(r["id_formapago"].ToString()));
                    dfp.Porcentaje = DB.DFloat(r["porcentaje"].ToString());
                    resultado.FormasPago.Add(dfp);
                }
            }
            return resultado;
        }

        public static int proximoNumeroOrden(int anio, int mes)
        {
            int proximo = 0;
            DataTable t = DB.Select("select IsNull(max(nro_orden),0) as ultimo from dg_orden_pub_ap where mes = " + mes.ToString() + " and anio = " + anio.ToString());
            if (t.Rows.Count == 1)
            {
                proximo = int.Parse(t.Rows[0]["ultimo"].ToString()) + 1;
                if (proximo == 1)
                  proximo = 5001;
            }
            return proximo;
        }

        private static int getCondicionIva() {

            return 1;
        }

        public Dg_orden_pub_ap save()
        {
            try
            {
                string sql = "";
                Anio = Fecha.Value.Year ;
                Mes = Fecha.Value.Month ;                
                Id_moneda = 1;
                /*Id_concepto_negocio = 2;*///Orden Digitales
                Id_cond_iva = getCondicionIva();

                // Si es nuevo va insert, sino update
                if (Id_op_dg == 0)
                {
                    sql = "insert into dg_orden_pub_ap (id_op_dg,anio, mes, nro_orden, id_empresa, id_medio, es_varios_medios, fecha, fecha_expiracion, id_agencia," +
                            " id_anunciante, id_producto, id_google_ad_manager, id_red, id_condpagoap, parafacturar,  " +
                            " nro_orden_ag, facturar_a, tipo_orden, observ, monto_bruto, monto_bonif, monto_dto, primer_neto, es_anulada, fecha_anulada, monto_desc_ag," +
                            " monto_desc_an, monto_desc_agan, monto_desc_fact, monto_desc_difag, monto_desc_restfa, nro_orden_imp, porc_dto, porc_conf, seg_neto, fecha_alta," +
                            " id_concepto_negocio, id_usuario, porcvol_ag, tercer_neto, localnacional, imp_conf_nc, imp_conf_fc, porcvol_an, Desc2, Desc3, Desc4, Desc5," +
                            " id_cond_iva, id_moneda, cambio, bonificado, id_convenio, transferido, bitacora, Id_facturar, id_op_relacionada, Total_Avisos, Total_Impresiones)" +
                            " values (@id_op_dg, @anio, @mes, @nro_orden, @id_empresa, @id_medio, @es_varios_medios, @fecha, @fecha_expiracion, " +
                            " @id_agencia, @id_anunciante, @id_producto, @id_google_ad_manager, @id_red, @id_condpagoap, @parafacturar, " +
                            " @nro_orden_ag, @facturar_a, @tipo_orden, @observ, @monto_bruto, @monto_bonif, @monto_dto, @primer_neto, 0, @fecha_anulada, @monto_desc_ag, " +
                            " @monto_desc_an, @monto_desc_agan, @monto_desc_fact, @monto_desc_difag, @monto_desc_restfa, @nro_orden_imp, @porc_dto, @porc_conf, @seg_neto, @fecha_alta," +
                            " @id_concepto_negocio, @id_usuario, @porcvol_ag, @tercer_neto, @localnacional, @imp_conf_nc, @imp_conf_fc, @porcvol_an, @Desc2, @Desc3, @Desc4, @Desc5," +
                            " @id_cond_iva, @id_moneda, @cambio, @bonificado, @id_convenio, @transferido, @bitacora, @Id_facturar, @id_op_relacionada, @Total_Avisos, @Total_Impresiones)";
                    
                    //Las ordenes nuevas tienen el mes y año en curso, verificar si esto es correcto
                    
                    Fecha_alta = DateTime.Now;
                    
                    Nro_orden = proximoNumeroOrden(Anio, Mes);

                    //DataTable t = DB.Select("SELECT IDENT_CURRENT('dg_orden_pub_ap') AS ULTIMO ");
                    DataTable t = DB.Select("select IsNull(max(Id_op_dg),0) as ultimo from dg_orden_pub_ap");
                    if (t.Rows.Count == 1)
                    {
                      Id_op_dg = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
                        if (Id_op_dg == 1)
                            Id_op_dg = 5001;
                    }
                }
                else
                {
                    sql = "update dg_orden_pub_ap set  nro_orden = @nro_orden, id_empresa = @id_empresa, id_medio = @id_medio, es_varios_medios = @es_varios_medios," +
                        "fecha = @fecha, fecha_expiracion = @fecha_expiracion, id_agencia = @id_agencia, id_anunciante = @id_anunciante, " +
                        "id_producto = @id_producto, id_google_ad_manager = @id_google_ad_manager, id_red = @id_red, parafacturar = @parafacturar, " +
                        "id_condpagoap = @id_condpagoap, nro_orden_ag = @nro_orden_ag, facturar_a = @facturar_a, tipo_orden = @tipo_orden, observ = @observ," +
                        "monto_bruto = @monto_bruto, monto_bonif = @monto_bonif, monto_dto = @monto_dto, primer_neto = @primer_neto, " +
                        "fecha_anulada = @fecha_anulada, monto_desc_ag = @monto_desc_ag, monto_desc_an = @monto_desc_an, monto_desc_agan = @monto_desc_agan," +
                        "monto_desc_fact = @monto_desc_fact, monto_desc_difag = @monto_desc_difag, monto_desc_restfa = @monto_desc_restfa, nro_orden_imp = @nro_orden_imp," +
                        "porc_dto = @porc_dto, porc_conf = @porc_conf, seg_neto = @seg_neto, " +
                        "id_concepto_negocio = @id_concepto_negocio, id_usuario = @id_usuario, porcvol_ag = @porcvol_ag, tercer_neto = @tercer_neto, localnacional = @localnacional," +
                        "imp_conf_nc = @imp_conf_nc, imp_conf_fc = @imp_conf_fc, porcvol_an = @porcvol_an, Desc2 = @Desc2, Desc3 = @Desc3, Desc4 = @Desc4, Desc5 = @Desc5," +
                        "id_cond_iva = @id_cond_iva, id_moneda = @id_moneda, cambio = @cambio, bonificado = @bonificado, id_convenio = @id_convenio, transferido = @transferido," +
                        "bitacora = @bitacora, Id_facturar = @Id_facturar, id_op_relacionada = @id_op_relacionada, Total_Avisos = @Total_Avisos," +
                        "Total_Impresiones = @Total_Impresiones where id_op_dg = @id_op_dg";                    
                }
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@id_op_dg", SqlDbType = SqlDbType.Int, Value = Id_op_dg });
                if (Anio != 0)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@anio", SqlDbType = SqlDbType.Int, Value = Anio });
                }
                if (Mes != 0)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@mes", SqlDbType = SqlDbType.Int, Value = Mes });
                }
                parametros.Add(new SqlParameter() { ParameterName = "@nro_orden", SqlDbType = SqlDbType.Int, Value = Nro_orden });
                parametros.Add(new SqlParameter() { ParameterName = "@id_empresa", SqlDbType = SqlDbType.Int, Value =  empresa.Id_empresa });

                if (medio == null) {
                    parametros.Add(new SqlParameter() { ParameterName = "@id_medio", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
                }
                else if (DB.DInt(medio.Id_medio) == 0)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@id_medio", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
                }
                else {
                    parametros.Add(new SqlParameter() { ParameterName = "@id_medio", SqlDbType = SqlDbType.Int, Value = DB.DInt(medio.Id_medio) });
                }
                parametros.Add(new SqlParameter() { ParameterName = "@es_varios_medios", SqlDbType = SqlDbType.Int, Value = Es_varios_medios });
                parametros.Add(new SqlParameter() { ParameterName = "@fecha", SqlDbType = SqlDbType.DateTime, Value = Fecha });
                parametros.Add(new SqlParameter() { ParameterName = "@fecha_expiracion", SqlDbType = SqlDbType.DateTime, Value = Fecha_expiracion });
                parametros.Add(new SqlParameter() { ParameterName = "@id_agencia", SqlDbType = SqlDbType.Int, Value = agencia.Id_contacto });
                parametros.Add(new SqlParameter() { ParameterName = "@id_anunciante", SqlDbType = SqlDbType.Int, Value = anunciante.Id_contacto });
                parametros.Add(new SqlParameter() { ParameterName = "@id_producto", SqlDbType = SqlDbType.Int, Value = producto.Id_producto });
                //AGREGUE (id gam):
                parametros.Add(new SqlParameter() { ParameterName = "@id_google_ad_manager", SqlDbType = SqlDbType.BigInt, Value = Id_Google_Ad_Manager });
                parametros.Add(new SqlParameter() { ParameterName = "@id_red", SqlDbType = SqlDbType.Int, Value = Id_red });
                parametros.Add(new SqlParameter() { ParameterName = "@id_condpagoap", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_condpagoap )});
                if (Nro_orden_ag != null)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@nro_orden_ag", SqlDbType = SqlDbType.NVarChar, Value = Nro_orden_ag });
                }
                parametros.Add(new SqlParameter() { ParameterName = "@facturar_a", SqlDbType = SqlDbType.Int, Value = DB.DInt(Facturar_a )});
                parametros.Add(new SqlParameter() { ParameterName = "@tipo_orden", SqlDbType = SqlDbType.Int, Value = Tipo_orden });
                parametros.Add(new SqlParameter() { ParameterName = "@observ", SqlDbType = SqlDbType.NVarChar, Value = Observ });
                parametros.Add(new SqlParameter() { ParameterName = "@monto_bruto", SqlDbType = SqlDbType.Float, Value = Monto_bruto });
                parametros.Add(new SqlParameter() { ParameterName = "@monto_bonif", SqlDbType = SqlDbType.Float, Value = DB.DInt(Monto_bonif )});
                parametros.Add(new SqlParameter() { ParameterName = "@monto_dto", SqlDbType = SqlDbType.Float, Value = Monto_dto });
                parametros.Add(new SqlParameter() { ParameterName = "@primer_neto", SqlDbType = SqlDbType.Float, Value = Primer_neto });
                if (Fecha_anulada == null) {
                    parametros.Add(new SqlParameter() { ParameterName = "@fecha_anulada", SqlDbType = SqlDbType.DateTime, Value = DBNull.Value });
                }
                else {
                    parametros.Add(new SqlParameter() { ParameterName = "@fecha_anulada", SqlDbType = SqlDbType.DateTime, Value = Fecha_anulada });
                }
                parametros.Add(new SqlParameter() { ParameterName = "@monto_desc_ag", SqlDbType = SqlDbType.Float, Value = DB.DFloat(Monto_desc_ag) });
                parametros.Add(new SqlParameter() { ParameterName = "monto_desc_an", SqlDbType = SqlDbType.Float, Value = DB.DFloat(Monto_desc_an) });
                parametros.Add(new SqlParameter() { ParameterName = "monto_desc_agan", SqlDbType = SqlDbType.Float, Value = DB.DFloat( Monto_desc_agan) });
                parametros.Add(new SqlParameter() { ParameterName = "monto_desc_fact", SqlDbType = SqlDbType.Float, Value = DB.DFloat( Monto_desc_fact) });
                parametros.Add(new SqlParameter() { ParameterName = "@monto_desc_difag", SqlDbType = SqlDbType.Int, Value = DB.DInt(Monto_desc_difag) });
                parametros.Add(new SqlParameter() { ParameterName = "@monto_desc_restfa", SqlDbType = SqlDbType.Int, Value = DB.DInt( Monto_desc_restfa) });
                if (Nro_orden_imp == null) { Nro_orden_imp = ""; }
                parametros.Add(new SqlParameter() { ParameterName = "@nro_orden_imp", SqlDbType = SqlDbType.NVarChar, Value = Nro_orden_imp });
                parametros.Add(new SqlParameter() { ParameterName = "@porc_dto", SqlDbType = SqlDbType.Float, Value = Porc_dto });
                parametros.Add(new SqlParameter() { ParameterName = "@porc_conf", SqlDbType = SqlDbType.Float, Value = DB.DFloat( Porc_conf) });
                parametros.Add(new SqlParameter() { ParameterName = "@seg_neto", SqlDbType = SqlDbType.Float, Value = Seg_neto });
                if (Fecha_alta != null)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@fecha_alta", SqlDbType = SqlDbType.DateTime, Value = Fecha_alta });
                }
                parametros.Add(new SqlParameter() { ParameterName = "@id_concepto_negocio", SqlDbType = SqlDbType.Int, Value =DB.DInt( Id_concepto_negocio )});
                parametros.Add(new SqlParameter() { ParameterName = "@id_usuario", SqlDbType = SqlDbType.Int, Value = usuario.Id_usuario });
                parametros.Add(new SqlParameter() { ParameterName = "@porcvol_ag", SqlDbType = SqlDbType.Float, Value = DB.DFloat( Porcvol_ag )});
                parametros.Add(new SqlParameter() { ParameterName = "@tercer_neto", SqlDbType = SqlDbType.Float, Value = Tercer_neto });
                parametros.Add(new SqlParameter() { ParameterName = "@localnacional", SqlDbType = SqlDbType.Int, Value = DB.DInt(Localnacional) });
                parametros.Add(new SqlParameter() { ParameterName = "@imp_conf_nc", SqlDbType = SqlDbType.Float, Value = Imp_conf_nc });
                parametros.Add(new SqlParameter() { ParameterName = "@imp_conf_fc", SqlDbType = SqlDbType.Float, Value = Imp_conf_fc });
                parametros.Add(new SqlParameter() { ParameterName = "@porcvol_an", SqlDbType = SqlDbType.Float, Value = DB.DFloat( Porcvol_an )});
                parametros.Add(new SqlParameter() { ParameterName = "@Desc2", SqlDbType = SqlDbType.Float, Value = DB.DInt( Desc2 )});
                parametros.Add(new SqlParameter() { ParameterName = "@Desc3", SqlDbType = SqlDbType.Float, Value = DB.DInt(Desc3 )});
                parametros.Add(new SqlParameter() { ParameterName = "@Desc4", SqlDbType = SqlDbType.Float, Value = DB.DInt(Desc4 )});
                parametros.Add(new SqlParameter() { ParameterName = "@Desc5", SqlDbType = SqlDbType.Float, Value = DB.DInt(Desc5 )});
                parametros.Add(new SqlParameter() { ParameterName = "@id_cond_iva", SqlDbType = SqlDbType.Int, Value = Id_cond_iva });
                parametros.Add(new SqlParameter() { ParameterName = "@id_moneda", SqlDbType = SqlDbType.Int, Value = Id_moneda });
                parametros.Add(new SqlParameter() { ParameterName = "@cambio", SqlDbType = SqlDbType.Float, Value = Cambio });
                parametros.Add(new SqlParameter() { ParameterName = "@bonificado", SqlDbType = SqlDbType.Int, Value = DB.DInt( Bonificado) });
                if (DB.DInt(Id_convenio) == 0)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@id_convenio", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
                }
                else
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@id_convenio", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_convenio) });
                }
                if (Transferido == null)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@transferido", SqlDbType = SqlDbType.Int, Value =DBNull.Value });
                }
                else
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@transferido", SqlDbType = SqlDbType.Char, Value = Transferido });

                }
                if (Bitacora == "")
                {
                    Bitacora = anunciante.RazonSocial + "_" + Anio.ToString() + "_" + Mes.ToString() + "_" + Nro_orden;
                }
                parametros.Add(new SqlParameter() { ParameterName = "@bitacora", SqlDbType = SqlDbType.NVarChar, Value = Bitacora });               
                parametros.Add(new SqlParameter() { ParameterName = "@Id_facturar", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_facturar) });
                //if (DB.DInt(Id_op_relacionada) == -1)
                //{
                //    parametros.Add(new SqlParameter() { ParameterName = "@id_op_relacionada", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
                //}
                //else {
                //    parametros.Add(new SqlParameter() { ParameterName = "@id_op_relacionada", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_op_relacionada) });
                //}
                parametros.Add(new SqlParameter() { ParameterName = "@id_op_relacionada", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_op_relacionada) });
                parametros.Add(new SqlParameter() { ParameterName = "@Total_Avisos", SqlDbType = SqlDbType.Int, Value = Total_Avisos });
                parametros.Add(new SqlParameter() { ParameterName = "@Total_Impresiones", SqlDbType = SqlDbType.Int, Value = Total_Impresiones });
                parametros.Add(new SqlParameter() { ParameterName = "@parafacturar", SqlDbType = SqlDbType.Int, Value = Parafacturar });

                using (TransactionScope transaccion = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 2, 0)))
                {                    
                    // Grabo Cabecera...
                    DB.Execute(sql, parametros);
                    // Grabo los Detalles...

                    //AGREGUE (delete medidas, emplazamientos):
                    DB.Execute("delete from dg_orden_pub_medidas where id_op_dg = " + Id_op_dg.ToString());
                    DB.Execute("delete from dg_orden_pub_emplazamientos where id_op_dg = " + Id_op_dg.ToString());

                    DB.Execute("delete from dg_orden_pub_as where id_op_dg = " + Id_op_dg.ToString());
                    DB.Execute("delete from dg_orden_pub_medios where id_op_dg = " + Id_op_dg.ToString());
                    
                    int contadorDetalle = 1;


                    foreach (Dg_orden_pub_as elem in Detalles)
                    {
                        elem.Id_op_dg = Id_op_dg;
                        elem.Id_detalle = contadorDetalle;
                        elem.Anio = Anio;
                        elem.Mes = Mes;
                        elem.Nro_orden = Nro_orden;
                        elem.save();
                        contadorDetalle++;
                    }
                    
                    // Formas Pago
                    DB.Execute("delete from dg_orden_pub_pagos where id_op_dg = " + Id_op_dg.ToString());
                    sql = "insert into dg_orden_pub_pagos(id_op_dg, anio, mes, nro_orden, id_formapago, porcentaje) " +
                          "values (@id_op_dg, @anio, @mes, @nro_orden, @id_formapago, @porcentaje)";
                    foreach (Dg_orden_pub_pagos elem in FormasPago)
                    {
                        List<SqlParameter> parametrosf = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                            new SqlParameter()
                            { ParameterName="@anio",SqlDbType = SqlDbType.Int, Value = Anio },
                            new SqlParameter()
                            { ParameterName="@mes",SqlDbType = SqlDbType.Int, Value = Mes },
                            new SqlParameter()
                            { ParameterName="@nro_orden",SqlDbType = SqlDbType.Int, Value = Nro_orden },
                            new SqlParameter()
                            { ParameterName="@id_formapago",SqlDbType = SqlDbType.Int, Value = elem.forma_pago.Id_formapago },
                            new SqlParameter()
                            { ParameterName="@porcentaje",SqlDbType = SqlDbType.Float, Value = elem.Porcentaje }
                        };
                        DB.Execute(sql, parametrosf);
                    }
                    // Ejecutivos
                    DB.Execute("delete from dg_orden_pub_ejecutivos where id_op_dg = " + Id_op_dg.ToString());
                    sql = "insert into dg_orden_pub_ejecutivos (id_op_dg, id_ejecutivo, anio, mes, nro_orden,  tipo_rol, porcentaje, tipo_ejecutivo, tipo_rol_padre) " +
                          "values (@id_op_dg, @id_ejecutivo, @anio, @mes, @nro_orden, @tipo_rol, @porcentaje, @tipo_ejecutivo, @tipo_rol_padre)";
                    foreach (Dg_orden_pub_ejecutivos elem in Ejecutivos)
                    {
                        List<SqlParameter> parametrose = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                            new SqlParameter()
                            { ParameterName="@id_ejecutivo",SqlDbType = SqlDbType.Int, Value = elem.Id_ejecutivo },
                            new SqlParameter()
                            { ParameterName="@anio",SqlDbType = SqlDbType.Int, Value = Anio },
                            new SqlParameter()
                            { ParameterName="@mes",SqlDbType = SqlDbType.Int, Value = Mes },
                            new SqlParameter()
                            { ParameterName="@nro_orden",SqlDbType = SqlDbType.Int, Value = Nro_orden },
                            new SqlParameter()
                            { ParameterName="@tipo_rol",SqlDbType = SqlDbType.Int, Value = elem.Tipo_rol },
                            new SqlParameter()
                            { ParameterName="@porcentaje",SqlDbType = SqlDbType.Float, Value = elem.Porcentaje },
                            new SqlParameter()
                            { ParameterName="@tipo_ejecutivo",SqlDbType = SqlDbType.Int, Value = elem.Tipo_ejecutivo },
                            new SqlParameter()
                            { ParameterName="@tipo_rol_padre",SqlDbType = SqlDbType.Int, Value = elem.Tipo_rol_padre }
                        };
                        DB.Execute(sql, parametrose);
                        
                    }
                    transaccion.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return this;
        }

        public static List<DatoGenerico> getOrdenesRadio(int Id_agencia, int Id_anunciante)
        {
            string sqlCommand = @"select id_op,
                                    CAST(anio as VARCHAR(4)) + '-' + RIGHT('00' + CAST(mes AS VARCHAR(2)), 2) + '-' + RIGHT('0000' + CAST(nro_orden AS VARCHAR(4)), 4)
                                    + ' (' + productos.desc_producto + ') - Ord.Ag.: ' + IsNull(nro_orden_ag, 'No Especificado')
                                    COLLATE DATABASE_DEFAULT as nro_orden
                                    from orden_pub_ap
                                    inner join productos on productos.id_producto = orden_pub_ap.id_producto
                                    where tipo_orden<>1
                                    and es_anulada = 0
                                    and (
                                    (tipo_orden = 0 and not exists (select id_op from menciones where es_facturada = 1 and menciones.id_op = orden_pub_ap.id_op))
                                    or
                                    (tipo_orden = 2 and es_facturada = 0)
                                    )
                                    and id_agencia = " + Id_agencia.ToString() + " and id_anunciante = " + Id_anunciante.ToString();

            List<DatoGenerico> col = new List<DatoGenerico>();
            DatoGenerico elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new DatoGenerico
                {
                    Id = int.Parse(item["id_op"].ToString()),
                    Descripcion = item["nro_orden"].ToString()
                };
                col.Add(elem);
            }
            return col;
        }

        public static Dg_orden_pub_ap getOpRadioById(int Id)
        {
            string sqlCommand = " select dg.id_op, dg.anio, dg.mes, dg.nro_orden, dg.id_empresa, dg.id_medio, dg.es_varios_medios, dg.fecha, " +
                                " dg.fecha_expiracion, dg.id_agencia, dg.id_anunciante, dg.id_producto, dg.id_condpagoap, dg.nro_orden_ag, " +
                                " dg.facturar_a, dg.tipo_orden, dg.observ, dg.monto_bruto, dg.monto_bonif, dg.monto_dto, dg.primer_neto, dg.es_anulada, " +
                                " dg.fecha_anulada, dg.monto_desc_ag, dg.monto_desc_an, dg.monto_desc_agan, dg.monto_desc_fact, dg.monto_desc_difag, " +
                                " dg.monto_desc_restfa, dg.nro_orden_imp, dg.porc_dto, dg.porc_conf, dg.seg_neto, dg.fecha_alta, dg.id_concepto_negocio, " +
                                " dg.id_usuario, dg.porcvol_ag, dg.tercer_neto, dg.localnacional, dg.imp_conf_nc, dg.imp_conf_fc, dg.porcvol_an, " +
                                " dg.Desc2, dg.Desc3, dg.Desc4, dg.desc5, dg.id_cond_iva, dg.id_moneda, dg.cambio, dg.bonificado, dg.id_convenio, dg.transferido, dg.bitacora, dg.Id_facturar, " +
                                " dg.Es_facturada, " +
                                " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre " +
                                " from orden_pub_ap dg " +
                                " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
                                " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
                                " left outer join productos p on p.id_producto = dg.id_producto " +
                                " where dg.id_op = " + Id.ToString();
            Dg_orden_pub_ap mi;
            mi = new Dg_orden_pub_ap();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                DataRow item = t.Rows[0];

                mi.Id_op_dg = DB.DInt(item["Id_op"].ToString());
                mi.Anio = DB.DInt(item["anio"].ToString());
                mi.Mes = DB.DInt(item["Mes"].ToString());
                mi.Nro_orden = DB.DInt(item["nro_orden"].ToString());
                mi.Es_facturada = (item["Es_facturada"].ToString() == "1");
                if (item["id_empresa"].ToString() != "")
                { mi.empresa = Empresa.getById(DB.DInt(item["id_empresa"].ToString())); };
                if (item["id_medio"].ToString() != "")
                { mi.medio = Medio.getById(DB.DInt(item["id_medio"].ToString())); };
                mi.Es_varios_medios = (item["Es_varios_medios"].ToString() == "1");
                mi.Fecha = DB.DFecha(item["Fecha"].ToString());
                mi.Fecha_expiracion = DB.DFecha(item["Fecha_expiracion"].ToString());
                if (item["id_agencia"].ToString() != "")
                    mi.agencia = Contacto.getContactoById(DB.DInt(item["id_agencia"].ToString()));
                if (item["id_anunciante"].ToString() != "")
                    mi.anunciante = Contacto.getContactoById(DB.DInt(item["id_anunciante"].ToString()));
                if (item["id_producto"].ToString() != "")
                    mi.producto = Producto.getById(DB.DInt(item["id_producto"].ToString()));
                mi.Id_condpagoap = DB.DInt(item["Id_condpagoap"].ToString());
                mi.Nro_orden_ag = item["Nro_orden_ag"].ToString();
                mi.Facturar_a = DB.DInt(item["Facturar_a"].ToString());
                mi.Tipo_orden = DB.DInt(item["Tipo_orden"].ToString());
                mi.Observ = item["Observ"].ToString();
                mi.Monto_bruto = DB.DFloat(item["Monto_bruto"].ToString());
                mi.Monto_bonif = DB.DFloat(item["Monto_bonif"].ToString());
                mi.Monto_dto = DB.DFloat(item["Monto_dto"].ToString());
                mi.Primer_neto = DB.DFloat(item["Primer_neto"].ToString());
                mi.Es_anulada = (item["Es_anulada"].ToString() == "1");
                mi.Fecha_anulada = DB.DFecha(item["Fecha_anulada"].ToString());
                mi.Monto_desc_ag = DB.DFloat(item["Monto_desc_ag"].ToString());
                mi.Monto_desc_an = DB.DFloat(item["Monto_desc_an"].ToString());
                mi.Monto_desc_agan = DB.DFloat(item["Monto_desc_agan"].ToString());
                mi.Monto_desc_fact = DB.DFloat(item["Monto_desc_fact"].ToString());
                mi.Monto_desc_difag = DB.DFloat(item["Monto_desc_difag"].ToString());
                mi.Monto_desc_restfa = DB.DFloat(item["Monto_desc_restfa"].ToString());
                mi.Nro_orden_imp = item["Nro_orden_imp"].ToString();
                mi.Porc_dto = DB.DFloat(item["Porc_dto"].ToString());
                mi.Porc_conf = DB.DFloat(item["Porc_conf"].ToString());
                mi.Seg_neto = DB.DFloat(item["Seg_neto"].ToString());
                mi.Fecha_alta = DB.DFecha(item["Fecha_alta"].ToString());
                mi.Id_concepto_negocio = DB.DInt(item["Id_concepto_negocio"].ToString());
                if (item["id_usuario"].ToString() != "")
                {
                    mi.usuario = Usuario.getById(DB.DInt(item["id_usuario"].ToString()));
                }
                mi.Porcvol_ag = DB.DFloat(item["Porcvol_ag"].ToString());
                mi.Tercer_neto = DB.DFloat(item["Tercer_neto"].ToString());
                mi.Localnacional = DB.DInt(item["Localnacional"].ToString());
                mi.Imp_conf_nc = DB.DFloat(item["Imp_conf_nc"].ToString());
                mi.Imp_conf_fc = DB.DFloat(item["Imp_conf_fc"].ToString());
                mi.Porcvol_an = DB.DFloat(item["Porcvol_an"].ToString());
                mi.Desc2 = DB.DFloat(item["Desc2"].ToString());
                mi.Desc3 = DB.DFloat(item["Desc3"].ToString());
                mi.Desc4 = DB.DFloat(item["Desc4"].ToString());
                mi.Desc5 = DB.DFloat(item["Desc5"].ToString());
                mi.Id_cond_iva = DB.DInt(item["Id_cond_iva"].ToString());
                mi.Id_moneda = DB.DInt(item["Id_moneda"].ToString());
                mi.Cambio = DB.DFloat(item["Cambio"].ToString());
                mi.Bonificado = DB.DInt(item["Bonificado"].ToString());
                mi.Id_convenio = DB.DInt(item["Id_convenio"].ToString());
                mi.Transferido = item["Transferido"].ToString();
                mi.Bitacora = item["Bitacora"].ToString();
                mi.Id_facturar = DB.DInt(item["Id_facturar"].ToString());
                if (item.Table.Columns["agencia_nombre"] != null)
                {
                    mi.Agencia_nombre = item["agencia_nombre"].ToString();
                }
                if (item.Table.Columns["anunciante_nombre"] != null)
                {
                    mi.Anunciante_nombre = item["anunciante_nombre"].ToString();
                }
                if (item.Table.Columns["producto_nombre"] != null)
                {
                    mi.Producto_nombre = item["producto_nombre"].ToString();
                }


            }
            return mi;
        }

        //AGREGUE:
        public static void saveId_Google_Ad_Manager (long idOp, long idGam)
        {
            string sql = "";

            sql = "UPDATE dg_orden_pub_ap SET id_google_ad_manager = @idgam WHERE id_op_dg = @id_op_dg ";
   
            List<SqlParameter> parametrost = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@id_op_dg",SqlDbType = SqlDbType.BigInt, Value = idOp },
                new SqlParameter()
                { ParameterName="@idgam",SqlDbType = SqlDbType.BigInt, Value = idGam }
            };
            try
            {
                DB.Execute(sql, parametrost);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public static bool existeOpGAMenBD(long idOpGAM, int idRed)
        //{
        //    bool existe = false;
        //    string sqlCommand = "SELECT id_op_dg FROM dg_orden_pub_ap WHERE id_google_ad_manager=@idgam and id_red=@idRed";

        //    List<SqlParameter> parametros = new List<SqlParameter>()
        //    {
        //        new SqlParameter()
        //        { ParameterName="@idgam",SqlDbType = SqlDbType.BigInt, Value = idOpGAM },
        //         new SqlParameter()
        //        { ParameterName="@idRed",SqlDbType = SqlDbType.Int, Value = idRed }
        //    };

        //    DataTable t = DB.Select(sqlCommand, parametros);
        //    if (t.Rows.Count == 1)
        //    {
        //        existe = true;
        //    }
        //    return existe;
        //}

        public static bool anularOrden(int idOp)
        {
            bool resultado = true;
            string sql = "UPDATE dg_orden_pub_ap SET es_anulada = 1, fecha_anulada = GETDATE() WHERE id_op_dg = " + idOp.ToString();

            try
            {
                DB.Execute(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                resultado = false;
            }
            return resultado;
        }

        public static bool existeOpNombre(int idOp, string nom)
        {
            bool resultado = false;
            string sqlCommand;

            if (idOp == 0) {
                sqlCommand = "SELECT id_op_dg FROM dg_orden_pub_ap WHERE bitacora like '" + nom + "'";
            }
            else
            {
                sqlCommand = "SELECT id_op_dg FROM dg_orden_pub_ap WHERE bitacora like '" + nom + "' and id_op_dg != " + idOp;
            }

            DataTable t = DB.Select(sqlCommand);
            if (t.Rows.Count == 1)
            {
                resultado = true;
            }
            return resultado;
        }

        public static Dg_orden_pub_ap getOpByIdGAM(long idOpGAM, int idRed)
        {
            Dg_orden_pub_ap op = new Dg_orden_pub_ap();
            string sqlCommand = "SELECT id_op_dg, anio, mes, nro_orden FROM dg_orden_pub_ap WHERE id_google_ad_manager=@idgam and id_red=@idRed";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@idgam",SqlDbType = SqlDbType.BigInt, Value = idOpGAM },
                 new SqlParameter()
                { ParameterName="@idRed",SqlDbType = SqlDbType.Int, Value = idRed }
            };

            DataTable t = DB.Select(sqlCommand, parametros);
            if (t.Rows.Count == 1)
            {
                DataRow item = t.Rows[0];

                op.Id_op_dg = DB.DInt(item["id_op_dg"].ToString());
                op.Anio = DB.DInt(item["anio"].ToString());
                op.Mes = DB.DInt(item["Mes"].ToString());
                op.Nro_orden = DB.DInt(item["nro_orden"].ToString());
            }
            return op;
        }

        //Otro 'getAll' para traer menos campos y hacer la consulta más liviana
        public static List<Dg_orden_pub_ap> getAll2()
        {
            List<Dg_orden_pub_ap> ordenes = new List<Dg_orden_pub_ap>();

            string sqlCommand = " select id_google_ad_manager, id_red from Dg_orden_pub_ap where es_anulada = 0";

            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                Dg_orden_pub_ap op = new Dg_orden_pub_ap();
                op.Id_Google_Ad_Manager = long.Parse(item["id_google_ad_manager"].ToString());
                if (item["id_red"].ToString() == "")
                {
                    op.Id_red = 0;
                }
                else
                {
                    op.Id_red = int.Parse(item["id_red"].ToString());
                }
                ordenes.Add(op);
            }
            return ordenes;
        }

    }
}
