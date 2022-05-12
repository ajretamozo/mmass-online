using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Conv_dg_detalle_forma_uso
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public static Conv_dg_detalle_forma_uso getFormaUso(int indice)
        {
            if ((indice < 0) || (indice > 4))
            {
                return null;
            }
            else
            {
                List<Conv_dg_detalle_forma_uso> col = new List<Conv_dg_detalle_forma_uso>();
                Conv_dg_detalle_forma_uso item = new Conv_dg_detalle_forma_uso();
                item.Id = 0;
                item.Descripcion = "CPM";
                col.Add(item);
                item = new Conv_dg_detalle_forma_uso();
                item.Id = 1;
                item.Descripcion = "CPD (Sponsor)";
                col.Add(item);
                item = new Conv_dg_detalle_forma_uso();
                item.Id = 2;
                item.Descripcion = "Posteo";
                col.Add(item);
                item = new Conv_dg_detalle_forma_uso();
                item.Id = 3;
                item.Descripcion = "CPC";
                col.Add(item);
                item = new Conv_dg_detalle_forma_uso();
                item.Id = 4;
                item.Descripcion = "CPA";
                col.Add(item);

                return col[indice];
            }
        }
        public static List<Conv_dg_detalle_forma_uso> getAll()
        {
            List<Conv_dg_detalle_forma_uso> col = new List<Conv_dg_detalle_forma_uso>();
            Conv_dg_detalle_forma_uso item = new Conv_dg_detalle_forma_uso();
            item.Id = 0;
            item.Descripcion = "CPM";
            col.Add(item);
            item = new Conv_dg_detalle_forma_uso();
            item.Id = 1;
            item.Descripcion = "CPD (Sponsor)";
            col.Add(item);
            item = new Conv_dg_detalle_forma_uso();
            item.Id = 2;
            item.Descripcion = "Posteo";
            col.Add(item);
            item = new Conv_dg_detalle_forma_uso();
            item.Id = 3;
            item.Descripcion = "CPC";
            col.Add(item);
            item = new Conv_dg_detalle_forma_uso();
            item.Id = 4;
            item.Descripcion = "CPA";
            col.Add(item);
            return col;
        }
    }
    public class Conv_dg_detalle
    {
        public int Id_det_conv { get; set; }
        public int Id_convenio { get; set; }
        public int Id_detalle { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Fecha_desde { get; set; }
        public DateTime? Fecha_hasta { get; set; }
        //public int Id_tipo_aviso_dg { get; set; }
        public Conv_dg_detalle_forma_uso Forma_uso { get; set; }
        public float Precio_unitario { get; set; }
        public Dg_tarifas Tarifa_dg { get; set; } 
        public float Porc_desc { get; set; }
        public float Porc_conf_nc { get; set; }
        public float Porc_conf_fc { get; set; }
        public int Id_area { get; set; }
        public int Id_empresa { get; set; }
        public List<Dg_tipos_avisos> Tipos_aviso;
        public List<Dg_orden_pub_medios> Medios;
        public List<Dg_emplazamientos> Emplazamientos;
        public List<Dg_medidas> Medidas;

       
        public static Conv_dg_detalle getById(int id_convenio, int id_detalle)
        {
            string sqlCommand = " select id_det_conv, id_convenio, id_detalle, descripcion, fecha_desde, fecha_hasta, forma_uso, id_tarifa_dg, precio_unitario," +
                                " porc_desc, porc_conf_nc, porc_conf_fc, id_area" +
                                " from conv_dg_detalle where id_convenio = " + id_convenio.ToString() + " and id_detalle = " + id_detalle.ToString();
           
            Conv_dg_detalle resultado = null;
            DataTable t = DB.Select(sqlCommand);
            DataTable det = new DataTable();

            if (t.Rows.Count == 1)
            {
                DataRow item = t.Rows[0];
                resultado = new Conv_dg_detalle();

                resultado.Id_det_conv = int.Parse(item["id_det_conv"].ToString());
                resultado.Id_convenio = int.Parse(item["id_convenio"].ToString());
                resultado.Id_detalle = int.Parse(item["id_detalle"].ToString());
                if (item["descripcion"].ToString() != "")
                {
                    resultado.Descripcion = item["descripcion"].ToString();
                }
                resultado.Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString());
                resultado.Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString());
                //if (item["id_tipo_aviso_dg"].ToString() != "")
                //{
                //    resultado.Id_tipo_aviso_dg = int.Parse(item["id_tipo_aviso_dg"].ToString());
                //}
                resultado.Forma_uso = Conv_dg_detalle_forma_uso.getFormaUso(DB.DInt(item["forma_uso"].ToString()));
                resultado.Precio_unitario = float.Parse(item["precio_unitario"].ToString());
                resultado.Tarifa_dg = Dg_tarifas.getById(DB.DInt(item["id_tarifa_dg"].ToString()));
                if (item["porc_desc"].ToString() != "")
                {
                    resultado.Porc_desc = float.Parse(item["porc_desc"].ToString());
                }
                if (item["porc_conf_nc"].ToString() != "")
                {
                    resultado.Porc_conf_nc = float.Parse(item["porc_conf_nc"].ToString());
                }
                if (item["porc_conf_fc"].ToString() != "")
                {
                    resultado.Porc_conf_fc = float.Parse(item["porc_conf_fc"].ToString());
                }
                if (item["id_area"].ToString() != "")
                {
                    resultado.Id_area = int.Parse(item["id_area"].ToString());
                }
                // Tipos de Aviso
                resultado.Tipos_aviso = new List<Dg_tipos_avisos>();
                det = DB.Select(@"select ta.* 
                                  from dg_conv_dg_detalle_tipo_Avisos cta 
                                  inner join categorias ta on ta.id_categoria = cta.id_tipo_aviso_dg
                                  where cta.id_convenio = " + id_convenio.ToString() + " and cta.id_detalle = " + id_detalle.ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    Dg_tipos_avisos elem = Dg_tipos_avisos.getDg_tipos_avisos(item2);
                    resultado.Tipos_aviso.Add(elem);
                }
                // Medios
                resultado.Medios = new List<Dg_orden_pub_medios>();
                det = DB.Select("select cm.id_convenio as id_op_dg, cm.id_detalle, cm.porcentaje, m.* from dg_conv_dg_detalle_medios cm inner join medios m on m.id_medio = cm.id_medio where cm.id_convenio = " + id_convenio.ToString() + " and cm.id_detalle = " + id_detalle.ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    //Medio elem = Medio.getMedio(item2);
                    //resultado.Medios.Add(elem);
                    resultado.Medios.Add(Dg_orden_pub_medios.getDg_orden_pub_medios(item2));
                }
                // Emplazamientos
                resultado.Emplazamientos = new List<Dg_emplazamientos>();
                det = DB.Select(@"select ce.id_convenio, ce.id_detalle, e.* 
                                  from dg_conv_dg_detalle_emplazamientos ce 
                                  inner join dg_emplazamientos e on e.id_emplazamiento = ce.id_emplazamiento
                                  where ce.id_convenio = " + id_convenio.ToString() + " and ce.id_detalle = " + id_detalle.ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    Dg_emplazamientos elem = Dg_emplazamientos.getDg_emplazamientos(item2);
                    resultado.Emplazamientos.Add(elem);
                }
                // Medidas
                resultado.Medidas = new List<Dg_medidas>();
                det = DB.Select(@"select cme.id_convenio, cme.id_detalle, m.* 
                                  from dg_conv_dg_detalle_medidas cme 
                                  inner join dg_medidas m on m.id_medidadigital = cme.id_medidadigital 
                                  where cme.id_convenio = " + id_convenio.ToString() + " and cme.id_detalle = " + id_detalle.ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    Dg_medidas elem = Dg_medidas.getDg_medidas(item2);
                    resultado.Medidas.Add(elem);
                }
            }
            return resultado;
        }


        public List<Conv_dg_detalle> getByIdConv()
        {
            //string sqlCommand = " select id_det_conv, id_convenio, id_detalle, descripcion, fecha_desde, fecha_hasta, id_tipo_aviso_dg, forma_uso, precio_unitario," +
            //                    " porc_desc, porc_conf_nc, porc_conf_fc, id_area" +
            //                    " from conv_dg_detalle where id_convenio = " + id_convenio.ToString();

            string sqlCommand = @"select distinct id_det_conv, cd.id_convenio, cd.id_detalle, descripcion, fecha_desde, fecha_hasta, id_tipo_aviso_dg,
                                  forma_uso, precio_unitario, porc_desc, porc_conf_nc, porc_conf_fc, id_area 
                                  from conv_dg_detalle cd 
                                  inner join dg_conv_dg_detalle_medios cdm on cdm.id_convenio=cd.id_convenio 
                                  and cdm.id_detalle=cd.id_detalle inner join medios m on m.ID_MEDIO=cdm.id_medio 
                                  where m.ID_EMPRESA = " + Id_empresa + " and cd.id_convenio = " + Id_convenio;

            List<Conv_dg_detalle> col = new List<Conv_dg_detalle>();
            Conv_dg_detalle elem;
            DataTable det = new DataTable();
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Conv_dg_detalle();

                elem.Id_det_conv = int.Parse(item["id_det_conv"].ToString());
                elem.Id_convenio = int.Parse(item["id_convenio"].ToString());
                elem.Id_detalle = int.Parse(item["id_detalle"].ToString());
                if (item["descripcion"].ToString() != "")
                {
                    elem.Descripcion = item["descripcion"].ToString();
                }
                if (item["fecha_desde"].ToString() != "")
                {
                    elem.Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString());
                }
                if (item["fecha_hasta"].ToString() != "")
                {
                    elem.Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString());
                }
                //if (item["id_tipo_aviso_dg"].ToString() != "")
                //{
                //    elem.Id_tipo_aviso_dg = int.Parse(item["id_tipo_aviso_dg"].ToString());
                //}
                if (item["forma_uso"].ToString() != "")
                {
                    elem.Forma_uso = Conv_dg_detalle_forma_uso.getFormaUso(DB.DInt(item["forma_uso"].ToString()));
                }
                if (item["precio_unitario"].ToString() != "")
                {
                    elem.Precio_unitario = float.Parse(item["precio_unitario"].ToString());
                }
                if (item["porc_desc"].ToString() != "")
                {
                    elem.Porc_desc = float.Parse(item["porc_desc"].ToString());
                }
                if (item["porc_conf_nc"].ToString() != "")
                {
                    elem.Porc_conf_nc = float.Parse(item["porc_conf_nc"].ToString());
                }
                if (item["porc_conf_fc"].ToString() != "")
                {
                    elem.Porc_conf_fc = float.Parse(item["porc_conf_fc"].ToString());
                }
                if (item["id_area"].ToString() != "")
                {
                    elem.Id_area = int.Parse(item["id_area"].ToString());
                }

                // Tipos de Aviso
                elem.Tipos_aviso = new List<Dg_tipos_avisos>();
                det = DB.Select(@"select ta.* 
                                  from dg_conv_dg_detalle_tipo_Avisos cta 
                                  inner join categorias ta on ta.id_categoria = cta.id_tipo_aviso_dg
                                  where cta.id_convenio = " + item["id_convenio"].ToString() + " and cta.id_detalle = " + item["id_detalle"].ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    Dg_tipos_avisos ta = Dg_tipos_avisos.getDg_tipos_avisos(item2);
                    elem.Tipos_aviso.Add(ta);
                }
                // Medios
                elem.Medios = new List<Dg_orden_pub_medios>();
                det = DB.Select("select cm.id_convenio as id_op_dg, cm.id_detalle, cm.porcentaje, m.* from dg_conv_dg_detalle_medios cm inner join medios m on m.id_medio = cm.id_medio where cm.id_convenio = " + item["id_convenio"].ToString() + " and cm.id_detalle = " + item["id_detalle"].ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    //Medio med = Medio.getMedio(item2);
                    //elem.Medios.Add(med);
                    elem.Medios.Add(Dg_orden_pub_medios.getDg_orden_pub_medios(item2));
                }
                // Emplazamientos
                elem.Emplazamientos = new List<Dg_emplazamientos>();
                det = DB.Select(@"select ce.id_convenio, ce.id_detalle, e.* 
                                  from dg_conv_dg_detalle_emplazamientos ce 
                                  inner join dg_emplazamientos e on e.id_emplazamiento = ce.id_emplazamiento
                                  where ce.id_convenio = " + item["id_convenio"].ToString() +
                                  " and ce.id_detalle = " + item["id_detalle"].ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    Dg_emplazamientos emp = Dg_emplazamientos.getDg_emplazamientos(item2);
                    elem.Emplazamientos.Add(emp);
                }
                // Medidas
                elem.Medidas = new List<Dg_medidas>();
                det = DB.Select(@"select cme.id_convenio, cme.id_detalle, m.* 
                                  from dg_conv_dg_detalle_medidas cme 
                                  inner join dg_medidas m on m.id_medidadigital = cme.id_medidadigital 
                                  where cme.id_convenio = " + item["id_convenio"].ToString() +
                                  " and cme.id_detalle = " + item["id_detalle"].ToString());
                foreach (DataRow item2 in det.Rows)
                {
                    Dg_medidas medi = Dg_medidas.getDg_medidas(item2);
                    elem.Medidas.Add(medi);
                }

                col.Add(elem);
            }
            return col;
        }

        public static List<Conv_dg_detalle> getAll()
        {
            string sqlCommand = "select id_convenio, id_detalle, descripcion, fecha_desde, fecha_hasta, forma_uso, precio_unitario," +
                                " porc_desc, porc_conf_nc, porc_conf_fc, id_area" +
                                " from conv_dg_detalle ";

            List<Conv_dg_detalle> col = new List<Conv_dg_detalle>();
            Conv_dg_detalle elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Conv_dg_detalle
                {
                    Id_convenio = int.Parse(item["id_convenio"].ToString()),
                    Id_detalle = int.Parse(item["id_detalle"].ToString()),
                    Descripcion = item["descripcion"].ToString(),
                    Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString()),
                    Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString()),
                    //Id_tipo_aviso_dg = int.Parse(item["id_tipo_aviso_dg"].ToString()),
                    Forma_uso = Conv_dg_detalle_forma_uso.getFormaUso(DB.DInt(item["forma_uso"].ToString())),
                    Precio_unitario = float.Parse(item["precio_unitario"].ToString()),
                    Porc_desc = float.Parse(item["porc_desc"].ToString()),
                    Porc_conf_nc = float.Parse(item["porc_conf_nc"].ToString()),
                    Porc_conf_fc = float.Parse(item["porc_conf_fc"].ToString()),
                    Id_area = int.Parse(item["id_area"].ToString()),
                };
                col.Add(elem);
            }
            return col;
        }

        //TERMINAR ESTO:
        public static List<Conv_dg_detalle> filter(List<Parametro> parametros)
        {
            string sqlCommand = "select id_convenio, id_detalle, descripcion, fecha_desde, fecha_hasta, forma_uso, precio_unitario, " +
                                "porc_desc, porc_conf_nc, porc_conf_fc, id_area " +
                                "from conv_dg_detalle " +
                                "where 1=1 ";
            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "descripcion") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and descripcion like '%" + p.Value + "%'";

                    if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                    {
                        DateTime fecha = DateTime.Parse(p.Value);
                        string formatted = fecha.ToString("dd-MM-yyyy");
                        mifiltro = mifiltro + " and fecha_desde >='" + formatted + "'";
                    }
                    if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                    {
                        DateTime fecha = DateTime.Parse(p.Value);
                        string formatted = fecha.ToString("dd-MM-yyyy");
                        mifiltro = mifiltro + " and fecha_hasta <='" + formatted + "'";
                    }
                    //if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                    //    mifiltro = mifiltro + " and fecha_desde >= '" + p.Value.ToString() + "'";
                    //if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                    //    mifiltro = mifiltro + " and fecha_hasta <= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "forma_uso") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and forma_uso = " + p.Value.ToString();
                    if ((p.ParameterName == "id_medio") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_conv_dg_detalle_medios where dg_conv_dg_detalle_medios.id_convenio = conv_dg_detalle.id_convenio and dg_conv_dg_detalle_medios.id_detalle = conv_dg_detalle.id_detalle and id_medio = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "id_tipo_aviso_dg") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_conv_dg_detalle_tipos_avisos_dg where dg_conv_dg_detalle_tipos_avisos_dg.id_convenio = conv_dg_detalle.id_convenio and dg_conv_dg_detalle_tipos_avisos_dg.id_detalle = conv_dg_detalle.id_detalle and id_tipo_aviso_dg = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "id_emplazamiento") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_conv_dg_detalle_emplazamientos where dg_conv_dg_detalle_emplazamientos.id_convenio = conv_dg_detalle.id_convenio and dg_conv_dg_detalle_emplazamientos.id_detalle = conv_dg_detalle.id_detalle and id_emplazamiento = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "id_medidadigital") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_conv_dg_detalle_medidas where dg_conv_dg_detalle_medidas.id_convenio = conv_dg_detalle.id_convenio and dg_conv_dg_detalle_medidas.id_detalle = conv_dg_detalle.id_detalle and id_medidadigital = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "id_areaGeo") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_conv_dg_detalle_areas where dg_conv_dg_detalle_areas.id_convenio = conv_dg_detalle.id_convenio and dg_conv_dg_detalle_areas.id_detalle = conv_dg_detalle.id_detalle and id_area = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "vigente") && (p.Value.ToString() == "1"))
                        mifiltro = mifiltro + " and (getdate() between fecha_desde and fecha_hasta) ";
                }
            }

            List<Conv_dg_detalle> col = new List<Conv_dg_detalle>();
            Conv_dg_detalle elem;
            DataTable t = DB.Select(sqlCommand + mifiltro);

            foreach (DataRow item in t.Rows)
            {
                elem = new Conv_dg_detalle
                {
                    Id_convenio = int.Parse(item["id_convenio"].ToString()),
                    Id_detalle = int.Parse(item["id_detalle"].ToString()),
                    Descripcion = item["descripcion"].ToString(),
                    Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString()),
                    Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString()),
                    //Id_tipo_aviso_dg = int.Parse(item["id_tipo_aviso_dg"].ToString()),
                    Forma_uso = Conv_dg_detalle_forma_uso.getFormaUso(DB.DInt(item["forma_uso"].ToString())),
                    Precio_unitario = float.Parse(item["precio_unitario"].ToString()),
                    Porc_desc = float.Parse(item["porc_desc"].ToString()),
                    Porc_conf_nc = float.Parse(item["porc_conf_nc"].ToString()),
                    Porc_conf_fc = float.Parse(item["porc_conf_fc"].ToString()),
                    Id_area = int.Parse(item["id_area"].ToString())
                };
                col.Add(elem);
            }
            return col;
        }

    }
}
