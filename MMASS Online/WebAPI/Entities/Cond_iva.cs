using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;

namespace WebApi.Entities
{
    public class Cond_iva
    {
        public int Id_cond_iva { get; set; }
        public string Desc_condicion_iva { get; set; }
        public string Codigo_sap { get; set; }
        public DateTime? Vig_desde { get; set; }
        public DateTime? Vig_hasta { get; set; }
        public float Alicuota1 { get; set; }
        public float Alicuota2 { get; set; }
        public bool Discrimina { get; set; }
        public float Percepcion1 { get; set; }
        public bool Es_borrado { get; set; }
        public int Cod_afip { get; set; }
        public string Tipo_cod { get; set; }
        public string Aux1 { get; set; }
        public string Aux2 { get; set; }
        public int Reqcuit { get; set; }
        public float ImpInc { get; set; }
        public static Cond_iva getById(int Id)
        {
            string sqlCommand = " select id_cond_iva, codigo_sap, desc_condicion_iva, vig_desde, vig_hasta, alicuota1, alicuota2," +
                                " percepcion1,es_borrado,cod_afip, tipo_cod, aux1, aux2,reqcuit, ImpInc " +
                                " from cond_iva where id_cond_iva = " + Id.ToString();
            Cond_iva resultado;
            resultado = new Cond_iva();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado.Id_cond_iva = int.Parse(t.Rows[0]["id_cond_iva"].ToString());
                resultado.Codigo_sap = t.Rows[0]["codigo_sap"].ToString();
                resultado.Desc_condicion_iva = t.Rows[0]["desc_condicion_iva"].ToString();
                resultado.Vig_desde = DateTime.Parse(t.Rows[0]["vig_desde"].ToString());
                resultado.Vig_hasta = DateTime.Parse(t.Rows[0]["vig_hasta"].ToString());
                resultado.Alicuota1 = float.Parse(t.Rows[0]["alicuota1"].ToString());
                resultado.Alicuota2 = float.Parse(t.Rows[0]["alicuota2"].ToString());
                resultado.Percepcion1 = float.Parse(t.Rows[0]["percepcion1"].ToString());
                resultado.Es_borrado = (t.Rows[0]["es_borrado"].ToString() == "1");
                resultado.Cod_afip = int.Parse(t.Rows[0]["cod_afip"].ToString());
                resultado.Tipo_cod = t.Rows[0]["tipo_cod"].ToString();
                resultado.Aux1 = t.Rows[0]["aux1"].ToString();
                resultado.Aux2 = t.Rows[0]["aux2"].ToString();
                resultado.Reqcuit = int.Parse(t.Rows[0]["reqcuit"].ToString());
                resultado.ImpInc = float.Parse(t.Rows[0]["ImpInc"].ToString());
            }
            return resultado;
        }

        public bool save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_cond_iva == 0)
            {
                sql = "insert into cond_iva (id_cond_iva, codigo_sap, desc_condicion_iva, vig_desde, vig_hasta, alicuota1, alicuota2," +
                " percepcion1,es_borrado,cod_afip, tipo_cod, aux1, aux2,reqcuit, ImpInc) " +
                " values (@id_cond_iva, @codigo_sap, @desc_condicion_iva, @vig_desde, @vig_hasta, @alicuota1, @alicuota2," +
                " @percepcion1,@es_borrado,@cod_afip, @tipo_cod, @aux1, @aux2, @reqcuit, @ImpInc) ";
                DataTable t = DB.Select("select max(id_cond_iva) as maximo from cond_iva");

                if (t.Rows.Count == 1)
                {
                    Id_cond_iva = int.Parse(t.Rows[0]["maximo"].ToString()) + 1;
                }
            }
            else
            {
                sql = "update cond_iva set codigo_sap = @codigo_sap, desc_condicion_iva = @desc_condicion_iva, vig_desde = @vig_desde, vig_hasta = @vig_hasta, alicuota1 = @alicuota1, alicuota2 = @alicuota2," +
                " percepcion1 = @percepcion1,es_borrado =@es_borrado,cod_afip = @cod_afip, tipo_cod=@tipo_cod, aux1 = @aux1, aux2 = @aux2,reqcuit =@reqcuit, ImpInc=@ImpInc " +
                " where id_cond_iva = @id_cond_iva";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_cond_iva",SqlDbType = SqlDbType.Int, Value = Id_cond_iva },
                    new SqlParameter()
                    { ParameterName="@Codigo_sap",SqlDbType = SqlDbType.NVarChar, Value = Codigo_sap },
                    new SqlParameter()
                    { ParameterName = "@Desc_condicion_iva", SqlDbType = SqlDbType.NVarChar, Value = Desc_condicion_iva },
                    new SqlParameter()
                    { ParameterName = "@vig_desde",SqlDbType = SqlDbType.DateTime, Value = Vig_desde},
                    new SqlParameter()
                    { ParameterName = "@vig_hasta",SqlDbType = SqlDbType.DateTime, Value = Vig_hasta},
                    new SqlParameter()
                    { ParameterName = "@Alicuota1",SqlDbType = SqlDbType.Float,Value = Alicuota1 },
                    new SqlParameter()
                    { ParameterName = "@alicuota2",SqlDbType = SqlDbType.Float, Value = Alicuota2 },
                    new SqlParameter()
                    { ParameterName = "@percepcion1", SqlDbType = SqlDbType.Float, Value = Percepcion1 },
                    new SqlParameter()
                    { ParameterName = "@es_borrado",SqlDbType = SqlDbType.Int,Value = Es_borrado },
                    new SqlParameter()
                    { ParameterName = "@Cod_afip", SqlDbType = SqlDbType.Int, Value = Cod_afip },
                    new SqlParameter()
                    { ParameterName = "@tipo_cod", SqlDbType = SqlDbType.NVarChar, Value = Tipo_cod},
                    new SqlParameter()
                    { ParameterName = "@aux1", SqlDbType = SqlDbType.NVarChar, Value = Aux1},
                    new SqlParameter()
                    { ParameterName = "@aux2", SqlDbType = SqlDbType.NVarChar, Value = Aux2},
                    new SqlParameter()
                    { ParameterName = "@reqcuit", SqlDbType = SqlDbType.Int, Value = Reqcuit},
                    new SqlParameter()
                    { ParameterName = "@ImpInc", SqlDbType = SqlDbType.Float, Value = ImpInc}
              };
            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static List<Cond_iva> getAll()
        {
            string sqlCommand = " select Id_cond_iva, Codigo_sap, Desc_condicion_iva, Vig_desde, Vig_hasta, Alicuota1, Alicuota2, Discrimina, Percepcion1, " +
                                " Es_borrado, Cod_afip, Tipo_cod, Aux1, Aux2, Reqcuit, ImpInc from cond_iva";

            List<Cond_iva> col = new List<Cond_iva>();
            Cond_iva elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Cond_iva
                {
                    Id_cond_iva = int.Parse(item["Id_cond_iva"].ToString()),
                    Codigo_sap = item["Codigo_sap"].ToString(),
                    Desc_condicion_iva = item["Desc_condicion_iva"].ToString(),
                    Vig_desde = DB.DFecha(item["Vig_desde"].ToString()),
                    Vig_hasta = DB.DFecha(item["Vig_hasta"].ToString()),
                    Alicuota1 = DB.DFloat(item["Alicuota1"].ToString()),
                    Alicuota2 = DB.DFloat(item["Alicuota2"].ToString()),
                    Discrimina = (item["Discrimina"].ToString()=="1"),
                    Percepcion1 = DB.DFloat(item["Percepcion1"].ToString()),                    
                    Cod_afip = DB.DInt(item["Cod_afip"].ToString()),
                    Tipo_cod = item["Tipo_cod"].ToString(),
                    Aux1 = item["Aux1"].ToString(),
                    Aux2 = item["Aux2"].ToString(),
                    Reqcuit = DB.DInt(item["Reqcuit"].ToString()),
                    ImpInc = DB.DInt(item["ImpInc"].ToString()),
                    Es_borrado = (item["Es_borrado"].ToString() == "1")
                };
                col.Add(elem);
            }
            return col;
        }
    }
}
