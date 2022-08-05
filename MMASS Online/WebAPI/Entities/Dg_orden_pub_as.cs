using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Dg_orden_pub_as
    {
        public int Id_op_dg { get; set; }
        public int Id_detalle { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Nro_orden { get; set; }
        public DateTime? Fecha_desde { get; set; }
        public DateTime? Fecha_hasta { get; set; }
        public int Id_producto { get; set; }
        public string Descripcion { get; set; }
        public int Id_programa { get; set; }
        public Dg_tarifas tarifa_dg { get; set; }
        public float Tarifa_manual { get; set; }
        public Dg_tipos_avisos tipo_aviso_dg { get; set; }
        public int Tipo_tarifa { get; set; }
        public float Imp_tarifa { get; set; }
        public float Importe_unitario { get; set; }
        public int Cantidad { get; set; }
        public float Monto_bruto { get; set; }
        public float Porc_dto { get; set; }
        public float Monto_neto { get; set; }
        public float Netomanual { get; set; }
        public float Porcconfnc { get; set; }
        public float Porcconffc { get; set; }
        public float Impconfnc { get; set; }
        public float Impconffc { get; set; }
        public float Porc_dto1 { get; set; }
        public float Imp_dto1 { get; set; }
        public int Id_mtvo_dto1 { get; set; }
        public int Tipo_dto1 { get; set; }
        public float Porc_dto2 { get; set; }
        public float Imp_dto2 { get; set; }
        public int Id_mtvo_dto2 { get; set; }
        public int Tipo_dto2 { get; set; }
        public float Porc_dto3 { get; set; }
        public float Imp_dto3 { get; set; }
        public int Id_mtvo_dto3 { get; set; }
        public int Tipo_dto3 { get; set; }
        public float Porc_dto4 { get; set; }
        public float Imp_dto4 { get; set; }
        public int Id_mtvo_dto4 { get; set; }
        public int Tipo_dto4 { get; set; }
	    public float Porc_dto5 { get; set; }
        public float Imp_dto5 { get; set; }
        public int Id_mtvo_dto5 { get; set; }
        public int Tipo_dto5 { get; set; }
        public bool Ron { get; set; }
        public long Id_Google_Ad_Manager { get; set; }
        public int Id_det_conv { get; set; }
        public int Id_red { get; set; }
        //AGREGUE:
        public Dg_areas_geo areaGeo { get; set; }


        public List<Dg_orden_pub_medios> Medios;

        //AGREGUE:
        public List<Dg_orden_pub_medidas> Medidas;
        public List<Dg_orden_pub_emplazamientos> Emplazamientos;

        public static Dg_orden_pub_as getDg_orden_pub_as(DataRow item)
        {
            Dg_orden_pub_as mi = new Dg_orden_pub_as();
            mi.Id_op_dg = int.Parse(item["Id_op_dg"].ToString());
            mi.Id_detalle = int.Parse(item["Id_detalle"].ToString());
            mi.Anio = int.Parse(item["Anio"].ToString());
            mi.Mes = int.Parse(item["Mes"].ToString());
            mi.Nro_orden = int.Parse(item["Nro_orden"].ToString());
            mi.Fecha_desde = DB.DFecha(item["Fecha_desde"].ToString());
            mi.Fecha_hasta = DB.DFecha(item["Fecha_hasta"].ToString());
            mi.Id_producto = DB.DInt(item["Id_producto"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            if (item["Id_programa"].ToString() != "")
            {
                mi.Id_programa = int.Parse(item["Id_programa"].ToString());
            }

            if (item["Id_tarifa_dg"].ToString() != "")
            {
                mi.tarifa_dg = Dg_tarifas.getById(DB.DInt(item["Id_tarifa_dg"].ToString()));
            }
            mi.Tarifa_manual = DB.DFloat(item["Tarifa_manual"].ToString());
            mi.tipo_aviso_dg = Dg_tipos_avisos.getById(DB.DInt(item["Id_tipo_aviso_dg"].ToString()));
            mi.Tipo_tarifa = DB.DInt(item["tipo_tarifa"].ToString());
            mi.Imp_tarifa = DB.DFloat(item["Imp_tarifa"].ToString());
            mi.Importe_unitario = DB.DFloat(item["Importe_unitario"].ToString());
            mi.Cantidad = DB.DInt(item["Cantidad"].ToString());
            mi.Monto_bruto = DB.DFloat(item["Monto_bruto"].ToString());
            mi.Porc_dto = DB.DFloat(item["Porc_dto"].ToString());
            mi.Monto_neto = DB.DFloat(item["Monto_neto"].ToString());
            mi.Netomanual = DB.DFloat(item["Netomanual"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.Porcconffc = DB.DFloat(item["Porcconffc"].ToString());
            mi.Impconfnc = DB.DFloat(item["Impconfnc"].ToString());
            mi.Impconffc = DB.DFloat(item["Impconffc"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.Id_mtvo_dto1 = DB.DInt(item["Id_mtvo_dto1"].ToString());
            mi.Id_mtvo_dto2 = DB.DInt(item["Id_mtvo_dto2"].ToString());
            mi.Id_mtvo_dto3 = DB.DInt(item["Id_mtvo_dto3"].ToString());
            mi.Id_mtvo_dto4 = DB.DInt(item["Id_mtvo_dto4"].ToString());
            mi.Id_mtvo_dto5 = DB.DInt(item["Id_mtvo_dto5"].ToString());

            mi.Porc_dto1 = DB.DFloat(item["Porc_dto1"].ToString());
            mi.Imp_dto1 = DB.DFloat(item["Imp_dto1"].ToString());
            mi.Tipo_dto1 = DB.DInt(item["Tipo_dto1"].ToString());

            mi.Porc_dto2 = DB.DFloat(item["Porc_dto2"].ToString());
            mi.Imp_dto2 = DB.DFloat(item["Imp_dto2"].ToString());
            mi.Tipo_dto2 = DB.DInt(item["Tipo_dto2"].ToString());

            mi.Porc_dto3 = DB.DFloat(item["Porc_dto3"].ToString());
            mi.Imp_dto3 = DB.DFloat(item["Imp_dto3"].ToString());
            mi.Tipo_dto3 = DB.DInt(item["Tipo_dto3"].ToString());

            mi.Porc_dto4 = DB.DFloat(item["Porc_dto4"].ToString());
            mi.Imp_dto4 = DB.DFloat(item["Imp_dto4"].ToString());
            mi.Tipo_dto4 = DB.DInt(item["Tipo_dto4"].ToString());

            mi.Porc_dto5 = DB.DFloat(item["Porc_dto5"].ToString());
            mi.Imp_dto5 = DB.DFloat(item["Imp_dto5"].ToString());
            mi.Tipo_dto5 = DB.DInt(item["Tipo_dto5"].ToString());
            mi.Id_Google_Ad_Manager=  DB.DLong(item["id_google_ad_manager"].ToString());
            mi.Ron = (item["ron"].ToString()=="1");
            mi.Id_det_conv = DB.DInt(item["id_det_conv"].ToString());
            //AGREGUE:
            if (item["Id_area"].ToString() != "")
            {
                mi.areaGeo = Dg_areas_geo.getById(DB.DInt(item["Id_area"].ToString()));
            }
            mi.Id_red = DB.DInt(item["Id_red"].ToString());

            mi.Medios = new List<Dg_orden_pub_medios>();
            // Medios
            DataTable dm = DB.Select("select id_op_dg, id_detalle, id_medio, porcentaje from dg_orden_pub_medios where id_op_dg = " +
                                      mi.Id_op_dg.ToString() + " and id_detalle = " + mi.Id_detalle.ToString() );
            foreach (DataRow m in dm.Rows)
            {
                //Dg_orden_pub_medios elem = new Dg_orden_pub_medios();
                //elem.Id_op_dg = mi.Id_op_dg;
                //elem.Id_detalle = mi.Id_detalle;
                //elem.Porcentaje = DB.DFloat(m["porcentaje"].ToString());

                mi.Medios.Add(Dg_orden_pub_medios.getDg_orden_pub_medios(m));
            }

            //AGREGUE:
            // Medidas
            mi.Medidas = new List<Dg_orden_pub_medidas>();
 
            DataTable dme = DB.Select("select id_op_dg, id_detalle, id_medidadigital, ancho, alto, tipo from dg_orden_pub_medidas" +
                                      " where id_op_dg = " + mi.Id_op_dg.ToString() + " and id_detalle = " + mi.Id_detalle.ToString() );
            foreach (DataRow m in dme.Rows)
            {
                mi.Medidas.Add(Dg_orden_pub_medidas.getDg_orden_pub_medidas(m));
            }
            // Emplazamientos
            mi.Emplazamientos = new List<Dg_orden_pub_emplazamientos>();

            DataTable de = DB.Select("select id_op_dg, id_detalle, id_emplazamiento, descripcion, codigo_emplazamiento from dg_orden_pub_emplazamientos" +
                                     " where id_op_dg = " + mi.Id_op_dg.ToString() + " and id_detalle = " + mi.Id_detalle.ToString());
            foreach (DataRow m in de.Rows)
            {
                mi.Emplazamientos.Add(Dg_orden_pub_emplazamientos.getDg_orden_pub_emplazamientos(m));
            }

            return mi;           
        }

        //AGREGUE (id_area):
        public bool save()
        {
            bool progisnull = (Id_programa == 0);

            string sql = "insert into dg_orden_pub_as (id_op_dg, id_detalle, anio, mes, nro_orden, fecha_desde, fecha_hasta," +
                        "id_producto, descripcion,";
            if (!progisnull)
            {
                sql += " id_programa,";
            }
            sql += " id_tarifa_dg, tarifa_manual, id_tipo_aviso_dg," +
            "tipo_tarifa, imp_tarifa, importe_unitario, cantidad, monto_bruto, porc_dto,monto_neto, netomanual," +
            "porcconfnc, porcconffc, impconffc, impconfnc, porc_dto1, imp_dto1, id_mtvo_dto1, tipo_dto1, porc_dto2, imp_dto2, id_mtvo_dto2, tipo_dto2," +
            "porc_dto3, imp_dto3, id_mtvo_dto3, tipo_dto3, porc_dto4, imp_dto4, id_mtvo_dto4, tipo_dto4, porc_dto5, imp_dto5, id_mtvo_dto5, tipo_dto5,id_google_ad_manager, ron, id_area, id_det_conv, id_red) " +
            " values (@id_op_dg, @id_detalle, @anio, @mes, @nro_orden, @fecha_desde, @fecha_hasta," +
            "@id_producto, @descripcion,";
            if (!progisnull)
            {
                sql += " @id_programa,";
            }
            sql += " @id_tarifa_dg, @tarifa_manual, @id_tipo_aviso_dg," +
            "@tipo_tarifa, @imp_tarifa, @importe_unitario, @cantidad, @monto_bruto, @porc_dto,@monto_neto, @netomanual," +
            "@porcconfnc, @porcconffc, @impconffc, @impconfnc, @porc_dto1, @imp_dto1, @id_mtvo_dto1, @tipo_dto1, @porc_dto2, @imp_dto2, @id_mtvo_dto2, @tipo_dto2," +
            "@porc_dto3, @imp_dto3, @id_mtvo_dto3, @tipo_dto3, @porc_dto4, @imp_dto4, @id_mtvo_dto4, @tipo_dto4, @porc_dto5, @imp_dto5, @id_mtvo_dto5, @tipo_dto5,@id_google_ad_manager, @ron, @id_area, @id_det_conv, @id_red)";

            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                    new SqlParameter()
                    { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                    new SqlParameter()
                    { ParameterName="@anio",SqlDbType = SqlDbType.Int, Value = Anio },
                    new SqlParameter()
                    { ParameterName="@mes",SqlDbType = SqlDbType.Int, Value = Mes },
                    new SqlParameter()
                    { ParameterName="@nro_orden",SqlDbType = SqlDbType.Int, Value = Nro_orden },
                    new SqlParameter()
                    { ParameterName="@fecha_desde",SqlDbType = SqlDbType.DateTime, Value = Fecha_desde },
                    new SqlParameter()
                    { ParameterName="@fecha_hasta",SqlDbType = SqlDbType.DateTime, Value = Fecha_hasta },
                    new SqlParameter()
                    { ParameterName="@id_producto",SqlDbType = SqlDbType.Int, Value = Id_producto },
                    new SqlParameter()
                    { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                    new SqlParameter()
                    {ParameterName = "@id_programa",SqlDbType = SqlDbType.NVarChar, Value = Id_programa},
                    new SqlParameter()
                    { ParameterName="@id_tarifa_dg",SqlDbType = SqlDbType.Int, Value = tarifa_dg.Id_tarifa_dg },
                    new SqlParameter()
                    { ParameterName="@tarifa_manual",SqlDbType = SqlDbType.Float, Value = Tarifa_manual },
                    new SqlParameter()
                    { ParameterName="@id_tipo_aviso_dg",SqlDbType = SqlDbType.Int, Value = tipo_aviso_dg.Id_tipo_aviso_dg },
                    new SqlParameter()
                    { ParameterName="@tipo_tarifa",SqlDbType = SqlDbType.Int, Value = Tipo_tarifa },
                    new SqlParameter()
                    { ParameterName="@imp_tarifa",SqlDbType = SqlDbType.Float, Value = Imp_tarifa },
                    new SqlParameter()
                    { ParameterName="@importe_unitario",SqlDbType = SqlDbType.Float, Value = Importe_unitario },
                    new SqlParameter()
                    { ParameterName="@cantidad",SqlDbType = SqlDbType.Int, Value = Cantidad },
                    new SqlParameter()
                    { ParameterName="@monto_bruto",SqlDbType = SqlDbType.Float, Value = Monto_bruto },
                    new SqlParameter()
                    { ParameterName="@porc_dto",SqlDbType = SqlDbType.Float, Value = Porc_dto },
                    new SqlParameter()
                    { ParameterName="@monto_neto",SqlDbType = SqlDbType.Float, Value = Monto_neto },
                    new SqlParameter()
                    { ParameterName="@netomanual",SqlDbType = SqlDbType.Float, Value = Netomanual },
                    new SqlParameter()
                    { ParameterName="@porcconfnc",SqlDbType = SqlDbType.Float, Value = Porcconfnc },
                    new SqlParameter()
                    { ParameterName="@porcconffc",SqlDbType = SqlDbType.Float, Value = Porcconffc },
                    new SqlParameter()
                    { ParameterName="@impconffc",SqlDbType = SqlDbType.Float, Value = Impconffc },
                    new SqlParameter()
                    { ParameterName="@impconfnc",SqlDbType = SqlDbType.Float, Value = Impconfnc },
                    new SqlParameter()
                    { ParameterName="@porc_dto1",SqlDbType = SqlDbType.Float, Value = Porc_dto1 },
                    new SqlParameter()
                    { ParameterName="@imp_dto1",SqlDbType = SqlDbType.Float, Value = Imp_dto1 },
                    new SqlParameter()
                    { ParameterName="@id_mtvo_dto1",SqlDbType = SqlDbType.Int, Value = Id_mtvo_dto1 },
                    new SqlParameter()
                    { ParameterName="@tipo_dto1",SqlDbType = SqlDbType.Int, Value = Tipo_dto1 },
                    new SqlParameter()
                    { ParameterName="@porc_dto2",SqlDbType = SqlDbType.Float, Value = Porc_dto2 },
                    new SqlParameter()
                    { ParameterName="@imp_dto2",SqlDbType = SqlDbType.Float, Value = Imp_dto2 },
                    new SqlParameter()
                    { ParameterName="@id_mtvo_dto2",SqlDbType = SqlDbType.Int, Value = Id_mtvo_dto2 },
                    new SqlParameter()
                    { ParameterName="@tipo_dto2",SqlDbType = SqlDbType.Int, Value = Tipo_dto2 },
                    new SqlParameter()
                    { ParameterName="@porc_dto3",SqlDbType = SqlDbType.Float, Value = Porc_dto3 },
                    new SqlParameter()
                    { ParameterName="@imp_dto3",SqlDbType = SqlDbType.Float, Value = Imp_dto3 },
                    new SqlParameter()
                    { ParameterName="@id_mtvo_dto3",SqlDbType = SqlDbType.Int, Value = Id_mtvo_dto3 },
                    new SqlParameter()
                    { ParameterName="@tipo_dto3",SqlDbType = SqlDbType.Int, Value = Tipo_dto3 },
                    new SqlParameter()
                    { ParameterName="@porc_dto4",SqlDbType = SqlDbType.Float, Value = Porc_dto4 },
                    new SqlParameter()
                    { ParameterName="@imp_dto4",SqlDbType = SqlDbType.Float, Value = Imp_dto4 },
                    new SqlParameter()
                    { ParameterName="@id_mtvo_dto4",SqlDbType = SqlDbType.Int, Value = Id_mtvo_dto4 },
                    new SqlParameter()
                    { ParameterName="@tipo_dto4",SqlDbType = SqlDbType.Int, Value = Tipo_dto4 },
                    new SqlParameter()
                    { ParameterName="@porc_dto5",SqlDbType = SqlDbType.Float, Value = Porc_dto5 },
                    new SqlParameter()
                    { ParameterName="@imp_dto5",SqlDbType = SqlDbType.Float, Value = Imp_dto5 },
                    new SqlParameter()
                    { ParameterName="@id_mtvo_dto5",SqlDbType = SqlDbType.Int, Value = Id_mtvo_dto5 },
                    new SqlParameter()
                    { ParameterName="@tipo_dto5",SqlDbType = SqlDbType.Int, Value = Tipo_dto5 },
                    new SqlParameter()
                    { ParameterName="@id_google_ad_manager",SqlDbType = SqlDbType.BigInt, Value = Id_Google_Ad_Manager},
                    new SqlParameter()
                    { ParameterName="@ron",SqlDbType = SqlDbType.Int, Value = Ron}
                };
            if (areaGeo != null)
            {
                parametros.Add(new SqlParameter(){ ParameterName = "@id_area", SqlDbType = SqlDbType.Int, Value = areaGeo.Id_area });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@id_area", SqlDbType = SqlDbType.Int, Value = 0 });
            }
            if (DB.DInt(Id_det_conv) == 0)
            {
                parametros.Add(new SqlParameter() { ParameterName = "@id_det_conv", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@id_det_conv", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_det_conv) });
            }
            if (DB.DInt(Id_red) == 0)
            {
                parametros.Add(new SqlParameter() { ParameterName = "@id_red", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@id_red", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_red) });
            }

            try
            {
                DB.Execute(sql, parametros);
                // Grabo detalle de Medios
                sql = "insert into dg_orden_pub_medios (id_op_dg, id_detalle, id_medio, porcentaje)" +
                      " values (@id_op_dg, @id_detalle, @id_medio, @porcentaje)";
                int count = Medios.Count();
                int i = 0;
                float porc = ((float)100.00 / (float)count);
                porc = (float)Math.Round(porc,2);
                //float porcSum=0;
                foreach (Dg_orden_pub_medios elem in Medios)
                {
                    
                    //elem.Porcentaje = porc;
                    //if (++i == count) {
                    //    elem.Porcentaje = 100- porcSum;
                    //}
                    //porcSum = porcSum + porc;
                    List<SqlParameter> parametrosd = new List<SqlParameter>()
                    {
                        new SqlParameter()
                        { ParameterName = "@id_op_dg", SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                        new SqlParameter()
                        { ParameterName = "@id_detalle", SqlDbType = SqlDbType.Int, Value = Id_detalle },
                        new SqlParameter()
                        { ParameterName = "@id_medio", SqlDbType = SqlDbType.Int, Value = elem.Id_medio },
                        new SqlParameter()
                        { ParameterName = "@porcentaje", SqlDbType = SqlDbType.Float, Value = elem.Porcentaje }
                    };
                    DB.Execute(sql, parametrosd);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (ex);
            }

            //AGREGUE:
            // Grabo detalle de Medidas
            try
            {
                sql = "insert into dg_orden_pub_medidas(id_op_dg, id_detalle, id_medidadigital, ancho, alto, tipo) " +
                      "values (@id_op_dg, @id_detalle, @id_medidadigital, @ancho, @alto, 0)";

                if (Medidas.Count != 0 && Medidas != null )
                {          

                 foreach (Dg_orden_pub_medidas elem in Medidas)
                 {
                        List<SqlParameter> parametrost = new List<SqlParameter>()
                        {
                                new SqlParameter()
                                { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                                new SqlParameter()
                                { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                                new SqlParameter()
                                { ParameterName="@id_medidadigital",SqlDbType = SqlDbType.Int, Value = elem.Id_medidadigital },
                                new SqlParameter()
                                { ParameterName="@ancho",SqlDbType = SqlDbType.Int, Value = elem.Ancho },
                                new SqlParameter()
                                { ParameterName="@alto",SqlDbType = SqlDbType.Int, Value = elem.Alto }
                                //new SqlParameter()
                                //{ ParameterName="@tipo",SqlDbType = SqlDbType.Int, Value = elem.Tipo }
                        };
                        DB.Execute(sql, parametrost);
                 }    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (ex);
            }

            //AGREGUE:
            // Grabo detalle de Emplazamientos
            try
            {
                sql = "insert into dg_orden_pub_emplazamientos(id_op_dg, id_detalle, id_emplazamiento, descripcion, codigo_emplazamiento) " +
                      "values (@id_op_dg, @id_detalle, @id_emplazamiento, @descripcion, @codigo_emplazamiento)";

                if (Emplazamientos.Count != 0 && Emplazamientos != null)
                {

                    foreach (Dg_orden_pub_emplazamientos elem in Emplazamientos)
                    {
                        List<SqlParameter> parametrost = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op_dg },
                            new SqlParameter()
                            { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                            new SqlParameter()
                            { ParameterName="@id_emplazamiento",SqlDbType = SqlDbType.Int, Value = elem.Id_emplazamiento },
                            new SqlParameter()
                            { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = elem.Descripcion },
                            new SqlParameter()
                            { ParameterName="@codigo_emplazamiento", SqlDbType = SqlDbType.BigInt, Value = elem.Codigo_emplazamiento }
                        };
                        DB.Execute(sql, parametrost);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (ex);
            }

            return true;
        }

        //public static Dg_orden_pub_ap getIdAdSById(int Id)
        //{
        //    string sqlCommand = " select dg.id_op_dg, dg.anio, dg.mes, dg.nro_orden, dg.id_empresa, dg.id_medio, dg.es_varios_medios, dg.fecha, " +
        //                        " dg.fecha_expiracion, dg.id_agencia, dg.id_anunciante, dg.id_producto, dg.id_condpagoap, dg.nro_orden_ag, " +
        //                        " dg.facturar_a, dg.tipo_orden, dg.observ, dg.monto_bruto, dg.monto_bonif, dg.monto_dto, dg.primer_neto, dg.es_anulada, " +
        //                        " dg.fecha_anulada, dg.monto_desc_ag, dg.monto_desc_an, dg.monto_desc_agan, dg.monto_desc_fact, dg.monto_desc_difag, " +
        //                        " dg.monto_desc_restfa, dg.nro_orden_imp, dg.porc_dto, dg.porc_conf, dg.seg_neto, dg.fecha_alta, dg.id_concepto_negocio, " +
        //                        " dg.id_usuario, dg.porcvol_ag, dg.tercer_neto, dg.localnacional, dg.imp_conf_nc, dg.imp_conf_fc, dg.porcvol_an, dg.id_op_relacionada, " +
        //                        " dg.Desc2, dg.Desc3, dg.Desc4, dg.desc5, dg.id_cond_iva, dg.id_moneda, dg.cambio, dg.bonificado, dg.id_convenio, dg.transferido, dg.bitacora, dg.Id_facturar, dg.Total_Avisos, dg.Total_Impresiones, " +
        //                        " dg.Es_facturada, dg.Id_factura_ap, " +
        //                        " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre, " +
        //                        " cast(op.anio as varchar(4)) + '-' + cast(op.mes as varchar(2)) + '-' + cast(op.nro_orden as varchar(5)) as nro_orden_rel " +
        //                        " from Dg_orden_pub_ap dg " +
        //                        " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
        //                        " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
        //                        " left outer join productos p on p.id_producto = dg.id_producto " +
        //                        " left outer join orden_pub_ap op on dg.id_op_relacionada = op.id_op " +
        //                        " where dg.id_op_dg = " + Id.ToString();
        //    Dg_orden_pub_ap resultado;
        //    resultado = new Dg_orden_pub_ap();
        //    DataTable t = DB.Select(sqlCommand);

        //    if (t.Rows.Count == 1)
        //    {
        //        resultado = getDg_orden_pub_ap(t.Rows[0]);
        //        // Detalles ...
        //        resultado.Detalles = new List<Dg_orden_pub_as>();
        //        string strSql = " select id_op_dg, id_detalle, anio, mes, nro_orden, fecha_desde, fecha_hasta, id_producto, descripcion, id_programa, id_tarifa_dg," +
        //                            " tarifa_manual, id_tipo_aviso_dg, tipo_tarifa, imp_tarifa, importe_unitario, cantidad, monto_bruto, porc_dto," +
        //                            " monto_neto, netomanual, porcconfnc, porcconffc, impconfnc, impconffc," +
        //                            " porc_dto1, imp_dto1, id_mtvo_dto1, tipo_dto1, porc_dto2, imp_dto2, id_mtvo_dto2, tipo_dto2," +
        //                            " porc_dto3, imp_dto3, id_mtvo_dto3, tipo_dto3, porc_dto4, imp_dto4, id_mtvo_dto4, tipo_dto4," +
        //                            " porc_dto5, imp_dto5, id_mtvo_dto5, tipo_dto5,id_google_ad_manager, ron from Dg_orden_pub_as where id_op_dg = " + resultado.Id_op_dg.ToString();
        //        DataTable td = DB.Select(strSql);
        //        Dg_orden_pub_as det;
        //        foreach (DataRow r in td.Rows)
        //        {
        //            det = Dg_orden_pub_as.getDg_orden_pub_as(r);
        //            resultado.Detalles.Add(det);
        //        }
        //        // Ejecutivos
        //        resultado.Ejecutivos = new List<Dg_orden_pub_ejecutivos>();
        //        td = DB.Select("select id_op_dg, id_ejecutivo, anio, mes, nro_orden, tipo_rol, porcentaje, tipo_ejecutivo, tipo_rol_padre " +
        //                        " from Dg_orden_pub_ejecutivos where id_op_dg = " + resultado.Id_op_dg.ToString());
        //        Dg_orden_pub_ejecutivos dej;
        //        foreach (DataRow r in td.Rows)
        //        {
        //            dej = new Dg_orden_pub_ejecutivos();
        //            dej.Id_op_dg = resultado.Id_op_dg;
        //            dej.Id_ejecutivo = DB.DInt(r["id_ejecutivo"].ToString());
        //            dej.Anio = DB.DInt(r["anio"].ToString());
        //            dej.Mes = DB.DInt(r["mes"].ToString());
        //            dej.Nro_orden = DB.DInt(r["nro_orden"].ToString());
        //            dej.Tipo_rol = DB.DInt(r["tipo_rol"].ToString());
        //            dej.Porcentaje = DB.DFloat(r["porcentaje"].ToString());
        //            dej.Tipo_ejecutivo = DB.DInt(r["Tipo_ejecutivo"].ToString());
        //            dej.Tipo_rol_padre = DB.DInt(r["tipo_rol_padre"].ToString());
        //            resultado.Ejecutivos.Add(dej);
        //        }
        //        // FormasPago
        //        resultado.FormasPago = new List<Dg_orden_pub_pagos>();
        //        td = DB.Select("select id_op_dg, anio, mes, nro_orden, id_formapago, porcentaje " +
        //                    " from Dg_orden_pub_pagos where id_op_dg = " + resultado.Id_op_dg.ToString());
        //        Dg_orden_pub_pagos dfp;
        //        foreach (DataRow r in td.Rows)
        //        {
        //            dfp = new Dg_orden_pub_pagos();
        //            dfp.Id_op_dg = resultado.Id_op_dg;
        //            dfp.Mes = DB.DInt(r["mes"].ToString());
        //            dfp.Anio = DB.DInt(r["anio"].ToString());
        //            dfp.Nro_orden = DB.DInt(r["nro_orden"].ToString());
        //            dfp.forma_pago = Forma_Pago.getById(DB.DInt(r["id_formapago"].ToString()));
        //            dfp.Porcentaje = DB.DFloat(r["porcentaje"].ToString());
        //            resultado.FormasPago.Add(dfp);
        //        }
        //    }
        //    return resultado;
        //}

        //AGREGUE:
        public static void saveId_Google_Ad_Manager(int idOp, int idDet, long idGam)
        {
            string sql = "";

            sql = "UPDATE dg_orden_pub_as SET id_google_ad_manager = @idgam WHERE id_op_dg = @id_op_dg AND id_detalle = @id_det";


            List<SqlParameter> parametrost = new List<SqlParameter>()
                        {
                                new SqlParameter()
                                { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = idOp },
                                 new SqlParameter()
                                { ParameterName="@id_det",SqlDbType = SqlDbType.Int, Value = idDet },
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

        //AGREGUE:
        public static List<Dg_orden_pub_as> getSponsorsPorFecha(Dg_orden_pub_as det)
        {
            List<Dg_orden_pub_as> col = new List<Dg_orden_pub_as>();
            Dg_orden_pub_as elem;

            foreach (Dg_orden_pub_emplazamientos emp in det.Emplazamientos)
            {
                string sqlCommand = @"SELECT o.fecha_desde, o.fecha_hasta, e.descripcion FROM dg_orden_pub_as o 
                                      INNER JOIN dg_orden_pub_emplazamientos e on o.id_op_dg=e.id_op_dg and o.id_detalle=e.id_detalle 
                                      WHERE o.tipo_tarifa = 1 
								      AND (o.fecha_desde <= @fechaH AND o.fecha_hasta >= @fechaD) 
								      AND e.id_emplazamiento = @emplaza
                                      ORDER BY o.fecha_desde";

                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@fechaD",SqlDbType = SqlDbType.DateTime, Value = det.Fecha_desde },
                    new SqlParameter()
                    { ParameterName="@fechaH",SqlDbType = SqlDbType.DateTime, Value = det.Fecha_hasta },
                     new SqlParameter()
                    { ParameterName="@emplaza",SqlDbType = SqlDbType.Int, Value = emp.Id_emplazamiento }
                };

                DataTable t = DB.Select(sqlCommand, parametros);

                foreach (DataRow item in t.Rows)
                {
                    Dg_orden_pub_emplazamientos emplaza = new Dg_orden_pub_emplazamientos();
                    emplaza.Descripcion = item["descripcion"].ToString();
                    List<Dg_orden_pub_emplazamientos> emplazas = new List<Dg_orden_pub_emplazamientos>();
                    emplazas.Add(emplaza);
                    elem = new Dg_orden_pub_as
                    {
                        Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString()),
                        Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString()),
                        Emplazamientos = emplazas
                    };
                col.Add(elem);
                }
            }
            return col;
        }

        public static Dg_orden_pub_as getByIdGam(long idGam, int idRed)
        {
            string strSql = " select id_op_dg, id_detalle, anio, mes, nro_orden, fecha_desde, fecha_hasta, id_producto, descripcion, id_programa, id_tarifa_dg," +
                                   " tarifa_manual, id_tipo_aviso_dg, tipo_tarifa, imp_tarifa, importe_unitario, cantidad, monto_bruto, porc_dto," +
                                   " monto_neto, netomanual, porcconfnc, porcconffc, impconfnc, impconffc," +
                                   " porc_dto1, imp_dto1, id_mtvo_dto1, tipo_dto1, porc_dto2, imp_dto2, id_mtvo_dto2, tipo_dto2," +
                                   " porc_dto3, imp_dto3, id_mtvo_dto3, tipo_dto3, porc_dto4, imp_dto4, id_mtvo_dto4, tipo_dto4," +
                                   " porc_dto5, imp_dto5, id_mtvo_dto5, tipo_dto5,id_google_ad_manager, ron, id_area, id_det_conv, id_red from Dg_orden_pub_as where id_google_ad_manager = " + idGam.ToString() + " and id_red = " + idRed.ToString();
            DataTable td = DB.Select(strSql);
            Dg_orden_pub_as det = new Dg_orden_pub_as();
            if (td.Rows.Count == 1)
            {
                det = getDg_orden_pub_as(td.Rows[0]);
            }
            return det;
        }

        public static List<Dg_orden_pub_as> getByIdOp(int idOp)
        {
            List<Dg_orden_pub_as> detalles = new List<Dg_orden_pub_as>();
            
            string strSql = " select id_op_dg, id_detalle, anio, mes, nro_orden, fecha_desde, fecha_hasta, id_producto, descripcion, id_programa, id_tarifa_dg," +
                                " tarifa_manual, id_tipo_aviso_dg, tipo_tarifa, imp_tarifa, importe_unitario, cantidad, monto_bruto, porc_dto," +
                                " monto_neto, netomanual, porcconfnc, porcconffc, impconfnc, impconffc," +
                                " porc_dto1, imp_dto1, id_mtvo_dto1, tipo_dto1, porc_dto2, imp_dto2, id_mtvo_dto2, tipo_dto2," +
                                " porc_dto3, imp_dto3, id_mtvo_dto3, tipo_dto3, porc_dto4, imp_dto4, id_mtvo_dto4, tipo_dto4," +
                                " porc_dto5, imp_dto5, id_mtvo_dto5, tipo_dto5,id_google_ad_manager, ron, id_area, id_det_conv, id_red from Dg_orden_pub_as where id_op_dg = " + idOp.ToString();
            
            DataTable td = DB.Select(strSql);
            Dg_orden_pub_as det;
            foreach (DataRow r in td.Rows)
            {
                det = getDg_orden_pub_as(r);
                detalles.Add(det);
            }
            return detalles;
        }

        public static bool existeDetGAMenBD(long idDetGAM, int idRed)
        {
            bool existe = false;
            string sqlCommand = "SELECT * FROM dg_orden_pub_as WHERE id_google_ad_manager=@idgam and id_red=@idRed";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@idgam",SqlDbType = SqlDbType.BigInt, Value = idDetGAM },
                 new SqlParameter()
                { ParameterName="@idRed",SqlDbType = SqlDbType.Int, Value = idRed }
            };

            DataTable t = DB.Select(sqlCommand, parametros);
            if (t.Rows.Count == 1)
            {
                existe = true;
            }
            return existe;
        }

        public static List<Dg_orden_pub_as> getAll()
        {
            List<Dg_orden_pub_as> detalles = new List<Dg_orden_pub_as>();

            string strSql = "SELECT id_google_ad_manager, id_red FROM dg_orden_pub_as";

            DataTable td = DB.Select(strSql);
            Dg_orden_pub_as det = new Dg_orden_pub_as();
            foreach (DataRow r in td.Rows)
            {
                det.Id_Google_Ad_Manager = long.Parse(r["id_google_ad_manager"].ToString());
                det.Id_red = int.Parse(r["id_red"].ToString());
                detalles.Add(det);
            }
            return detalles;
        }

    }
}
