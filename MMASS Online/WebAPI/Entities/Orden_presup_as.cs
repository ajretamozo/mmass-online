using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Orden_presup_as
    {
        public int Id_presup { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Nro_presup { get; set; }
        public int Id_detalle { get; set; }
        public int Id_medio { get; set; }
        public int Id_tarifa { get; set; }
        public string Desc_tarifa { get; set; }
        public float Netomanual { get; set; }
        public DateTime? Fecha_desde { get; set; }
        public DateTime? Fecha_hasta { get; set; }
        public int Id_programa { get; set; }
        public string Desc_programa { get; set; }
        public int Id_categoria { get; set; }
        public string Desc_categoria { get; set; }
        public DateTime? Hs_desde { get; set; }
        public DateTime? Hs_hasta { get; set; }
        public double Imp_tarifa { get; set; }
        public int Cantidad { get; set; }
        public double Monto_bruto { get; set; }
        public float Porc_dto { get; set; }
        public double Monto_neto { get; set; }
        public float Porcconfnc { get; set; }
        public float Porcconffc { get; set; }
        public float Impconfnc { get; set; }
        public float Impconffc { get; set; }
        public int AmbitoTar { get; set; }
        public int Formausotarifa { get; set; }
        public string UsuarioSesion { get; set; } // Atributo usado para enviar el usuario en los mails de alerta


        public static Orden_presup_as getOrden_presup_as(DataRow item)
        {
            Orden_presup_as mi = new Orden_presup_as();
            mi.Id_presup = int.Parse(item["Id_presup"].ToString());
            mi.Id_detalle = int.Parse(item["Id_detalle"].ToString());
            mi.Id_medio = int.Parse(item["Id_medio"].ToString());
            mi.Anio = int.Parse(item["Anio"].ToString());
            mi.Mes = int.Parse(item["Mes"].ToString());
            mi.Nro_presup = int.Parse(item["Nro_presup"].ToString());
            mi.Fecha_desde = DB.DFecha(item["Fecha_desde"].ToString());
            mi.Fecha_hasta = DB.DFecha(item["Fecha_hasta"].ToString());
            mi.Hs_desde = DB.DFecha(item["Hs_desde"].ToString());
            mi.Hs_hasta = DB.DFecha(item["Hs_hasta"].ToString());
            if (item["Id_programa"].ToString() != "")
            {
                mi.Id_programa = int.Parse(item["Id_programa"].ToString());
            }
            mi.Desc_programa = item["desc_programa"].ToString();
            if (item["Id_tarifa"].ToString() != "")
            {
                mi.Id_tarifa = int.Parse(item["Id_tarifa"].ToString());
            }
            mi.Desc_tarifa = item["desc_tarifa"].ToString();
            mi.Netomanual = int.Parse(item["Netomanual"].ToString());
            mi.Id_categoria = int.Parse(item["Id_categoria"].ToString());
            mi.Desc_categoria = item["desc_categoria"].ToString();
            mi.Imp_tarifa = DB.DFloat(item["Imp_tarifa"].ToString());
            mi.Imp_tarifa = double.Parse(item["Imp_tarifa"].ToString());
            mi.Cantidad = DB.DInt(item["Cantidad"].ToString());
            mi.Monto_bruto = double.Parse(item["Monto_bruto"].ToString());
            mi.Porc_dto = DB.DFloat(item["Porc_dto"].ToString());
            mi.Monto_neto = double.Parse(item["Monto_neto"].ToString());
            mi.Netomanual = DB.DFloat(item["Netomanual"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.Porcconffc = DB.DFloat(item["Porcconffc"].ToString());
            mi.Impconfnc = DB.DFloat(item["Impconfnc"].ToString());
            mi.Impconffc = DB.DFloat(item["Impconffc"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.Porcconfnc = DB.DFloat(item["Porcconfnc"].ToString());
            mi.AmbitoTar = DB.DInt(item["ambitoTar"].ToString());
            mi.Formausotarifa = DB.DInt(item["formausotarifa"].ToString());

            return mi;
        }

        public bool save()
        {
            bool progisnull = (Id_programa == 0);

            string sql = "insert into orden_presup_as " +
            "(id_presup, id_detalle, id_medio, anio, mes, nro_presup, fecha_desde, fecha_hasta, hs_desde, hs_hasta,";
            if (!progisnull)
            {
                sql += " id_programa,";
            }
            sql += " id_tarifa, id_categoria, imp_tarifa, cantidad, monto_bruto, porc_dto, monto_neto, netomanual," +
            " porcconfnc, porcconffc, impconffc, impconfnc, ambitoTar, formausotarifa) " +
            " values (@id_presup, @id_detalle, @id_medio, @anio, @mes, @nro_presup, @fecha_desde, @fecha_hasta, @hs_desde, @hs_hasta,";
            if (!progisnull)
            {
                sql += " @id_programa,";
            }
            sql += " @id_tarifa, @id_categoria, @imp_tarifa, @cantidad, @monto_bruto, @porc_dto, @monto_neto, @netomanual," +
            "@porcconfnc, @porcconffc, @impconffc, @impconfnc, @ambitoTar, @formausotarifa)";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                    new SqlParameter()
                    { ParameterName="@id_presup",SqlDbType = SqlDbType.Int, Value = Id_presup },
                    new SqlParameter()
                    { ParameterName="@id_detalle",SqlDbType = SqlDbType.Int, Value = Id_detalle },
                    new SqlParameter()
                    { ParameterName="@id_medio",SqlDbType = SqlDbType.Int, Value = Id_medio },
                    new SqlParameter()
                    { ParameterName="@anio",SqlDbType = SqlDbType.Int, Value = Anio },
                    new SqlParameter()
                    { ParameterName="@mes",SqlDbType = SqlDbType.Int, Value = Mes },
                    new SqlParameter()
                    { ParameterName="@nro_presup",SqlDbType = SqlDbType.Int, Value = Nro_presup },
                    new SqlParameter()
                    { ParameterName="@fecha_desde",SqlDbType = SqlDbType.DateTime, Value = Fecha_desde },
                    new SqlParameter()
                    { ParameterName="@fecha_hasta",SqlDbType = SqlDbType.DateTime, Value = Fecha_hasta },
                    new SqlParameter()
                    { ParameterName="@hs_desde",SqlDbType = SqlDbType.DateTime, Value = Hs_desde },
                    new SqlParameter()
                    { ParameterName="@hs_hasta",SqlDbType = SqlDbType.DateTime, Value = Hs_hasta },
                    new SqlParameter()
                    {ParameterName = "@id_programa",SqlDbType = SqlDbType.NVarChar, Value = Id_programa},
                    new SqlParameter()
                    { ParameterName="@id_categoria",SqlDbType = SqlDbType.Int, Value = Id_categoria },
                    new SqlParameter()
                    { ParameterName="@id_tarifa",SqlDbType = SqlDbType.Int, Value = Id_tarifa},
                    new SqlParameter()
                    { ParameterName="@imp_tarifa",SqlDbType = SqlDbType.Float, Value = Imp_tarifa },
                    new SqlParameter()
                    { ParameterName="@cantidad",SqlDbType = SqlDbType.Int, Value = Cantidad },
                    new SqlParameter()
                    { ParameterName="@monto_bruto",SqlDbType = SqlDbType.Float, Value = Monto_bruto },
                    new SqlParameter()
                    { ParameterName="@porc_dto",SqlDbType = SqlDbType.Float, Value = Porc_dto },
                    new SqlParameter()
                    { ParameterName="@monto_neto",SqlDbType = SqlDbType.Float, Value = Monto_neto },
                    new SqlParameter()
                    { ParameterName="@netomanual",SqlDbType = SqlDbType.Int, Value = Netomanual },
                    new SqlParameter()
                    { ParameterName="@porcconfnc",SqlDbType = SqlDbType.Float, Value = Porcconfnc },
                    new SqlParameter()
                    { ParameterName="@porcconffc",SqlDbType = SqlDbType.Float, Value = Porcconffc },
                    new SqlParameter()
                    { ParameterName="@impconffc",SqlDbType = SqlDbType.Float, Value = Impconffc },
                    new SqlParameter()
                    { ParameterName="@impconfnc",SqlDbType = SqlDbType.Float, Value = Impconfnc },
                    new SqlParameter()
                    { ParameterName="@ambitoTar",SqlDbType = SqlDbType.Int, Value = AmbitoTar },
                    new SqlParameter()
                    { ParameterName="@formausotarifa",SqlDbType = SqlDbType.Int, Value = Formausotarifa }
            };           

            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return true;
        }

        public static List<Orden_presup_as> getByIdPresup(int idPtresup)
        {
            List<Orden_presup_as> detalles = new List<Orden_presup_as>();

            string strSql = " select * from orden_presup_as where id_presup = " + idPtresup.ToString();

            DataTable td = DB.Select(strSql);
            Orden_presup_as det;
            foreach (DataRow r in td.Rows)
            {
                det = getOrden_presup_as(r);
                detalles.Add(det);
            }
            return detalles;
        }

        public static List<Orden_presup_as> getAll()
        {
            List<Orden_presup_as> detalles = new List<Orden_presup_as>();

            string strSql = "SELECT * FROM orden_presup_as";

            DataTable td = DB.Select(strSql);
            Orden_presup_as det;
            foreach (DataRow r in td.Rows)
            {
                det = getOrden_presup_as(r);
                detalles.Add(det);
            }
            return detalles;
        }

    }
}

