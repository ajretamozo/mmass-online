using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Convenios
    {
        public int Id_convenio { get; set; }
        public string Desc_convenio { get; set; }
        public int Id_formapago { get; set; }
        public int Id_agencia { get; set; }
        public int Id_anunciante { get; set; }
        public bool Es_borrado { get; set; }
        public float Importe_total { get; set; }
        public DateTime? Fecha_desde { get; set; }
        public DateTime? Fecha_hasta { get; set; }
        public int Estado { get; set; }
        public string Observaciones { get; set; }
        public int Facturar_a { get; set; }
        public string Agencia_nombre { get; set; }
        public string Anunciante_nombre { get; set; }
        public string Formapago_nombre { get; set; }

        //public static Convenios getById(int id_convenio)
        //{
        //    string sqlCommand = @"select c.id_convenio, c.desc_convenio, c.id_formapago, c.id_agencia, c.id_anunciante, c.importe_total,
        //                            c.fecha_desde, c.fecha_hasta, c.estado, c.observaciones, c.facturar_a,
        //                            ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, fp.desc_formapago as formapago_nombre
        //                            from convenio_anual_precios c
        //                            left outer join contactos ag on ag.id_contacto = c.id_agencia
        //                            left outer join contactos an on an.id_contacto = c.id_anunciante
        //    left outer join formas_pago fp on fp.id_formapago = c.id_formapago
        //                            where c.id_convenio = " + id_convenio.ToString() + " and c.es_borrado = 0";

        //    Convenios resultado = new Convenios();
        //    DataTable t = DB.Select(sqlCommand);

        //    if (t.Rows.Count == 1)
        //    {
        //            resultado.Id_convenio = int.Parse(t.Rows[0]["id_convenio"].ToString());
        //            resultado.Desc_convenio = t.Rows[0]["desc_convenio"].ToString();
        //            resultado.Id_formapago = int.Parse(t.Rows[0]["id_formapago"].ToString());
        //            resultado.Id_agencia = int.Parse(t.Rows[0]["id_agencia"].ToString());
        //            resultado.Id_anunciante = int.Parse(t.Rows[0]["id_anunciante"].ToString());
        //            resultado.Importe_total = float.Parse(t.Rows[0]["importe_total"].ToString());
        //            resultado.Fecha_desde = DateTime.Parse(t.Rows[0]["fecha_desde"].ToString());
        //            resultado.Fecha_hasta = DateTime.Parse(t.Rows[0]["fecha_hasta"].ToString());
        //            resultado.Estado = int.Parse(t.Rows[0]["estado"].ToString());
        //            resultado.Observaciones = t.Rows[0]["observaciones"].ToString();
        //            resultado.Facturar_a = int.Parse(t.Rows[0]["facturar_a"].ToString());
        //            if (t.Rows[0]["agencia_nombre"] != null)
        //            {
        //                resultado.Agencia_nombre = t.Rows[0]["agencia_nombre"].ToString();
        //            }
        //            if (t.Rows[0]["anunciante_nombre"] != null)
        //            {
        //                resultado.Anunciante_nombre = t.Rows[0]["anunciante_nombre"].ToString();
        //            }
        //            if (t.Rows[0]["formapago_nombre"] != null)
        //            {
        //                resultado.Formapago_nombre = t.Rows[0]["formapago_nombre"].ToString();
        //            }   
        //    }         
        //    return resultado;
        //}

        public static Convenios getById(int id_convenio)
        {
            string sqlCommand = @"select c.id_convenio, c.desc_convenio, c.id_formapago, c.id_agencia, c.id_anunciante, c.importe_total,
                                    c.fecha_desde, c.fecha_hasta, c.estado, c.observaciones, c.facturar_a,
                                    ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, fp.desc_formapago as formapago_nombre
                                    from convenio_anual_precios c
                                    left outer join contactos ag on ag.id_contacto = c.id_agencia
                                    left outer join contactos an on an.id_contacto = c.id_anunciante
								    left outer join formas_pago fp on fp.id_formapago = c.id_formapago
                                    where c.id_convenio = " + id_convenio.ToString() + " and c.es_borrado = 0";

            Convenios resultado = new Convenios();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado.Id_convenio = int.Parse(t.Rows[0]["id_convenio"].ToString());
                resultado.Desc_convenio = t.Rows[0]["desc_convenio"].ToString();
                if (t.Rows[0]["id_formapago"].ToString() != "")
                {
                    resultado.Id_formapago = int.Parse(t.Rows[0]["id_formapago"].ToString());
                }
                resultado.Id_agencia = int.Parse(t.Rows[0]["id_agencia"].ToString());
                resultado.Id_anunciante = int.Parse(t.Rows[0]["id_anunciante"].ToString());
                resultado.Importe_total = float.Parse(t.Rows[0]["importe_total"].ToString());
                resultado.Fecha_desde = DateTime.Parse(t.Rows[0]["fecha_desde"].ToString());
                resultado.Fecha_hasta = DateTime.Parse(t.Rows[0]["fecha_hasta"].ToString());
                resultado.Estado = int.Parse(t.Rows[0]["estado"].ToString());
                resultado.Observaciones = t.Rows[0]["observaciones"].ToString();
                if (t.Rows[0]["facturar_a"].ToString() != "")
                {
                    resultado.Facturar_a = int.Parse(t.Rows[0]["facturar_a"].ToString());
                }
                if (t.Rows[0]["agencia_nombre"].ToString() != "")
                {
                    resultado.Agencia_nombre = t.Rows[0]["agencia_nombre"].ToString();
                }
                if (t.Rows[0]["anunciante_nombre"].ToString() != "")
                {
                    resultado.Anunciante_nombre = t.Rows[0]["anunciante_nombre"].ToString();
                }
                if (t.Rows[0]["formapago_nombre"].ToString() != "")
                {
                    resultado.Formapago_nombre = t.Rows[0]["formapago_nombre"].ToString();
                }
            }
            return resultado;
        }

        //public static List<Convenios> getAll()
        //{
        //    string sqlCommand = @"select c.id_convenio, c.desc_convenio, c.id_formapago, c.id_agencia, c.id_anunciante, c.importe_total,
        //                            c.fecha_desde, c.fecha_hasta, c.estado, c.observaciones, c.facturar_a,
        //                            ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, fp.desc_formapago as formapago_nombre
        //                            from convenio_anual_precios c
        //                            left outer join contactos ag on ag.id_contacto = c.id_agencia
        //                            left outer join contactos an on an.id_contacto = c.id_anunciante
        //    left outer join formas_pago fp on fp.id_formapago = c.id_formapago";

        //    List<Convenios> col = new List<Convenios>();
        //    Convenios elem;
        //    DataTable t = DB.Select(sqlCommand);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        elem = new Convenios
        //        {
        //            Id_convenio = int.Parse(item["id_convenio"].ToString()),
        //            Desc_convenio = item["desc_convenio"].ToString(),
        //            Id_formapago = int.Parse(item["id_formapago"].ToString()),
        //            Id_agencia = int.Parse(item["id_agencia"].ToString()),
        //            Id_anunciante = int.Parse(item["id_anunciante"].ToString()),
        //            Importe_total = float.Parse(item["importe_total"].ToString()),
        //            Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString()),
        //            Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString()),
        //            Estado = int.Parse(item["estado"].ToString()),
        //            Observaciones = item["observaciones"].ToString(),
        //            Facturar_a = int.Parse(item["facturar_a"].ToString()),
        //            Agencia_nombre = item["agencia_nombre"].ToString(),
        //            Anunciante_nombre = item["anunciante_nombre"].ToString(),
        //            Formapago_nombre = item["formapago_nombre"].ToString()
        //        };
        //        col.Add(elem);
        //    }
        //    return col;
        //}

        public static List<Convenios> getAll()
        {
            string sqlCommand = @"select c.id_convenio, c.desc_convenio, c.id_formapago, c.id_agencia, c.id_anunciante, c.importe_total,
                                    c.fecha_desde, c.fecha_hasta, c.estado, c.observaciones, c.facturar_a,
                                    ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, fp.desc_formapago as formapago_nombre
                                    from convenio_anual_precios c
                                    left outer join contactos ag on ag.id_contacto = c.id_agencia
                                    left outer join contactos an on an.id_contacto = c.id_anunciante
								    left outer join formas_pago fp on fp.id_formapago = c.id_formapago";

            List<Convenios> col = new List<Convenios>();
            Convenios elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Convenios();

                elem.Id_convenio = int.Parse(item["id_convenio"].ToString());
                elem.Desc_convenio = item["desc_convenio"].ToString();
                if (item["id_formapago"].ToString() != "")
                {
                    elem.Id_formapago = int.Parse(item["id_formapago"].ToString());
                }
                elem.Id_agencia = int.Parse(item["id_agencia"].ToString());
                elem.Id_anunciante = int.Parse(item["id_anunciante"].ToString());
                elem.Importe_total = float.Parse(item["importe_total"].ToString());
                elem.Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString());
                elem.Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString());
                elem.Estado = int.Parse(item["estado"].ToString());
                elem.Observaciones = item["observaciones"].ToString();
                if (item["facturar_a"].ToString() != "")
                {
                    elem.Facturar_a = int.Parse(item["facturar_a"].ToString());
                }               
                if (item["agencia_nombre"].ToString() != "")
                {
                    elem.Agencia_nombre = item["agencia_nombre"].ToString();
                }
                if (item["anunciante_nombre"].ToString() != "")
                {
                    elem.Anunciante_nombre = item["anunciante_nombre"].ToString();
                }
                if (item["formapago_nombre"].ToString() != "")
                {
                    elem.Formapago_nombre = item["formapago_nombre"].ToString();
                }             

                col.Add(elem);
            }
            return col;
        }

        //public static List<Convenios> filter(List<Parametro> parametros)
        //{
        //    string sqlCommand = @"select c.id_convenio, c.desc_convenio, c.id_formapago, c.id_agencia, c.id_anunciante, c.importe_total,
        //                            c.fecha_desde, c.fecha_hasta, c.estado, c.observaciones, c.facturar_a,
        //                            ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, fp.desc_formapago as formapago_nombre
        //                            from convenio_anual_precios c
        //                            left outer join contactos ag on ag.id_contacto = c.id_agencia
        //                            left outer join contactos an on an.id_contacto = c.id_anunciante
        //    left outer join formas_pago fp on fp.id_formapago = c.id_formapago
        //                            where c.es_borrado = 0 ";
        //    string mifiltro = "";

        //    foreach (Parametro p in parametros)
        //    {
        //        if (p.Value.ToString() != "")
        //        {
        //            if ((p.ParameterName == "desc_convenio") && (p.Value.ToString() != ""))
        //                mifiltro = mifiltro + " and c.desc_convenio like '%" + p.Value + "%'";
        //            if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
        //            {
        //                DateTime fecha = DateTime.Parse(p.Value);
        //                string formatted = fecha.ToString("dd-MM-yyyy");
        //                mifiltro = mifiltro + " and fecha_desde >='" + formatted + "'";
        //            }
        //            if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
        //            {
        //                DateTime fecha = DateTime.Parse(p.Value);
        //                string formatted = fecha.ToString("dd-MM-yyyy");
        //                mifiltro = mifiltro + " and fecha_hasta <='" + formatted + "'";
        //            }
        //            if ((p.ParameterName == "agencia_nombre") && (p.Value.ToString() != ""))
        //                mifiltro = mifiltro + " and ag.razon_social like '%" + p.Value + "%'";
        //            if ((p.ParameterName == "anunciante_nombre") && (p.Value.ToString() != ""))
        //                mifiltro = mifiltro + " and an.razon_social like '%" + p.Value + "%'";
        //        }
        //    }
        //    List<Convenios> col = new List<Convenios>();
        //    Convenios elem;
        //    DataTable t = DB.Select(sqlCommand + mifiltro + " order by c.id_convenio desc");

        //    foreach (DataRow item in t.Rows)
        //    {
        //        elem = new Convenios
        //        {
        //            Id_convenio = int.Parse(item["id_convenio"].ToString()),
        //            Desc_convenio = item["desc_convenio"].ToString(),
        //            Id_formapago = int.Parse(item["id_formapago"].ToString()),
        //            Id_agencia = int.Parse(item["id_agencia"].ToString()),
        //            Id_anunciante = int.Parse(item["id_anunciante"].ToString()),
        //            Importe_total = float.Parse(item["importe_total"].ToString()),
        //            Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString()),
        //            Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString()),
        //            Estado = int.Parse(item["estado"].ToString()),
        //            Observaciones = item["observaciones"].ToString(),
        //            Facturar_a = int.Parse(item["facturar_a"].ToString()),
        //            Agencia_nombre = item["agencia_nombre"].ToString(),
        //            Anunciante_nombre = item["anunciante_nombre"].ToString(),
        //            Formapago_nombre = item["formapago_nombre"].ToString()
        //        };
        //        col.Add(elem);
        //    }
        //    return col;
        //}

        public static List<Convenios> filter(List<Parametro> parametros)
        {
            string sqlCommand = @"select c.id_convenio, c.desc_convenio, c.id_formapago, c.id_agencia, c.id_anunciante, c.importe_total,
                                    c.fecha_desde, c.fecha_hasta, c.estado, c.observaciones, c.facturar_a,
                                    ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, fp.desc_formapago as formapago_nombre
                                    from convenio_anual_precios c
                                    left outer join contactos ag on ag.id_contacto = c.id_agencia
                                    left outer join contactos an on an.id_contacto = c.id_anunciante
            left outer join formas_pago fp on fp.id_formapago = c.id_formapago
                                    where c.es_borrado = 0 ";
            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "desc_convenio") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and c.desc_convenio like '%" + p.Value + "%'";
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
                    if ((p.ParameterName == "agencia_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ag.razon_social like '%" + p.Value + "%'";
                    if ((p.ParameterName == "anunciante_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and an.razon_social like '%" + p.Value + "%'";
                }
            }
            List<Convenios> col = new List<Convenios>();
            Convenios elem;
            DataTable t = DB.Select(sqlCommand + mifiltro + " order by c.id_convenio desc");

            foreach (DataRow item in t.Rows)
            {
                elem = new Convenios();

                elem.Id_convenio = int.Parse(item["id_convenio"].ToString());
                elem.Desc_convenio = item["desc_convenio"].ToString();
                if (item["id_formapago"].ToString() != "")
                {
                    elem.Id_formapago = int.Parse(item["id_formapago"].ToString());
                }
                elem.Id_agencia = int.Parse(item["id_agencia"].ToString());
                elem.Id_anunciante = int.Parse(item["id_anunciante"].ToString());
                elem.Importe_total = float.Parse(item["importe_total"].ToString());
                elem.Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString());
                elem.Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString());
                elem.Estado = int.Parse(item["estado"].ToString());
                elem.Observaciones = item["observaciones"].ToString();
                if (item["facturar_a"].ToString() != "")
                {
                    elem.Facturar_a = int.Parse(item["facturar_a"].ToString());
                }
                if (item["agencia_nombre"].ToString() != "")
                {
                    elem.Agencia_nombre = item["agencia_nombre"].ToString();
                }
                if (item["anunciante_nombre"].ToString() != "")
                {
                    elem.Anunciante_nombre = item["anunciante_nombre"].ToString();
                }
                if (item["formapago_nombre"].ToString() != "")
                {
                    elem.Formapago_nombre = item["formapago_nombre"].ToString();
                }

                col.Add(elem);
            }
            return col;
        }

    }
}
