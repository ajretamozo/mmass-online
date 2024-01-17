using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Orden_presup_ap
    {
        public int Id_presup { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Nro_presup { get; set; }
        public DateTime? Fecha_alta { get; set; }
        public int Id_estado { get; set; }
        public int Id_moneda { get; set; }
        public float Cambio { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? Fecha_expiracion { get; set; }
        public string Descripcion { get; set; }
        public int Id_concepto_negocio { get; set; }
        public Contacto agencia { get; set; }
        public Contacto anunciante { get; set; }
        public Producto producto { get; set; }
        public string Agencia_nombre { get; set; }
        public string Anunciante_nombre { get; set; }
        public string Producto_nombre { get; set; }
        public int Facturar_a { get; set; }
        public int Id_facturar { get; set; }
        public int Id_condpagoap { get; set; }
        public int Id_ejecutivo { get; set; }
        public double Monto_bruto { get; set; }
        public float Porc_dto { get; set; }
        public double Primer_neto { get; set; }
        public float Imp_conf_nc { get; set; }
        public float Imp_conf_fc { get; set; }
        public double Seg_neto { get; set; }
        //public int bloqueado { get; set; }
        public List<Orden_presup_as> Detalles;
        public List<Orden_presup_pagos> FormasPago;
        public string UsuarioSesion { get; set; } // Variable usada para enviar el usuario en los mails de alerta
        public string LinkPresup { get; set; } // Variable usada para enviar el link en los mails de alerta
        public string SimboloMoneda { get; set; }
        public Usuario usuario { get; set; }

        public static Orden_presup_ap getOrden_presup_ap(DataRow item)
        {
            Orden_presup_ap mi = new Orden_presup_ap();
            mi.Id_presup = DB.DInt(item["Id_presup"].ToString());
            mi.Anio = DB.DInt(item["anio"].ToString());
            mi.Mes = DB.DInt(item["Mes"].ToString());
            mi.Nro_presup = DB.DInt(item["Nro_presup"].ToString());
            mi.Fecha = DB.DFecha(item["Fecha"].ToString());
            mi.Fecha_expiracion = DB.DFecha(item["Fecha_expiracion"].ToString());
            if (item["id_agencia"].ToString() != "")
                mi.agencia = Contacto.getContactoById(DB.DInt(item["id_agencia"].ToString()));
            if (item["id_anunciante"].ToString() != "")
                mi.anunciante = Contacto.getContactoById(DB.DInt(item["id_anunciante"].ToString()));
            if (item["id_producto"].ToString() != "")
                mi.producto = Producto.getById(DB.DInt(item["id_producto"].ToString()));
            mi.Id_condpagoap = DB.DInt(item["Id_condpagoap"].ToString());
            mi.Id_ejecutivo = DB.DInt(item["Id_ejecutivo"].ToString());
            mi.Facturar_a = DB.DInt(item["Facturar_a"].ToString());
            mi.Id_facturar = DB.DInt(item["Id_facturar"].ToString());
            mi.Monto_bruto = double.Parse(item["Monto_bruto"].ToString());
            mi.Primer_neto = double.Parse(item["Primer_neto"].ToString());
            mi.Porc_dto = DB.DFloat(item["Porc_dto"].ToString());
            mi.Seg_neto = double.Parse(item["Seg_neto"].ToString());
            mi.Fecha_alta = DB.DFecha(item["Fecha_alta"].ToString());
            mi.Id_concepto_negocio = DB.DInt(item["Id_concepto_negocio"].ToString());
            if (item["id_usuario"].ToString() != "")
            {
                mi.usuario = Usuario.getById(DB.DInt(item["id_usuario"].ToString()));
            }
            mi.Imp_conf_nc = DB.DFloat(item["Imp_conf_nc"].ToString());
            mi.Imp_conf_fc = DB.DFloat(item["Imp_conf_fc"].ToString());
            mi.Id_moneda = DB.DInt(item["Id_moneda"].ToString());
            mi.Cambio = DB.DFloat(item["Cambio"].ToString());
            mi.Id_estado = DB.DInt(item["Id_estado"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            //if (item["bloqueado"] != null)
            //{
            //    mi.bloqueado = DB.DInt(item["bloqueado"].ToString());
            //}
            //else
            //{
            //    mi.bloqueado = 0;
            //}
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
            if (item.Table.Columns["simbolo"] != null)
            {
                mi.SimboloMoneda = item["simbolo"].ToString();
            }
            return mi;
        }

        public static List<Orden_presup_ap> filter(List<Parametro> parametros)
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

            string sqlCommand = " select top 100 dg.*, m.simbolo, " +
                                " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre " +
                                " from orden_presup_ap dg " +
                                " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
                                " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
                                " left outer join productos p on p.id_producto = dg.id_producto " +
                                " left outer join moneda m on dg.id_moneda = m.id_moneda " +
                                " where 1 = 1 ";
            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "anio") && (p.Value.ToString() != ""))
                    {
                        mifiltro = mifiltro + " and dg.anio = " + p.Value;
                    }

                    if ((p.ParameterName == "mes") && (p.Value.ToString() != ""))
                    {
                        mifiltro = mifiltro + " and dg.mes = " + p.Value;
                    }

                    if ((p.ParameterName == "nro_presup") && (p.Value.ToString() != ""))
                    {
                        mifiltro = mifiltro + " and dg.nro_presup = " + p.Value;
                    }

                    if ((p.ParameterName == "descripcion") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.descripcion like '%" + p.Value + "%'";

                    if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha >= '" + p.Value.ToString() + "'";

                    if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha_expiracion <= '" + p.Value.ToString() + "'";

                    if ((p.ParameterName == "fecha_desde_d") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha <= '" + p.Value.ToString() + "'";

                    if ((p.ParameterName == "fecha_hasta_d") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.fecha_expiracion >= '" + p.Value.ToString() + "'";

                    if ((p.ParameterName == "agencia_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and ag.razon_social like '%" + p.Value + "%'";

                    if ((p.ParameterName == "anunciante_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and an.razon_social like '%" + p.Value + "%'";

                    if ((p.ParameterName == "anunciante_id") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.id_anunciante = " + p.Value;

                    if ((p.ParameterName == "producto_nombre") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and p.desc_producto like '%" + p.Value + "%'";

                    if ((p.ParameterName == "id_estado") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and dg.id_estado = " + p.Value;
                }
            }
            List<Orden_presup_ap> col = new List<Orden_presup_ap>();
            Orden_presup_ap elem;
            DataTable t = DB.Select(sqlCommand + mifiltro + " order by dg.id_presup desc");

            foreach (DataRow item in t.Rows)
            {
                elem = getOrden_presup_ap(item);

                if (fillDetalles)
                {
                    elem.Detalles = new List<Orden_presup_as>();
                    string strSql = " select * from orden_presup_as where id_presup = " + elem.Id_presup.ToString();
                    DataTable td = DB.Select(strSql);
                    Orden_presup_as det;
                    foreach (DataRow r in td.Rows)
                    {
                        det = Orden_presup_as.getOrden_presup_as(r);
                        elem.Detalles.Add(det);
                    }
                }
                col.Add(elem);
            }
            return col;
        }

        public static List<Orden_presup_ap> getAll()
        {
            string sqlCommand = " select top 100 dg.*, m.simbolo, " +
                                " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre " +
                                " from orden_presup_ap dg " +
                                " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
                                " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
                                " left outer join productos p on p.id_producto = dg.id_producto " +
                                " left outer join moneda m on dg.id_moneda = m.id_moneda " +
                                " order by dg.id_presup desc";
            List<Orden_presup_ap> col = new List<Orden_presup_ap>();
            Orden_presup_ap elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getOrden_presup_ap(item);
                col.Add(elem);
            }
            return col;
        }

        public static Orden_presup_ap getById(int Id)
        {
            string sqlCommand = " select top 100 dg.*, m.simbolo, " +
                                " ag.razon_social as agencia_nombre, an.razon_social as anunciante_nombre, p.desc_producto as producto_nombre " +
                                " from orden_presup_ap dg " +
                                " left outer join contactos ag on ag.id_contacto = dg.id_agencia " +
                                " left outer join contactos an on an.id_contacto = dg.id_anunciante " +
                                " left outer join productos p on p.id_producto = dg.id_producto " +
                                " left outer join moneda m on dg.id_moneda = m.id_moneda " +
                                " where dg.id_presup = " + Id.ToString();
            Orden_presup_ap resultado;
            resultado = new Orden_presup_ap();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getOrden_presup_ap(t.Rows[0]);
                // Detalles ...
                resultado.Detalles = new List<Orden_presup_as>();
                string strSql = " select d.*, p.desc_programa, c.desc_categoria, t.desc_tarifa from orden_presup_as d " +
                                "left join programas p on p.id_programa=d.id_programa " +
                                "left join categorias c on c.id_categoria=d.id_categoria " +
                                "left join tarifas t on t.id_tarifa=d.id_tarifa " +
                                "where id_presup = " + resultado.Id_presup.ToString();
                DataTable td = DB.Select(strSql);
                Orden_presup_as det;
                foreach (DataRow r in td.Rows)
                {
                    det = Orden_presup_as.getOrden_presup_as(r);
                    resultado.Detalles.Add(det);
                }
                // FormasPago
                resultado.FormasPago = new List<Orden_presup_pagos>();
                td = DB.Select("select id_presup, anio, mes, nro_presup, id_formapago, porcentaje " +
                            " from orden_presup_pagos where id_presup = " + resultado.Id_presup.ToString());
                Orden_presup_pagos dfp;
                foreach (DataRow r in td.Rows)
                {
                    dfp = new Orden_presup_pagos();
                    dfp.Id_presup = resultado.Id_presup;
                    dfp.Mes = DB.DInt(r["mes"].ToString());
                    dfp.Anio = DB.DInt(r["anio"].ToString());
                    dfp.Nro_presup = DB.DInt(r["nro_presup"].ToString());
                    dfp.Id_formapago = DB.DInt(r["id_formapago"].ToString());
                    dfp.Porcentaje = DB.DFloat(r["porcentaje"].ToString());
                    resultado.FormasPago.Add(dfp);
                }
            }
            return resultado;
        }

        public static int proximoNumeroPresup(int anio, int mes)
        {
            int proximo = 0;
            DataTable t = DB.Select("select IsNull(max(nro_presup),0) as ultimo from orden_presup_ap where mes = " + mes.ToString() + " and anio = " + anio.ToString());
            if (t.Rows.Count == 1)
            {
                proximo = int.Parse(t.Rows[0]["ultimo"].ToString()) + 1;
                //if (proximo == 1)
                //    proximo = 5001;
            }
            return proximo;
        }

        public Orden_presup_ap save()
        {
            try
            {
                string sql = "";
                Anio = Fecha.Value.Year;
                Mes = Fecha.Value.Month;
                //Id_moneda = 1;

                // Si es nuevo va insert, sino update
                if (Id_presup == 0)
                {

                    sql = "insert into orden_presup_ap (id_presup, anio, mes, nro_presup, fecha_alta, id_estado, id_moneda, cambio, fecha, fecha_expiracion, " +
                            " descripcion, id_concepto_negocio, id_agencia, id_anunciante, id_producto, facturar_a, id_condpagoap, id_ejecutivo, " +
                            " monto_bruto, porc_dto, primer_neto, imp_conf_nc, imp_conf_fc, seg_neto, id_usuario, id_facturar)" +
                            " values (@id_presup, @anio, @mes, @nro_presup, @fecha_alta, @id_estado, @id_moneda, @cambio, @fecha, @fecha_expiracion, " +
                            " @descripcion, @id_concepto_negocio, @id_agencia, @id_anunciante, @id_producto, @facturar_a, @id_condpagoap, @id_ejecutivo, " +
                            " @monto_bruto, @porc_dto, @primer_neto, @imp_conf_nc, @imp_conf_fc, @seg_neto, @id_usuario, @id_facturar)";

                    Fecha_alta = DateTime.Now;

                    Nro_presup = proximoNumeroPresup(Anio, Mes);

                    DataTable t = DB.Select("select IsNull(max(Id_presup),0) as ultimo from orden_presup_ap");
                    if (t.Rows.Count == 1)
                    {
                        Id_presup = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
                        if (Id_presup == 1)
                            Id_presup = 5001;
                    }
                }
                else
                {
                    sql = "update orden_presup_ap set id_estado=@id_estado, id_moneda=@id_moneda, cambio=@cambio, " +
                            " fecha=@fecha, fecha_expiracion=@fecha_expiracion, descripcion=@descripcion, id_concepto_negocio=@id_concepto_negocio," +
                            " id_agencia=@id_agencia, id_anunciante=@id_anunciante, id_producto=@id_producto, facturar_a=@facturar_a, " +
                            " id_condpagoap=@id_condpagoap, id_ejecutivo=@id_ejecutivo, monto_bruto=@monto_bruto, porc_dto=@porc_dto, " +
                            " primer_neto=@primer_neto, imp_conf_nc=@imp_conf_nc, imp_conf_fc=@imp_conf_fc, seg_neto=@seg_neto," +
                            " id_usuario=@id_usuario, id_facturar=@id_facturar " +
                            " where id_presup = @id_presup";
                }
                List<SqlParameter> parametros = new List<SqlParameter>();

                parametros.Add(new SqlParameter() { ParameterName = "@id_presup", SqlDbType = SqlDbType.Int, Value = Id_presup });
                if (Anio != 0)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@anio", SqlDbType = SqlDbType.Int, Value = Anio });
                }
                if (Mes != 0)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@mes", SqlDbType = SqlDbType.Int, Value = Mes });
                }
                parametros.Add(new SqlParameter() { ParameterName = "@nro_presup", SqlDbType = SqlDbType.Int, Value = Nro_presup });
                parametros.Add(new SqlParameter() { ParameterName = "@fecha", SqlDbType = SqlDbType.DateTime, Value = Fecha });
                parametros.Add(new SqlParameter() { ParameterName = "@fecha_expiracion", SqlDbType = SqlDbType.DateTime, Value = Fecha_expiracion });
                parametros.Add(new SqlParameter() { ParameterName = "@id_agencia", SqlDbType = SqlDbType.Int, Value = agencia.Id_contacto });
                parametros.Add(new SqlParameter() { ParameterName = "@id_anunciante", SqlDbType = SqlDbType.Int, Value = anunciante.Id_contacto });
                parametros.Add(new SqlParameter() { ParameterName = "@id_producto", SqlDbType = SqlDbType.Int, Value = producto.Id_producto });
                parametros.Add(new SqlParameter() { ParameterName = "@id_condpagoap", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_condpagoap) });
                parametros.Add(new SqlParameter() { ParameterName = "@facturar_a", SqlDbType = SqlDbType.Int, Value = DB.DInt(Facturar_a) });
                parametros.Add(new SqlParameter() { ParameterName = "@monto_bruto", SqlDbType = SqlDbType.Float, Value = Monto_bruto });
                parametros.Add(new SqlParameter() { ParameterName = "@primer_neto", SqlDbType = SqlDbType.Float, Value = Primer_neto });
                parametros.Add(new SqlParameter() { ParameterName = "@porc_dto", SqlDbType = SqlDbType.Float, Value = Porc_dto });
                parametros.Add(new SqlParameter() { ParameterName = "@seg_neto", SqlDbType = SqlDbType.Float, Value = Seg_neto });
                if (Fecha_alta != null)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@fecha_alta", SqlDbType = SqlDbType.DateTime, Value = Fecha_alta });
                }
                parametros.Add(new SqlParameter() { ParameterName = "@id_estado", SqlDbType = SqlDbType.Int, Value = Id_estado });
                parametros.Add(new SqlParameter() { ParameterName = "@id_concepto_negocio", SqlDbType = SqlDbType.Int, Value = DB.DInt(Id_concepto_negocio) });
                parametros.Add(new SqlParameter() { ParameterName = "@id_usuario", SqlDbType = SqlDbType.Int, Value = usuario.Id_usuario });
                parametros.Add(new SqlParameter() { ParameterName = "@imp_conf_nc", SqlDbType = SqlDbType.Float, Value = Imp_conf_nc });
                parametros.Add(new SqlParameter() { ParameterName = "@imp_conf_fc", SqlDbType = SqlDbType.Float, Value = Imp_conf_fc });
                parametros.Add(new SqlParameter() { ParameterName = "@id_moneda", SqlDbType = SqlDbType.Int, Value = Id_moneda });
                parametros.Add(new SqlParameter() { ParameterName = "@cambio", SqlDbType = SqlDbType.Float, Value = Cambio });
                if (Descripcion == "")
                {
                    Descripcion = anunciante.Nombre_com + "_" + Anio.ToString() + "_" + Mes.ToString() + "_" + Nro_presup;
                }
                parametros.Add(new SqlParameter() { ParameterName = "@descripcion", SqlDbType = SqlDbType.NVarChar, Value = Descripcion });
                parametros.Add(new SqlParameter() { ParameterName = "@id_ejecutivo", SqlDbType = SqlDbType.Int, Value = Id_ejecutivo });
                parametros.Add(new SqlParameter() { ParameterName = "@id_facturar", SqlDbType = SqlDbType.Int, Value = Id_facturar });

                using (TransactionScope transaccion = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 2, 0)))
                {
                    // Grabo Cabecera...
                    DB.Execute(sql, parametros);

                    //Recupero datos para el mensaje de guardado con éxito
                    DataTable t = DB.Select("SELECT anio, mes, nro_presup FROM orden_presup_ap WHERE id_presup = " + Id_presup.ToString());
                    if (t.Rows.Count == 1)
                    {
                        DataRow item = t.Rows[0];
                        Anio = DB.DInt(item["anio"].ToString());
                        Mes = DB.DInt(item["mes"].ToString());
                        Nro_presup = DB.DInt(item["nro_presup"].ToString());
                    }

                    // Grabo los Detalles...
                    DB.Execute("delete from orden_presup_as where id_presup = " + Id_presup.ToString());

                    int contadorDetalle = 1;

                    foreach (Orden_presup_as elem in Detalles)
                    {
                        elem.Id_presup = Id_presup;
                        elem.Id_detalle = contadorDetalle;
                        elem.Anio = Anio;
                        elem.Mes = Mes;
                        elem.Nro_presup = Nro_presup;
                        elem.save();
                        contadorDetalle++;
                    }

                    // Formas Pago
                    DB.Execute("delete from orden_presup_pagos where id_presup = " + Id_presup.ToString());
                    sql = "insert into orden_presup_pagos(id_presup, anio, mes, nro_presup, id_formapago, porcentaje, porcCanje, xCliente) " +
                          "values (@id_presup, @anio, @mes, @nro_presup, @id_formapago, @porcentaje, 0, 0)";

                    foreach (Orden_presup_pagos elem in FormasPago)
                    {
                        List<SqlParameter> parametrosf = new List<SqlParameter>()
                        {
                            new SqlParameter()
                            { ParameterName="@id_presup",SqlDbType = SqlDbType.Int, Value = Id_presup },
                            new SqlParameter()
                            { ParameterName="@anio",SqlDbType = SqlDbType.Int, Value = Anio },
                            new SqlParameter()
                            { ParameterName="@mes",SqlDbType = SqlDbType.Int, Value = Mes },
                            new SqlParameter()
                            { ParameterName="@nro_presup",SqlDbType = SqlDbType.Int, Value = Nro_presup },
                            new SqlParameter()
                            { ParameterName="@id_formapago",SqlDbType = SqlDbType.Int, Value = elem.Id_formapago },
                            new SqlParameter()
                            { ParameterName="@porcentaje",SqlDbType = SqlDbType.Float, Value = elem.Porcentaje }
                        };
                        DB.Execute(sql, parametrosf);
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

        //ADAPTAR PARA PRESUPUESTO
        public static void grabarLog(List<Parametro> datosLog)
        {
            string objeto = "";
            string clave = "";
            string accion = "";
            string descripcion = "";
            int idusuario = 0;

            string sql = @"insert into auditoria (fechahora, objeto, clave, accion, descripcion, idusuario, borrado) 
                           values(@fechahora, @objeto, @clave, @accion, @descripcion, @idusuario, 0)";

            foreach (Parametro p in datosLog)
            {
                if (p.Value.ToString() != "")
                {
                    if (p.ParameterName == "objeto")
                    {
                        objeto = p.Value;
                    }
                    if (p.ParameterName == "clave")
                    {
                        clave = p.Value;
                    }
                    if (p.ParameterName == "accion")
                    {
                        accion = p.Value;
                    }
                    if (p.ParameterName == "descripcion")
                    {
                        descripcion = p.Value;
                    }
                    if (p.ParameterName == "idusuario")
                    {
                        idusuario = int.Parse(p.Value);
                    }
                }
            }

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@fechahora",SqlDbType = SqlDbType.DateTime, Value = DateTime.Now }
            };
            if (objeto != "")
            {
                parametros.Add(new SqlParameter() { ParameterName = "@objeto", SqlDbType = SqlDbType.NVarChar, Value = objeto });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@objeto", SqlDbType = SqlDbType.NVarChar, Value = DBNull.Value });
            }
            if (clave != "")
            {
                parametros.Add(new SqlParameter() { ParameterName = "@clave", SqlDbType = SqlDbType.NVarChar, Value = clave });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@clave", SqlDbType = SqlDbType.NVarChar, Value = DBNull.Value });
            }
            if (accion != "")
            {
                parametros.Add(new SqlParameter() { ParameterName = "@accion", SqlDbType = SqlDbType.NVarChar, Value = accion });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@accion", SqlDbType = SqlDbType.NVarChar, Value = DBNull.Value });
            }
            if (descripcion != "")
            {
                parametros.Add(new SqlParameter() { ParameterName = "@descripcion", SqlDbType = SqlDbType.Text, Value = descripcion });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@descripcion", SqlDbType = SqlDbType.Text, Value = DBNull.Value });
            }
            if (idusuario != 0)
            {
                parametros.Add(new SqlParameter() { ParameterName = "@idusuario", SqlDbType = SqlDbType.Int, Value = idusuario });
            }
            else
            {
                parametros.Add(new SqlParameter() { ParameterName = "@idusuario", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
            }

            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool existePresupNombre(int idPresup, string nom)
        {
            bool resultado = false;
            string sqlCommand;

            if (idPresup == 0)
            {
                sqlCommand = "SELECT id_presup FROM orden_presup_ap WHERE descripcion like '" + nom + "'";
            }
            else
            {
                sqlCommand = "SELECT id_presup FROM orden_presup_ap WHERE descripcion like '" + nom + "' and id_presup != " + idPresup;
            }

            DataTable t = DB.Select(sqlCommand);
            if (t.Rows.Count == 1)
            {
                resultado = true;
            }
            return resultado;
        }

    }
}
