using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;
using System.Transactions;

namespace WebApi.Entities
{
    public class Forma_Pago
    {
        public int Id_formapago {get; set; }
        public string Desc_formapago { get; set; }
        public bool Es_canje { get; set; }
        public bool Es_borrado { get; set; }
        public string Abreviatura { get; set; }
        public string Cod_fac { get; set; }
        public string Cod_nc { get; set; }
        public int Id_formapago_grupo { get; set; }
        public string Aux1 { get; set; }
        public string Aux2 { get; set; }
        public int Por_bonif { get; set; }
        public bool Contado { get; set; }
        public static Forma_Pago getById(int Id)
        {
            string sqlCommand = "select id_formapago, desc_formapago, es_canje, es_borrado, abreviatura, cod_fac, cod_nc, id_formapago_grupo, aux1, aux2, por_bonif, contado from formas_pago where id_formapago = " + Id.ToString();
            Forma_Pago resultado;
            resultado = new Forma_Pago();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado.Id_formapago = DB.DInt(t.Rows[0]["id_formapago"].ToString());
                resultado.Desc_formapago = t.Rows[0]["desc_formapago"].ToString();
                resultado.Es_canje = (t.Rows[0]["es_canje"].ToString()=="1");
                resultado.Es_borrado = (t.Rows[0]["es_borrado"].ToString() == "1");
                resultado.Abreviatura = t.Rows[0]["abreviatura"].ToString();
                resultado.Cod_fac = t.Rows[0]["cod_fac"].ToString();
                resultado.Cod_nc = t.Rows[0]["cod_nc"].ToString();
                resultado.Id_formapago_grupo = DB.DInt(t.Rows[0]["id_formapago_grupo"].ToString());
                resultado.Aux1 = t.Rows[0]["aux1"].ToString();
                resultado.Aux2 = t.Rows[0]["aux2"].ToString();
                resultado.Por_bonif = DB.DInt(t.Rows[0]["por_bonif"].ToString());
                resultado.Contado = (t.Rows[0]["contado"].ToString() == "1");
            }
            return resultado;
        }

        private static Forma_Pago getForma_Pago(DataRow item)
        {
            Forma_Pago mi = new Forma_Pago
            {
                Id_formapago = DB.DInt(item["id_formapago"].ToString()),
                Desc_formapago = item["desc_formapago"].ToString(),
                Es_canje = (item["es_canje"].ToString() == "1"),
                Es_borrado = (item["es_borrado"].ToString() == "1"),
                Abreviatura = item["abreviatura"].ToString(),
                Cod_fac = item["cod_fac"].ToString(),
                Cod_nc = item["cod_nc"].ToString(),
                Id_formapago_grupo = DB.DInt(item["id_formapago_grupo"].ToString()),
                Aux1 = item["aux1"].ToString(),
                Aux2 = item["aux2"].ToString(),
                Por_bonif = DB.DInt(item["por_bonif"].ToString()),
                Contado = (item["contado"].ToString() == "1")
            };
            return mi;
        }

        public static List<Forma_Pago> getAll()
        {
            string sqlCommand = "select id_formapago, desc_formapago, es_canje, es_borrado, abreviatura, cod_fac, cod_nc, id_formapago_grupo," +
                                "aux1, aux2, por_bonif, contado from formas_pago where es_borrado = 0 ";

            List<Forma_Pago> col = new List<Forma_Pago>();
            Forma_Pago elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getForma_Pago(item);
                col.Add(elem);
            }
            return col;
        }

        public bool save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_formapago == 0)
            {
                sql = "insert into formas_pago(id_formapago, desc_formapago, es_canje, es_borrado, abreviatura, cod_fac, cod_nc, id_formapago_grupo, aux1, aux2, por_bonif, contado ) " +
                      "values (@id_formapago, @desc_formapago, @es_canje, 0, @abreviatura, @cod_fac, @cod_nc, @id_formapago_grupo, @aux1, @aux2, @por_bonif, @contado )";
                DataTable t = DB.Select("SELECT max(id_formapago) AS ULTIMO from formas_pago");
                if (t.Rows.Count == 1)
                {
                    Id_formapago = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
                }
            }
            else
            {
                sql = " update formas_pago set desc_formapago = @desc_formapago, es_canje = @es_canje, es_borrado = @es_borrado, abreviatura = @abreviatura, cod_fac = @cod_fac, cod_nc = @cod_nc, id_formapago_grupo = @id_formapago_grupo, aux1=@aux1, aux2=@aux2, por_bonif= @por_bonif, contado = @contado " +
                                 " where id_formapago = @id_formapago";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_formapago",SqlDbType = SqlDbType.Int, Value = Id_formapago },
                    new SqlParameter()
                    { ParameterName="@desc_formapago", SqlDbType = SqlDbType.NVarChar, Value = Desc_formapago},
                    new SqlParameter()
                    { ParameterName="@es_canje", SqlDbType = SqlDbType.Int, Value = Es_canje},
                    new SqlParameter()
                    { ParameterName="@abreviatura", SqlDbType = SqlDbType.NVarChar, Value = Abreviatura},
                    new SqlParameter()
                    { ParameterName="@cod_fac", SqlDbType = SqlDbType.NVarChar, Value = Cod_fac},
                    new SqlParameter()
                    { ParameterName="@cod_nc", SqlDbType = SqlDbType.NVarChar, Value = Cod_nc},
                    new SqlParameter()
                    { ParameterName="@id_formapago_grupo", SqlDbType = SqlDbType.Int, Value = Id_formapago_grupo},
                    new SqlParameter()
                    { ParameterName="@aux1", SqlDbType = SqlDbType.NVarChar, Value = Aux1},
                    new SqlParameter()
                    { ParameterName="@aux2", SqlDbType = SqlDbType.NVarChar, Value = Aux2},
                    new SqlParameter()
                    { ParameterName="@por_bonif", SqlDbType = SqlDbType.Float, Value = Por_bonif},
                    new SqlParameter()
                    { ParameterName="@contado", SqlDbType = SqlDbType.SmallInt, Value = Contado}
            };
            try
            {
                using (TransactionScope transaccion = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 2, 0)))
                {
                    // Grabo la Forma de pago
                    DB.Execute(sql, parametros);
                    transaccion.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool remove()
        {
            string sql = "";

            if (Id_formapago != 0)
            {
                sql = "update formas_pago set es_borrado = 1 where Id_formapago = " + Id_formapago.ToString();
                try
                {
                    using (TransactionScope transaccion = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 2, 0)))
                    {
                        DB.Execute(sql);
                        transaccion.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
